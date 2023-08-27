namespace ValksStructures.Content.Items;

public class SmallHouse : SchematicItem
{
    protected override string SchematicName => "SmallHouse1";
    protected override Ingredient[] Ingredients => new Ingredient[]
    {
        new(ItemID.Wood, 50)
    };
    protected override int ItemRarity => ItemRarityID.LightRed;
}
