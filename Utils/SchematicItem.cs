namespace ValksStructures.Content.Items;

public abstract class SchematicItem : StructureItem
{
    protected abstract string SchematicName { get; }
    protected virtual int VerticalOffset { get; } = 0;

    public override void UseItem(Player player)
    {
        Schematic schematic = Schematic.Load(SchematicName);

        if (schematic == null)
        {
            Main.NewText($"Could not find the '{SchematicName}' schematic");
            return;
        }

        Schematic.Paste(schematic,
            styleOffset: ModContent.GetInstance<Config>().BuildStyle,
            vOffset: VerticalOffset);
    }
}
