using StructureHelper.API;

namespace Structify.Utils;

public static class StructureUtils
{
    public static void Generate(string schematic, int offset, Mod mod, Point16 mPos)
    {
        string path = $"Schematics/{schematic}.shstruct";
        Point16 dimensions = GetDimensions(schematic, mod);
        Point16 origin = GetOrigin(offset, dimensions, mPos);
        
        Generator.GenerateStructure(path, origin, mod);
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
}