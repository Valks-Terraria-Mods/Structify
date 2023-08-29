namespace ValksStructures.Content.Items;

public abstract class SchematicItem : StructureItem
{
    protected abstract string SchematicName { get; }
    protected virtual int VerticalOffset { get; } = 0;

    public override void UseTheItem(Player player, Vector2I mPos)
    {
        Schematic schematic = Schematic.Load(SchematicName);

        if (schematic == null)
        {
            Main.NewText($"Could not find the '{SchematicName}' schematic");
            return;
        }

        Schematic.Paste(schematic, mPos, VerticalOffset);

        return;
    }
}
