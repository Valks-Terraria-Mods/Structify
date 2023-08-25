using System.IO;
using System.Text.Json;

namespace ValksStructures;

public class Schematic
{
    public Vector2I Size { get; set; }
    public List<TileInfo> Tiles { get; set; } = new();

    public static bool Building { get; private set; }

    static readonly List<Action> actions = new();

    public static Schematic Create(Vector2I topLeft, int diffX, int diffY)
    {
        Schematic schematic = new()
        {
            Size = new Vector2I(diffX - 1, diffY - 1)
        };

        for (int x = topLeft.X + 1; x < topLeft.X + diffX; x++)
        {
            for (int y = topLeft.Y + 1; y < topLeft.Y + diffY; y++)
            {
                Tile tile = Main.tile[x, y];

                schematic.Tiles.Add(new TileInfo
                {
                    WallType = tile.WallType,
                    TileType = tile.TileType,
                    TileFrameX = tile.TileFrameX,
                    TileFrameY = tile.TileFrameY,
                    Slope = (int)tile.Slope,
                    HasTile = tile.HasTile
                });
            }
        }

        return schematic;
    }

    public static void Save(Schematic schematic, string savePath, string fileName)
    {
        string json = JsonSerializer.Serialize(schematic, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        Directory.CreateDirectory(savePath);
        File.WriteAllText($"{savePath}/{fileName}.json", json);
    }

    public static Schematic Load(string fileName)
    {
        Stream stream = ModContent
            .GetInstance<ValksStructures>()
            .GetFileStream($"{fileName}.json");

        using StreamReader reader = new(stream);
        string json = reader.ReadToEnd();

        return JsonSerializer.Deserialize<Schematic>(json);
    }

    public static void Paste(Schematic schematic, int style)
    {
        if (Building)
        {
            Main.NewText("Please wait for the current house to finish building");
            return;
        }

        Building = true;

        Vector2I size = schematic.Size;

        Vector2I startPos = new(
            (int)Main.MouseWorld.X / 16,
            (int)Main.MouseWorld.Y / 16 - size.Y + 1);

        int index = 0;

        Dictionary<int, List<TileInfo>> furniture = new();

        // Prepare the furniture dictionary
        for (int i = 0; i < size.X * size.Y; i++)
        {
            TileInfo tileInfo = schematic.Tiles[i];
            int tileId = tileInfo.TileType;

            if (Utils.IsFurnitureTile(tileId) && !furniture.ContainsKey(tileId))
                furniture[tileInfo.TileType] = new();
        }

        for (int i = 0; i < size.X; i++)
            for (int j = 0; j < size.Y; j++)
            {
                int x = startPos.X + i;
                int y = startPos.Y + j;

                actions.Add(() =>
                {
                    WorldGen.KillWall(x, y);
                    WorldGen.KillTile(x, y, 
                        fail: false, 
                        effectOnly: false, 
                        noItem: true);
                });
            }

        for (int i = 0; i < size.X; i++)
        {
            for (int j = 0; j < size.Y; j++)
            {
                TileInfo tileInfo = schematic.Tiles[index++];

                int x = startPos.X + i;
                int y = startPos.Y + j;

                // Replace the wood wall with the current style wall
                // Note that this is not perfect because the styles are all over the place
                if (style != 0)
                    ReplaceWall(tileInfo, WallID.Wood, 40 + style);

                actions.Add(() =>
                {
                    // Place walls
                    WorldGen.PlaceWall(x, y, tileInfo.WallType, false);
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
                actions.Add(() =>
                {
                    PlaceTile(x, y, tileInfo, style);
                });
            }
        }

        AddFurnitureTiles(furniture, style);

        VModSystem.Update += ExecuteAction;
    }

    static void ExecuteAction()
    {
        if (actions.Count == 0)
        {
            VModSystem.Update -= ExecuteAction;
            Building = false;
            return;
        }

        actions[0]();
        actions.RemoveAt(0);
    }

    static void AddFurnitureTiles(Dictionary<int, List<TileInfo>> furniture, int style)
    {
        // Otherwise chairs will not be placed properly
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

    static void PlaceTile(int x, int y, TileInfo tileInfo, int style)
    {
        // Empty tile so clear this tile
        if (!tileInfo.HasTile)
        {
            Main.tile[x, y].ClearTile();
            return;
        }

        // Replace wood block with appropriate style
        // Note that this is not perfect because the styles are all over the place
        if (style != 0)
            ReplaceTile(tileInfo, TileID.WoodBlock, 156 + style);

        // Place tile
        WorldGen.PlaceTile(x, y, tileInfo.TileType,
                mute: true,
                forced: true,
                plr: -1,
                style: style);

        WorldGen.SlopeTile(x, y, tileInfo.Slope);

        if (tileInfo.TileType is TileID.Chairs)
        {
            // WorldGen.PlaceTile(...) overwrites the TileFrameX so that is why
            // this is set after

            // TileFrameX and TileFrameY seem to break workbenches
            Main.tile[x, y].TileFrameX = (short)tileInfo.TileFrameX;
            //Main.tile[x, y].TileFrameY = (short)tileInfo.TileFrameY;
        }
    }
}
