using Structify.Common.Items;

namespace Structify.Content.Items;

public class FishingPond : SchematicItem
{
    public override string SchematicName => "FishingPond1";
    public override int VerticalOffset { get; } = 7;
    
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.StoneBlock, 150)
    ];
}