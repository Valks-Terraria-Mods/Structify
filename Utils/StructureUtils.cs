using Structify.Common.Items;
using StructureHelper.API;

namespace Structify.Utils;

public static class StructureUtils
{
    public static void Generate(SchematicItem item, Point16 mPos)
    {
        string path = $"Schematics/{item.SchematicName}.shstruct";
        Point16 dimensions = GetDimensions(item);
        Point16 origin = GetOrigin(item, dimensions, mPos);
        
        Generator.GenerateStructure(path, origin, item.Mod);
    }

    public static Point16 GetDimensions(SchematicItem item)
    {
        return Generator.GetStructureDimensions($"Schematics/{item.SchematicName}.shstruct", item.Mod);
    }
    
    public static Point16 GetOrigin(SchematicItem item, Point16 dimensions, Point16 mPos)
    {
        Point16 bottomLeftAnchor = mPos - new Point16(0, dimensions.Y - 1 - item.VerticalOffset);
        return bottomLeftAnchor;
    }
}