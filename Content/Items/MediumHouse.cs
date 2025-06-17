using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class MediumHouse : SchematicItem
{
    public override string SchematicName => "MediumHouse1";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 125)
    ];
    
    protected override string[] Authors { get; } = [Builders.Name36154];
}
