using Structify.Common.Items;

namespace Structify.Content.Items;

public class CastleGate : SchematicItem
{
    public override string SchematicName => "CastleGate1";
    public override int VerticalOffset { get; } = 6;

    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.StoneBlock, 150)
    ];
}