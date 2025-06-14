namespace Structify;

public class TileInfo
{
    public int WallType { get; set; }
    public int TileType { get; set; }
    public int LiquidType { get; set; }
    public int Style { get; set; }
    public byte LiquidAmount { get; set; }
    public byte TileColor { get; set; }
    public byte WallColor { get; set; }
    public int TileFrameX { get; set; }
    public int TileFrameY { get; set; }
    public int Slope { get; set; }
    public bool HasTile { get; set; }

    // Note: This property is not used when serialized
    public Vector2I Position { get; set; }
}
