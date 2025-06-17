using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class SmallHouse2 : SchematicItem
{
    public override string SchematicName => "SmallHouse2";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 125)
    ];
    
    protected override string[] Authors { get; } = [Builders.Name36154, Builders.Valkyrienyanko];
}
