using System.Diagnostics;
using System.IO;

namespace ValksStructures;

public static class Utils
{
    public static void OpenFolder(string path)
    {
        if (Directory.Exists(path))
            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true,
                Verb = "open"
            });
    }

    public static void KillTile(Vector2I pos)
    {
        int x = pos.X;
        int y = pos.Y;

        if (!WorldGen.CanKillTile(x, y))
            return;

        Tile tile = Main.tile[x, y];

        // Resetting the tile type before destroying it is VERY IMPORTANT.
        // If this is not here then placing structures over trees will
        // FREEZE THE GAME.
        tile.ResetToType(TileID.WoodBlock);
        WorldGen.KillTile(x, y, noItem: true);

        // Send the destroyed tile to other clients if multiplayer
        if (Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendTileSquare(Main.myPlayer, x, y);
    }

    static bool KillTree(int x)
    {
        int grassY = GetGrassYAtX(x);

        if (grassY == -1)
            return false;

        // Kill a tree if it exists here
        try
        {
            WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(
            x, grassY, TileID.Diamond);
        } catch
        {

        }

        return true;
    }

    /// <summary>
    /// Get the highest grass Y at given X
    /// </summary>
    static int GetGrassYAtX(int x)
    {
        int lowestTrunkY = -1;

        for (int y = 0; y < Main.maxTilesY; y++)
        {
            Tile tile = Main.tile[x, y];

            if (tile.HasTile)
            {
                if (TileID.Sets.IsATreeTrunk[tile.TileType])
                {
                    lowestTrunkY = y;
                }
            }
        }

        return lowestTrunkY;
    }

    public static void KillEverything(Vector2I pos)
    {
        if (!IsInWorld(pos))
            return;

        bool killedTree = KillTree(pos.X);

        if (!killedTree)
        {
            Tile tile = Main.tile[pos.X, pos.Y];
            tile.ClearEverything();
        }

        // Send the destroyed tile to other clients if multiplayer
        if (Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendTileSquare(Main.myPlayer, pos.X, pos.Y);
    }

    public static bool IsInWorld(Vector2I pos) =>
        pos.X > 0 && pos.X < Main.maxTilesX - 1 &&
        pos.Y > 0 && pos.Y < Main.maxTilesY - 1;

    public static bool IsFurnitureTile(int id)
    {
        _ = id switch
        {
            TileID.WorkBenches or
            TileID.OpenDoor or
            TileID.ClosedDoor or
            TileID.TallGateOpen or
            TileID.TallGateClosed or
            TileID.TrapdoorOpen or
            TileID.TrapdoorClosed or
            TileID.Beds or
            TileID.Tables or
            TileID.Tables2 or
            TileID.Torches or
            TileID.Lamps or
            TileID.Lampposts or
            TileID.DjinnLamp or
            TileID.LavaLamp or
            TileID.LogicGateLamp or
            TileID.PlasmaLamp or
            TileID.Chairs or
            TileID.Candles or
            TileID.Bookcases or
            TileID.Chandeliers or
            TileID.Switches or
            TileID.Sundial or
            TileID.TargetDummy or
            TileID.Containers or
            TileID.Containers2 or
            TileID.FakeContainers or
            TileID.FakeContainers2 or
            TileID.Extractinator or
            TileID.Furnaces or
            TileID.FoodPlatter or
            TileID.BubbleMachine or
            TileID.FogMachine or
            TileID.IceMachine or
            TileID.ImbuingStation or
            TileID.Toilets => true,
            _ => false
        };

        if (TileID.Sets.Paintings[id])
            return true;

        if (TileID.Sets.CanBeSatOnForPlayers[id])
            return true;

        if (TileID.Sets.RoomNeeds.CountsAsChair.Contains(id) ||
            TileID.Sets.RoomNeeds.CountsAsDoor.Contains(id) ||
            TileID.Sets.RoomNeeds.CountsAsTable.Contains(id) ||
            TileID.Sets.RoomNeeds.CountsAsTorch.Contains(id))
            return true;

        return false;
    }
}
