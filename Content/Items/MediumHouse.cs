using Structify.Common.Items;

namespace Structify.Content.Items;

public class MediumHouse : SchematicItem
{
    public override string SchematicName => "MediumHouse1";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 100)
    ];
}
