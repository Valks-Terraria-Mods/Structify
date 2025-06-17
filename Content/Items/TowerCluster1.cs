using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class TowerCluster1 : SchematicItem
{
    protected override string ItemName { get; } = "Tower Cluster";
    
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 225)
    ];
    
    protected override string[] Authors { get; } = [Builders.Name36154, Builders.Valkyrienyanko];
}
