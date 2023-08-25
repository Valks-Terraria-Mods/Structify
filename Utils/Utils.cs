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

    public static bool IsFurnitureTile(int id) => id switch
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
        TileID.Chairs or
        TileID.Toilets => true,
        _ => false
    };
}
