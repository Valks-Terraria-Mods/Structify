namespace Structify.Content.Items;

public abstract class SchematicItem : StructureItem
{
    protected abstract string SchematicName { get; }
    protected virtual int VerticalOffset { get; } = 0;

    public override void SetDefaults()
    {
        base.SetDefaults();
        // TODO: Load schematic here so we can show dusts on mouse hover
    }

    public override bool UseTheItem(Player player, Vector2I mPos)
    {
        Schematic schematic = Schematic.Load(SchematicName);

        if (schematic == null)
        {
            Main.NewText($"Could not find the '{SchematicName}' schematic");
            return false;
        }

        return Schematic.Paste(schematic, mPos, VerticalOffset);
    }
}
