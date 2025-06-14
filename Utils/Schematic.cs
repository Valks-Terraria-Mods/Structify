using System.IO;
using System.Text.Json;

namespace Structify;

public partial class Schematic
{
    public Vector2I Size { get; set; }
    public List<TileInfo> Tiles { get; set; } = [];

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

                if (tile.TileType == TileID.Tables ||
                    tile.TileType == TileID.Tables2)
                {
                    if (tile.TileFrameX != 18 || tile.TileFrameY != 18)
                    {
                        schematic.Tiles.Add(new TileInfo
                        {
                            Position = new Vector2I(x, y) - topLeft,
                            HasTile = false,
                            WallColor = tile.WallColor,
                            WallType = tile.WallType,
                            LiquidAmount = tile.LiquidAmount,
                            LiquidType = tile.LiquidType
                        });

                        continue;
                    }  
                }

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
                    HasTile = tile.HasTile,
                    Position = new Vector2I(x, y) - topLeft
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
        Structify mod = ModContent.GetInstance<Structify>();
        
        string resource = mod
            .GetFileNames()
            .FirstOrDefault(n => n
                .StartsWith("Schematics/", StringComparison.OrdinalIgnoreCase) && Path.GetFileNameWithoutExtension(n)
                .Equals(fileName, StringComparison.OrdinalIgnoreCase)
            );

        if (resource == null)
        {
            Main.NewText($"Could not find Schematic file {fileName}.");
            return null;
        }

        Stream stream = mod.GetFileStream(resource);

        using StreamReader reader = new(stream);
        string json = reader.ReadToEnd();

        return JsonSerializer.Deserialize<Schematic>(json);
    }
}
