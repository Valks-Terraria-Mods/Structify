using StructureHelper.API;

namespace Structify.Common.Items;

public abstract class SchematicItem : StructureItem
{
    protected abstract string SchematicName { get; }

    public override void SetDefaults()
    {
        base.SetDefaults();
        // TODO: Load schematic here so we can show dusts on mouse hover
    }

    public override bool UseTheItem(Player player, Point16 mPos)
    {
        string path = $"Schematics/{SchematicName}.shstruct";

        Point16 dim = Generator.GetStructureDimensions(path, Mod);

        Point16 origin = mPos - new Point16(0, (int)dim.Y);

        Generator.GenerateStructure(path, origin, Mod);

        return true;
    }
}
