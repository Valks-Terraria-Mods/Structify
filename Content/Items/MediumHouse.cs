namespace Structify.Content.Items;

public class MediumHouse : SchematicItem
{
    protected override string SchematicName => "MediumHouse1";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 100)
    ];
    protected override int ItemRarity => ItemRarityID.Pink;
}
