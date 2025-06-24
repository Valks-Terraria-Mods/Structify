using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class SmallHouse1 : SchematicItem
{
    public override string ItemName => "Small House";
    public override string Description => "Can hold 1 NPC and comes with a basement.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 5;

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.shopCustomPrice = Item.buyPrice(silver: 20);
    }
}
