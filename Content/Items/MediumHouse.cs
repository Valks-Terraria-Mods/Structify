namespace ValksStructures.Content.Items;

public class MediumHouse : HouseItem
{
    protected override string SchematicName => "MediumHouse1";
    protected override Ingredient[] Ingredients => new Ingredient[]
    {
        new(ItemID.Wood, 100)
    };
    protected override int ItemRarity => ItemRarityID.Pink;
}
