using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class LargeHouse : SchematicItem
{
    public override string SchematicName => "LargeHouse1";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 250),
        new(ItemID.StoneBlock, 100)
    ];
    
    protected override string[] Authors { get; } = [Builders.Name36154, Builders.Valkyrienyanko];

    public override int VerticalOffset { get; } = 12;
}
