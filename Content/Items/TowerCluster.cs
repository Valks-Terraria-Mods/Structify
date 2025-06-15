namespace Structify.Content.Items;

public class TowerCluster : SchematicItem
{
    protected override string SchematicName => "TowerCluster1";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 200)
    ];
    protected override int ItemRarity => ItemRarityID.LightRed;
}
