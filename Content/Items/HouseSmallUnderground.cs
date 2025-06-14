namespace Structify.Content.Items;

public class HouseSmallUnderground : SchematicItem
{
    protected override string SchematicName => "UndergroundHouse1";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 50)
    ];
    protected override int ItemRarity => ItemRarityID.LightRed;
}
