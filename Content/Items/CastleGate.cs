using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class CastleGate : SchematicItem
{
    public override string SchematicName => "CastleGate1";
    public override int VerticalOffset { get; } = 6;
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko, Builders.Burlierbuffalo1];

    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.StoneBlock, 250),
        new(ItemID.Wood, 50),
        new(ItemID.IronBar, 20)
    ];
}
