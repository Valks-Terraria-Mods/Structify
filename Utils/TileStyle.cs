namespace ValksStructures;

public static class TileStyle
{
    static readonly Dictionary<int, SpriteSheetDir> spriteSheetDirs = new()
    {
        { TileID.Platforms, SpriteSheetDir.Vertical }
    };

    public static int CalculateStyle(TileInfo tileInfo)
    {
        int id = tileInfo.TileType;
        int frameX = tileInfo.TileFrameX;
        int frameY = tileInfo.TileFrameY;

        if (spriteSheetDirs.ContainsKey(id))
        {
            return spriteSheetDirs[id] == SpriteSheetDir.Horizontal ?
                frameX / 18 : frameY / 18;
        }

        return frameX / 18;
    }
}

/// <summary>
/// SpriteDirection indicates if the sprite sheet is horizontal or vertical.
/// For example, if it is vertical then TileFrameY / 18 will indicate the
/// platform style while TileFrameX / 18 will indicate the variant of this
/// platform. 'Variant' as in is this the corner of a platform? Or maybe this
/// is a platform stairway.
/// </summary>
public enum SpriteSheetDir
{
    Horizontal,
    Vertical
}
