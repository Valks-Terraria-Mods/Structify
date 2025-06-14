namespace Structify.Content.Items;

public class SmallHouse : SchematicItem
{
    protected override string SchematicName => "SmallHouse1";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 50)
    ];
    protected override int ItemRarity => ItemRarityID.LightRed;
}
