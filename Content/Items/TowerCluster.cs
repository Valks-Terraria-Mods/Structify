using Structify.Common.Items;

namespace Structify.Content.Items;

public class TowerCluster : SchematicItem
{
    public override string SchematicName => "TowerCluster1";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 200)
    ];
    protected override int ItemRarity => ItemRarityID.LightRed;
}
