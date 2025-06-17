using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class TowerGate1 : SchematicItem
{
    protected override string ItemName { get; } = "Tower Gate";
    
    public override int VerticalOffset { get; } = 2;
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];

    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.StoneBlock, 250),
        new(ItemID.Wood, 50),
        new(ItemID.IronBar, 20)
    ];
}
