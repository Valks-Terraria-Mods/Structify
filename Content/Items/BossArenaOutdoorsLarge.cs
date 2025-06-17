using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class BossArenaOutdoorsLarge : SchematicItem
{
    protected override string ItemName => "Large Outdoors Boss Arena";
    protected override bool CanReplaceTiles => false;

    protected override Ingredient[] Ingredients =>
    [
        new(ItemID.StoneBlock, 300),
        new(RecipeGroupID.IronBar, 15)
    ];

    protected override string[] Authors { get; } = [Builders.Grim];
}