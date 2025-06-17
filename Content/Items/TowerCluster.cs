using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class TowerCluster : SchematicItem
{
    public override string SchematicName => "TowerCluster1";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 225)
    ];
    
    protected override string[] Authors { get; } = [Builders.Name36154];
}
