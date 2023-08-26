namespace ValksStructures;

public partial class Schematic
{
    public static void Paste(Schematic schematic, int style, int vOffset = 0)
    {
        if (Building)
        {
            Main.NewText("Please wait for the current house to " +
                "finish building");

            return;
        }

        // Setup all variables
        Building = true;

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
            style, 
            furniture);

        // Place liquids
        actions.Add(() =>
        {
            PlaceLiquids(startPos, size, schematic, ref schematicTilesIndex);
        });

        // Ensure all tiles are sloped correctly
        SlopeAllTiles(
            startPos, 
            size,
            ref schematicTilesIndex,
            schematic);

        // Add all the furniture
        AddFurnitureTiles(furniture, style);

        // Construction will be built by one task at a time every frame
        VModSystem.Update += ExecuteAction;
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

                if (IsReplaceTile(tileInfo))
                    continue;

                actions.Add(() =>
                {
                    WorldGen.KillWall(x, y);
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
        int style,
        Dictionary<int, List<TileInfo>> furniture)
    {
        for (int i = 0; i < size.X; i++)
        {
            for (int j = 0; j < size.Y; j++)
            {
                TileInfo tileInfo = schematic.Tiles[schematicTilesIndex++];

                int x = startPos.X + i;
                int y = startPos.Y + j;

                // Replace the wood wall with the current style wall
                // Note that this is not perfect because the styles are all over the place
                if (style != 0)
                    ReplaceWall(tileInfo, WallID.Wood, 40 + style);

                actions.Add(() =>
                {
                    // Place walls
                    WorldGen.PlaceWall(x, y, tileInfo.WallType, 
                        mute: false);
                });

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
                    actions.Add(() =>
                    {
                        PlaceTile(x, y, tileInfo, style);
                    });
                }
            }
        }

        schematicTilesIndex = 0;
    }

    static void SlopeAllTiles(
        Vector2I startPos, 
        Vector2I size,
        ref int schematicTilesIndex,
        Schematic schematic)
    {
        // Final pass to ensure all tiles are sloped correctly
        for (int i = 0; i < size.X; i++)
        {
            for (int j = 0; j < size.Y; j++)
            {
                TileInfo tileInfo = schematic.Tiles[schematicTilesIndex++];

                int x = startPos.X + i;
                int y = startPos.Y + j;

                if (tileInfo.HasTile)
                {
                    actions.Add(() =>
                    {
                        SlopeTile(x, y, tileInfo);
                    });
                }
            }
        }

        schematicTilesIndex = 0;
    }

    static void AddFurnitureTiles(Dictionary<int, List<TileInfo>> furniture, int style)
    {
        // Otherwise chairs will not be placed properly
        if (furniture.ContainsKey(TileID.Chairs))
            furniture[TileID.Chairs].Reverse();

        foreach (List<TileInfo> furnitureList in furniture.Values)
            foreach (TileInfo tileInfo in furnitureList)
                AddFurnitureTile(tileInfo, style);
    }

    static void AddFurnitureTile(TileInfo tileInfo, int style)
    {
        // Open doors break surrounding tiles when placed in the world
        ReplaceTile(tileInfo, TileID.OpenDoor, TileID.ClosedDoor);
        ReplaceTile(tileInfo, TileID.TallGateOpen, TileID.TallGateClosed);
        ReplaceTile(tileInfo, TileID.TrapdoorOpen, TileID.TrapdoorClosed);

        int x = tileInfo.Position.X;
        int y = tileInfo.Position.Y;

        actions.Add(() =>
        {
            if (tileInfo.TileType == TileID.Chairs && style != 0)
            {
                // Chair styles are offset by 1
                // https://docs.google.com/spreadsheets/d/1b-12C9BrUURP_0pHN7QC-0wYFsSF1ZbWyN6SkWRO48A/edit?usp=sharing
                PlaceTile(x, y, tileInfo, style + 1);
            }
            else
            {
                PlaceTile(x, y, tileInfo, style);
            }
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

    static void PlaceTile(int x, int y, TileInfo tileInfo, int style)
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

        // Replace wood block with appropriate style
        // Note that this is not perfect because the styles are all over the place
        if (style != 0)
            ReplaceTile(tileInfo, TileID.WoodBlock, 156 + style);

        // Place tile (no effect for liquids)
        WorldGen.PlaceTile(x, y, tileInfo.TileType,
            mute: false,
            forced: true,
            plr: -1,
            style: style/*tileInfo.Style*/);

        // Paint the tile with the appropriate color
        tile.TileColor = tileInfo.TileColor;

        if (tileInfo.TileType is TileID.Chairs)
        {
            // WorldGen.PlaceTile(...) overwrites the TileFrameX so that is why
            // this is set after

            // TileFrameX and TileFrameY seem to break workbenches
            tile.TileFrameX = (short)tileInfo.TileFrameX;
            //tile.TileFrameY = (short)tileInfo.TileFrameY;
        }
    }

    static bool IsLiquid(TileInfo tileInfo) => tileInfo.LiquidAmount > 0;
}
