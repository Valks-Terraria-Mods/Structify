using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class TowerCluster1 : SchematicItem
{
    protected override string ItemName => "Tower Cluster";
    protected override string Description => "Can hold 10 NPCs.";
    protected override string[] Authors { get; } = [Builders.Name36154, Builders.Valkyrienyanko];
}
