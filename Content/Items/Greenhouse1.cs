using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class Greenhouse1 : SchematicItem
{
    protected override string ItemName { get; } = "Greenhouse Type 1";
    
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 150)
    ];
    
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
}