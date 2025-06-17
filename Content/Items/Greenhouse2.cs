using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class Greenhouse2 : SchematicItem
{
    protected override string ItemName { get; } = "Greenhouse Type 2";
    
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 175)
    ];

    public override int VerticalOffset { get; } = 16;

    protected override string[] Authors { get; } = [Builders.Grim];
}