using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class TowerGate1 : SchematicItem
{
    public override string ItemName => "Tower Gate";
    public override string Description => "Defensive structure commonly used to divide biomes.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 2;
}
