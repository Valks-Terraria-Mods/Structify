using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class TowerGate1 : SchematicItem
{
    protected override string ItemName => "Tower Gate";
    protected override string Description => "Defensive structure commonly used to divide biomes.";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 2;
}
