using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class FishingPond1 : SchematicItem
{
    protected override string ItemName { get; } = "Fishing Pond";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko, Builders.Burlierbuffalo1];
    public override int VerticalOffset { get; } = 7;
    
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 100),
        new(ItemID.StoneBlock, 25)
    ];
}