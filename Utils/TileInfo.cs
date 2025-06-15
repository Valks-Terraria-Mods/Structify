namespace Structify;

public class TileInfo
{
    // Init properties
    public int WallType { get; init; }
    public int LiquidType { get; init; }
    public int Style { get; init; }
    public byte LiquidAmount { get; init; }
    public byte TileColor { get; init; }
    public byte WallColor { get; init; }
    public int TileFrameX { get; init; }
    public int TileFrameY { get; init; }
    public int Slope { get; init; }
    public bool HasTile { get; init; }
    public bool IsReplaceTile { get; init; }
    
    // Properties that have get AND set
    public int TileType { get; set; }

    // Note: This property is not used when serialized
    public Vector2I Position { get; init; }
}
