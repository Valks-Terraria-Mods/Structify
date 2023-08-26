using System.Diagnostics;
using System.IO;
using Terraria.ID;

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
