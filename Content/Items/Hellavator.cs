using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public enum MessageType : byte
{
    SpawnHellavator
}

public class Hellavator : StructureItem
{
    public override string ItemName => "Hellavator";
    public override string Description => "An elevator to hell.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int ItemRarity => ItemRarityID.Red;

    protected override bool UseTheItem(Player player, Point16 mPos)
    {
        // If I'm a multiplayer client, ask the server to build the hellavator
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)MessageType.SpawnHellavator);
            packet.Write((short)mPos.X);
            packet.Write((short)mPos.Y);
            packet.Send(); // to server
            return true;
        }

        // Singleplayer
        BuildHellavator(mPos);
        return true;
    }

    public static void BuildHellavator(Point16 mPos)
    {
        PlaceLeftWall(mPos);
        PlaceRightWall(mPos);
        KillEverythingBetweenWalls(mPos);
        PlaceChain(mPos);
        PlaceBackgroundWalls(mPos);
        PlaceTorches(mPos);
    }

    private static void PlaceLeftWall(Point16 mPos)
    {
        for (int x = -4; x < -2; x++)
        {
            for (int y = mPos.Y; y < Main.maxTilesY; y++)
            {
                Point16 pos = new Point16(mPos.X + x, y);
                KillEverything(pos);
                PlaceTile(pos, TileID.StoneSlab);
            }
        }
    }

    private static void PlaceRightWall(Point16 mPos)
    {
        for (int x = 4; x > 2; x--)
        {
            for (int y = mPos.Y; y < Main.maxTilesY; y++)
            {
                Point16 pos = new Point16(mPos.X + x, y);
                KillEverything(pos);
                PlaceTile(pos, TileID.StoneSlab);
            }
        }
    }

    private static void PlaceChain(Point16 mPos)
    {
        for (int y = mPos.Y; y < Main.maxTilesY; y++)
        {
            Point16 pos = new Point16(mPos.X, y);

            if (!IsInWorld(pos))
                continue;

            KillEverything(pos);
            PlaceTile(pos, TileID.Chain);
        }
    }

    private static void KillEverythingBetweenWalls(Point16 mPos)
    {
        for (int x = -2; x <= 2; x++)
        {
            for (int y = mPos.Y; y < Main.maxTilesY; y++)
            {
                Point16 pos = new Point16(mPos.X + x, y);
                KillEverything(pos);
            }
        }
    }

    private static void PlaceBackgroundWalls(Point16 mPos)
    {
        // Place background walls
        for (int x = -2; x <= 2; x++)
        {
            for (int y = mPos.Y + 1; y < Main.maxTilesY; y++)
            {
                Point16 pos = new Point16(mPos.X + x, y);
                PlaceWall(pos, WallID.StoneSlab);
            }
        }
    }

    private static void PlaceTorches(Point16 mPos)
    {
        const int RedTorchStyle = 2;
        
        // Place torches
        foreach (int x in new int[] { -2, 2 })
        {
            for (int y = mPos.Y + 30; y < Main.maxTilesY; y += 30)
            {
                Point16 pos = new Point16(mPos.X + x, y);
                PlaceTile(pos, TileID.Torches, RedTorchStyle);
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
