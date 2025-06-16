using StructureHelper.API;

namespace Structify.Common.Items;

public abstract class SchematicItem : StructureItem
{
    public abstract string SchematicName { get; }

    public override bool UseTheItem(Player player, Point16 mPos)
    {
        string path = $"Schematics/{SchematicName}.shstruct";

        Point16 dimensions = Generator.GetStructureDimensions(path, Mod);
        Point16 bottomLeftAnchor = mPos - new Point16(0, dimensions.Y - 1);

        Generator.GenerateStructure(path, bottomLeftAnchor, Mod);

        return true;
    }
}
