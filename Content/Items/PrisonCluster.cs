namespace ValksStructures.Content.Items;

public class PrisonCluster : SchematicItem
{
    protected override string SchematicName => "PrisonCluster2";
    protected override Ingredient[] Ingredients => new Ingredient[]
    {
        new(ItemID.Wood, 200)
    };
    protected override int ItemRarity => ItemRarityID.LightRed;
}
