using Structify.Common.Items;

namespace Structify.Content.Items;

public class Hellavator : StructureItem
{
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.StoneBlock, 100)
    ];

    protected override int ItemRarity => ItemRarityID.Red;

    public override bool UseTheItem(Player player, Point16 mPos)
    {
        PlaceLeftWall(mPos);
        PlaceRightWall(mPos);
        KillEverythingBetweenWalls(mPos);
        PlaceChain(mPos);
        PlaceBackgroundWalls(mPos);
        PlaceTorches(mPos);

        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            int xStart = mPos.X - 4;
            int width = 8;
            int worldHeight = Main.maxTilesY;

            // packets can only be sent in chunks
            for (int y = 0; y < worldHeight; y += 100)
            {
                // height of this slice: either 100, or whatever remains at the bottom
                int sliceHeight = Math.Min(100, worldHeight - y);

                NetMessage.SendTileSquare(
                    whoAmi: Main.myPlayer,  // or -1 on the server to broadcast
                    tileX: xStart,
                    tileY: y,
                    xSize: width,
                    ySize: sliceHeight
                );
            }
        }

        return true;
    }

    private static void PlaceLeftWall(Point16 mPos)
    {
        for (int x = -4; x < -2; x++)
        {
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                Point16 pos = mPos + new Point16(x, y);
                KillEverything(pos);
                PlaceTile(pos, TileID.StoneSlab);
            }
        }
    }

    private static void PlaceRightWall(Point16 mPos)
    {
        for (int x = 4; x > 2; x--)
        {
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                Point16 pos = mPos + new Point16(x, y);
                KillEverything(pos);
                PlaceTile(pos, TileID.StoneSlab);
            }
        }
    }

    private static void PlaceChain(Point16 mPos)
    {
        for (int y = 0; y < Main.maxTilesY; y++)
        {
            Point16 pos = mPos + new Point16(0, y);
            KillEverything(pos);
            PlaceTile(pos, TileID.Chain);
        }
    }

    private static void KillEverythingBetweenWalls(Point16 mPos)
    {
        for (int x = -2; x <= 2; x++)
        {
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                Point16 pos = mPos + new Point16(x, y);
                KillEverything(pos);
            }
        }
    }

    private static void PlaceBackgroundWalls(Point16 mPos)
    {
        // Place background walls
        for (int x = -2; x <= 2; x++)
        {
            for (int y = 1; y < Main.maxTilesY; y++)
            {
                Point16 pos = mPos + new Point16(x, y);
                PlaceWall(pos, WallID.StoneSlab);
            }
        }
    }

    private static void PlaceTorches(Point16 mPos)
    {
        // Place torches
        foreach (int x in new int[] { -2, 2 })
        {
            for (int y = 30; y < Main.maxTilesY; y += 30)
            {
                Point16 pos = mPos + new Point16(x, y);
                int redTorch = 2;
                PlaceTile(pos, TileID.Torches, redTorch);
            }
        }
    }

    private static void PlaceTile(Point16 pos, int tileId, int style = 0)
    {
        if (!IsInWorld(pos))
            return;

        WorldGen.PlaceTile(pos.X, pos.Y, tileId,
            mute: true,
            forced: true,
            plr: Main.myPlayer,
            style: style);
    }

    private static void PlaceWall(Point16 pos, int wallId)
    {
        if (!IsInWorld(pos))
            return;

        WorldGen.PlaceWall(pos.X, pos.Y, wallId,
            mute: true);
    }

    private static void KillEverything(Point16 pos)
    {
        if (!IsInWorld(pos))
            return;

        Tile tile = Main.tile[pos.X, pos.Y];

        tile.ResetToType(TileID.WoodBlock);
        tile.ClearEverything();
    }

    private static bool IsInWorld(Point16 pos)
    {
        return pos.X > 0 && pos.X < Main.maxTilesX - 1 && pos.Y > 0 && pos.Y < Main.maxTilesY - 1;
    }
}
