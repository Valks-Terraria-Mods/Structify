namespace Structify;

public static class TileStyle
{
    private static readonly Dictionary<int, SpriteSheetDir> _spriteSheetDirs = new()
    {
        { TileID.Platforms, SpriteSheetDir.Vertical },
        { TileID.ClosedDoor, SpriteSheetDir.Vertical },
        { TileID.WorkBenches, SpriteSheetDir.Horizontal },
        // There are only 2 styles of gates (Left and Right)
        { TileID.TallGateClosed, SpriteSheetDir.Vertical },
        // There is only 1 style of trapdoor
        { TileID.TrapdoorClosed, SpriteSheetDir.Horizontal },
        { TileID.Beds, SpriteSheetDir.Vertical },
        { TileID.Tables, SpriteSheetDir.Horizontal },
        { TileID.Tables2, SpriteSheetDir.Horizontal },
        { TileID.Torches, SpriteSheetDir.Vertical },
        { TileID.Lamps, SpriteSheetDir.Vertical }
    };

    public static int CalculateStyle(TileInfo tileInfo)
    {
        int id = tileInfo.TileType;
        int frameX = tileInfo.TileFrameX;
        int frameY = tileInfo.TileFrameY;

        if (_spriteSheetDirs.ContainsKey(id))
        {
            return _spriteSheetDirs[id] == SpriteSheetDir.Horizontal ?
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
