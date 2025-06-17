using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class SmallHouse1 : SchematicItem
{
    protected override string ItemName { get; } = "Small House";
    
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 75)
    ];
    
    protected override string[] Authors { get; } = [Builders.ColinFour, Builders.Valkyrienyanko];
}
