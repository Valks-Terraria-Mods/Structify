using System.IO;
using System.Text.Json;

namespace ValksStructures;

public partial class Schematic
{
    public Vector2I Size { get; set; }
    public List<TileInfo> Tiles { get; set; } = new();

    public static bool IsCurrentlyBuilding { get; private set; }

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
                    // This is not working correctly. Wrong styles are being retrieved.
                    Style = TileObjectData.GetTileStyle(tile),
                    LiquidAmount = tile.LiquidAmount,
                    LiquidType = tile.LiquidType,
                    TileFrameX = tile.TileFrameX,
                    TileFrameY = tile.TileFrameY,
                    TileColor = tile.TileColor,
                    WallColor = tile.WallColor,
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
            .GetFileStream($"Schematics/{fileName}.json");

        using StreamReader reader = new(stream);
        string json = reader.ReadToEnd();

        return JsonSerializer.Deserialize<Schematic>(json);
    }

    static void ExecuteAction()
    {
        if (actions.Count == 0)
        {
            VModSystem.Update -= ExecuteAction;
            IsCurrentlyBuilding = false;
            return;
        }

        actions[0]();
        actions.RemoveAt(0);
    }
}
