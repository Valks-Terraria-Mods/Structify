using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class SmallHouse : SchematicItem
{
    public override string SchematicName => "SmallHouse1";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 75)
    ];
    
    protected override string[] Authors { get; } = [Builders.ColinFour];
}
