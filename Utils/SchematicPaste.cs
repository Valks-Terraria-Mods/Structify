namespace ValksStructures;

public partial class Schematic
{
    static readonly List<TileInfo> solidTiles = new();

    public static void Paste(Schematic schematic, int styleOffset, int vOffset = 0)
    {
        if (IsCurrentlyBuilding)
        {
            Main.NewText("Please wait for the current house to " +
                "finish building");

            return;
        }

        // Setup all variables
        IsCurrentlyBuilding = true;

        Vector2I size = schematic.Size;
        Vector2I startPos = new(
            (int)Main.MouseWorld.X / 16,
            (int)Main.MouseWorld.Y / 16 - size.Y + 1 + vOffset);

        Dictionary<int, List<TileInfo>> furniture = 
            PrepareFurnitureDictionary(schematic, size);

        int schematicTilesIndex = 0;

        // Destroy the area where the structure will be placed
        DestroyArea(
            startPos, 
            size, 
            schematic,
            ref schematicTilesIndex);

        // Place walls and tiles
        PlaceWallsAndTiles(
            startPos, 
            size, 
            schematic,
            ref schematicTilesIndex, 
            styleOffset, 
            furniture);

        // Place liquids
        actions.Add(() =>
        {
            PlaceLiquids(startPos, size, schematic, ref schematicTilesIndex);
        });

        // Ensure all tiles are sloped correctly
        SlopeAllTiles();

        // Add all the furniture
        AddFurnitureTiles(furniture, styleOffset);

        if (ModContent.GetInstance<Config>().BuildInstantly)
        {
            foreach (Action action in actions)
                action();

            actions.Clear();

            IsCurrentlyBuilding = false;
        }
        else
        {
            // Construction will be built by one task at a time every frame
            VModSystem.Update += ExecuteAction;
        }
    }

    static void PlaceLiquids(
        Vector2I startPos, 
        Vector2I size,
        Schematic schematic,
        ref int schematicTilesIndex)
    {
        for (int i = 0; i < size.X; i++)
        {
            for (int j = 0; j < size.Y; j++)
            {
                TileInfo tileInfo = schematic.Tiles[schematicTilesIndex++];

                int x = startPos.X + i;
                int y = startPos.Y + j;

                if (IsLiquid(tileInfo))
                {
                    WorldGen.PlaceLiquid(x, y, (byte)tileInfo.LiquidType,
                        tileInfo.LiquidAmount);
                };
            }
        }

        schematicTilesIndex = 0;
    }

    static Dictionary<int, List<TileInfo>> PrepareFurnitureDictionary(
        Schematic schematic, 
        Vector2I size)
    {
        Dictionary<int, List<TileInfo>> furniture = new();

        for (int i = 0; i < size.X * size.Y; i++)
        {
            TileInfo tileInfo = schematic.Tiles[i];
            int tileId = tileInfo.TileType;

            if (Utils.IsFurnitureTile(tileId) && !furniture.ContainsKey(tileId))
                furniture[tileInfo.TileType] = new();
        }

        return furniture;
    }

    static void DestroyArea(
        Vector2I startPos, 
        Vector2I size, 
        Schematic schematic,
        ref int schematicTilesIndex)
    {
        for (int i = 0; i < size.X; i++)
            for (int j = 0; j < size.Y; j++)
            {
                TileInfo tileInfo = schematic.Tiles[schematicTilesIndex++];

                int x = startPos.X + i;
                int y = startPos.Y + j;

                // Do not kill tile if it is a replace tile
                if (IsReplaceTile(tileInfo))
                    continue;

                // Do not destroy a non-existent tile
                if (!Main.tile[x, y].HasTile)
                    continue;

                actions.Add(() =>
                {
                    WorldGen.KillTile(x, y,
                        fail: false,
                        effectOnly: false,
                        noItem: true);
                });
            }

        schematicTilesIndex = 0;
    }
    
    static bool IsReplaceTile(TileInfo tileInfo) =>
        tileInfo.TileType == 
        ModContent.TileType<Content.Tiles.SchematicReplace>();

    static void PlaceWallsAndTiles(
        Vector2I startPos, 
        Vector2I size,
        Schematic schematic,
        ref int schematicTilesIndex,
        int styleOffset,
        Dictionary<int, List<TileInfo>> furniture)
    {
        // Reset solid tiles dictionary
        solidTiles.Clear();

        for (int i = 0; i < size.X; i++)
        {
            for (int j = 0; j < size.Y; j++)
            {
                TileInfo tileInfo = schematic.Tiles[schematicTilesIndex++];
                
                // This is a replace tile, don't place anything here
                if (IsReplaceTile(tileInfo))
                    continue;

                int x = startPos.X + i;
                int y = startPos.Y + j;

                // Replace the wood wall with the current style wall
                // Note that this is not perfect because the styles are all over the place
                if (styleOffset != 0)
                    ReplaceWall(tileInfo, WallID.Wood, 40 + styleOffset);

                // Only place walls if wall exists in this tileInfo
                if (tileInfo.WallType != 0)
                {
                    actions.Add(() =>
                    {
                        if (Main.tile[x, y].WallType == 0)
                        {
                            // No wall here, so place one
                            WorldGen.PlaceWall(x, y, tileInfo.WallType,
                                mute: true);
                        }
                        else
                        {
                            // Wall exists here, replace it
                            WorldGen.ReplaceWall(x, y, (ushort)tileInfo.WallType);
                        }

                        WorldGen.paintWall(x, y, tileInfo.WallColor);
                    });
                }

                // Do not add furniture tiles right now
                if (Utils.IsFurnitureTile(tileInfo.TileType))
                {
                    // Pass over the position
                    tileInfo.Position = new Vector2I(x, y);

                    // Keep track of the furniture tile to be added later
                    furniture[tileInfo.TileType].Add(tileInfo);

                    // This is a furniture tile so skip it
                    continue;
                }

                // Place solid tiles
                if (tileInfo.HasTile)
                {
                    // Pass over the position
                    tileInfo.Position = new Vector2I(x, y);

                    // Keep track of solid tiles for later use with slope
                    solidTiles.Add(tileInfo);

                    actions.Add(() =>
                    {
                        PlaceTile(x, y, tileInfo, styleOffset);
                    });
                }
            }
        }

        schematicTilesIndex = 0;
    }

    static void SlopeAllTiles()
    {
        // Final pass to ensure all tiles are sloped correctly
        foreach (TileInfo solidTile in solidTiles)
        {
            actions.Add(() =>
            {
                Vector2I pos = solidTile.Position;
                SlopeTile(pos.X, pos.Y, solidTile);
            });
        }
    }

    static void AddFurnitureTiles(Dictionary<int, List<TileInfo>> furniture, int styleOffset)
    {
        // Otherwise chairs will not be placed properly
        if (furniture.ContainsKey(TileID.Chairs))
            furniture[TileID.Chairs].Reverse();

        foreach (List<TileInfo> furnitureList in furniture.Values)
            foreach (TileInfo tileInfo in furnitureList)
                AddFurnitureTile(tileInfo, styleOffset);
    }

    static void AddFurnitureTile(TileInfo tileInfo, int styleOffset)
    {
        // Open doors break surrounding tiles when placed in the world
        ReplaceTile(tileInfo, TileID.OpenDoor, TileID.ClosedDoor);
        ReplaceTile(tileInfo, TileID.TallGateOpen, TileID.TallGateClosed);
        ReplaceTile(tileInfo, TileID.TrapdoorOpen, TileID.TrapdoorClosed);

        int x = tileInfo.Position.X;
        int y = tileInfo.Position.Y;

        actions.Add(() =>
        {
            PlaceTile(x, y, tileInfo, styleOffset);
        });
    }

    static void ReplaceWall(TileInfo tileInfo, int oldWall, int newWall)
    {
        if (tileInfo.WallType == oldWall)
            tileInfo.WallType = newWall;
    }

    static void ReplaceTile(TileInfo tileInfo, int oldTile, int newTile)
    {
        if (tileInfo.TileType == oldTile)
            tileInfo.TileType = newTile;
    }

    static void SlopeTile(int x, int y, TileInfo tileInfo)
    {
        WorldGen.SlopeTile(x, y, tileInfo.Slope);
    }

    static void PlaceTile(int x, int y, TileInfo tileInfo, int styleOffset)
    {
        if (IsReplaceTile(tileInfo))
            return;

        Tile tile = Main.tile[x, y];

        // Empty tile so clear this tile
        if (!tileInfo.HasTile)
        {
            tile.ClearTile();
            return;
        }

        // Get the style of this tile
        int style = TileStyle.CalculateStyle(tileInfo);

        // Place tile (no effect for liquids)
        WorldGen.PlaceTile(x, y, tileInfo.TileType,
            mute: true,
            forced: true,
            plr: -1,
            style: style + styleOffset);

        // Paint the tile with the appropriate color
        tile.TileColor = tileInfo.TileColor;

        if (tileInfo.TileType is TileID.Chairs)
        {
            // WorldGen.PlaceTile(...) overwrites the TileFrameX so that is why
            // this is set after

            // TileFrameX and TileFrameY seem to break workbenches
            // and other pieces of furniture
            tile.TileFrameX = (short)tileInfo.TileFrameX;
            //tile.TileFrameY = (short)tileInfo.TileFrameY;
        }
    }

    static bool IsLiquid(TileInfo tileInfo) => tileInfo.LiquidAmount > 0;
}
