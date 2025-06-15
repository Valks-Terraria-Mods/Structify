using Structify.Common.Items;

namespace Structify.Content.Items;

public class LargeHouse : SchematicItem
{
    protected override string SchematicName => "LargeHouse1";
    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.Wood, 175),
        new(ItemID.StoneBlock, 25)
    ];
    protected override int ItemRarity => ItemRarityID.Quest;
}
