using StructureHelper.API;
using StructureHelper.Models;

namespace Structify.Utils;

public static class StructureUtils
{
    private const int MaxUndoHistory = 25;

    private readonly record struct PlacementSnapshot(StructureData Data, Point16 Origin);
    private static readonly List<PlacementSnapshot> UndoHistory = [];

    public static void Generate(string schematic, int offset, Mod mod, Point16 mPos)
    {
        string path = $"Schematics/{schematic}.shstruct";
        Point16 dimensions = GetDimensions(schematic, mod);
        Point16 origin = GetOrigin(offset, dimensions, mPos);

        CaptureUndoSnapshot(origin, dimensions);
        
        Generator.GenerateStructure(path, origin, mod);
    }

    public static bool UndoLastPlacement()
    {
        if (UndoHistory.Count == 0)
            return false;

        PlacementSnapshot snapshot = UndoHistory[^1];
        UndoHistory.RemoveAt(UndoHistory.Count - 1);

        try
        {
            Generator.GenerateFromData(snapshot.Data, snapshot.Origin);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static Point16 GetDimensions(string schematic, Mod mod)
    {
        return Generator.GetStructureDimensions($"Schematics/{schematic}.shstruct", mod);
    }
    
    public static Point16 GetOrigin(int offset, Point16 dimensions, Point16 mPos)
    {
        Point16 bottomLeftAnchor = mPos - new Point16(0, dimensions.Y - 1 - offset);
        return bottomLeftAnchor;
    }

    private static void CaptureUndoSnapshot(Point16 origin, Point16 dimensions)
    {
        if (dimensions.X <= 0 || dimensions.Y <= 0)
            return;

        if (origin.X < 0 || origin.Y < 0 || origin.X + dimensions.X >= Main.maxTilesX || origin.Y + dimensions.Y >= Main.maxTilesY)
            return;

        UndoHistory.Add(new PlacementSnapshot(StructureData.FromWorld(origin.X, origin.Y, dimensions.X, dimensions.Y), origin));

        if (UndoHistory.Count > MaxUndoHistory)
            UndoHistory.RemoveAt(0);
    }
}