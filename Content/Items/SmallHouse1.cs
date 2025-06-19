using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class SmallHouse1 : SchematicItem
{
    protected override string ItemName => "Small House Type 1";
    protected override string Description => "Can hold 1 NPC.";
    protected override string[] Authors { get; } = [Builders.ColinFour, Builders.Valkyrienyanko];

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.shopCustomPrice = Item.buyPrice(silver: 20);
    }
}
