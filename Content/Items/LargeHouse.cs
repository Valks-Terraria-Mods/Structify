namespace ValksStructures.Content.Items;

public class LargeHouse : HouseItem
{
    protected override string SchematicName => "LargeHouse1";
    protected override Ingredient[] Ingredients => new Ingredient[]
    {
        new(ItemID.Wood, 175),
        new(ItemID.StoneBlock, 25)
    };
    protected override int ItemRarity => ItemRarityID.Quest;
}
