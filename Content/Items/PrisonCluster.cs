namespace ValksStructures.Content.Items;

public class PrisonCluster : SchematicItem
{
    protected override string SchematicName => "PrisonCluster2";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 200)
    ];
    protected override int ItemRarity => ItemRarityID.LightRed;
}
