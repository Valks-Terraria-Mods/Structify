namespace ValksStructures.Content.Items;

public class Bridge : SchematicItem
{
    protected override string SchematicName => "Bridge1";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.StoneBlock, 50)
    ];
    protected override int ItemRarity => ItemRarityID.Blue;
    protected override int VerticalOffset => 10;
}
