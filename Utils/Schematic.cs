using System.IO;
using System.Text.Json;
using Structify.Content.Tiles;

namespace Structify;

public partial class Schematic
{
    public Vector2I Size { get; set; }
    public List<TileInfo> Tiles { get; set; } = [];
    
    private static HashSet<Vector2I> _furnitureOrigins;

    public static Schematic Create(Vector2I topLeft, int diffX, int diffY)
    {
        _furnitureOrigins = [];
        
        Schematic schematic = new()
        {
            Size = new Vector2I(diffX - 1, diffY - 1)
        };

        for (int x = topLeft.X + 1; x < topLeft.X + diffX; x++)
        {
            for (int y = topLeft.Y + 1; y < topLeft.Y + diffY; y++)
            {
                Vector2I worldPos = new(x, y);
                Vector2I localPos = worldPos - topLeft;
                Tile tile = Main.tile[x, y];

                // Replacement tiles
                if (tile.HasTile && tile.TileType == ModContent.TileType<SchematicReplace>())
                {
                    schematic.Tiles.Add(new TileInfo
                    {
                        IsReplaceTile = true,
                        TileType = tile.TileType,
                        HasTile = tile.HasTile,
                        Position = localPos
                    });

                    continue;
                }
                
                // Furniture Tiles
                if (Utils.IsFurnitureTile(tile.TileType))
                {
                    // 1) Get the “flat” style index
                    int flatStyle = TileObjectData.GetTileStyle(tile);

                    // 2) Fetch the base data so we can read limits
                    TileObjectData baseData = TileObjectData.GetTileData(tile.TileType, 0, 0);
                    if (baseData == null)
                        continue;

                    // 3) Determine wrapLimit (at least 1)
                    int wrapLimit = Math.Max(baseData.StyleWrapLimit, 1);

                    // 4) Split flatStyle into style-within-base & alternate index
                    int style = flatStyle % wrapLimit;
                    int alt   = flatStyle / wrapLimit;

                    // 5) Sanity‑check bounds before calling GetTileData(...):
                    //    - style must be < wrapLimit
                    //    - alt   must be <= AlternatesCount
                    if (style < 0 || style >= wrapLimit)
                        continue;
                    if (alt < 0 || alt > baseData.AlternatesCount)
                        continue;

                    // 6) Now safely fetch the exact data
                    TileObjectData data = TileObjectData.GetTileData(tile.TileType, style, alt);
                    if (data == null)
                        continue;

                    // 1) Compute the “frame index” of this cell…
                    int tileFrameX = tile.TileFrameX / 18;
                    int tileFrameY = tile.TileFrameY / 18;

                    // 2) Find how far this cell is from the object’s top‑left corner
                    int offsetX = tileFrameX % data.Width;
                    int offsetY = tileFrameY % data.Height;

                    // 3) Collapse back to the true top‑left in world‑space
                    int originX = x - offsetX;
                    int originY = y - offsetY;
                    var origin = new Vector2I(originX, originY);

                    // 8) Dedupe multi‑tile furniture
                    if (!_furnitureOrigins.Add(origin))
                        continue;

                    // 9) Record that one origin tile
                    var originLocal = origin - topLeft;
                    Tile originTile = Main.tile[originX, originY];

                    Main.NewText($"[Schematic] Captured furniture origin at {originLocal}", Color.Lime);

                    schematic.Tiles.Add(new TileInfo
                    {
                        WallType     = originTile.WallType,
                        TileType     = originTile.TileType,
                        Style        = flatStyle,    // for Paste()'s WorldGen.PlaceTile
                        LiquidAmount = originTile.LiquidAmount,
                        LiquidType   = originTile.LiquidType,
                        TileFrameX   = originTile.TileFrameX,
                        TileFrameY   = originTile.TileFrameY,
                        TileColor    = originTile.TileColor,
                        WallColor    = originTile.WallColor,
                        Slope        = (int)originTile.Slope,
                        HasTile      = originTile.HasTile,
                        Position     = originLocal
                    });

                    continue;
                }
                
                // All other tiles
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
                    Position = localPos
                });
            }
        }
        
        int chairID = TileID.Chairs;
        int tableID = TileID.Tables;
        int chairsSaved = schematic.Tiles.Count(t => t.TileType == chairID);
        int tablesSaved = schematic.Tiles.Count(t => t.TileType == tableID);
        Main.NewText($"[Debug Schematic] Chairs saved: {chairsSaved}, Tables saved: {tablesSaved}", Color.Yellow);

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
