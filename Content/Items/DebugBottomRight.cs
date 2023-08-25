namespace ValksStructures.Content.Items;

public class DebugBottomRight : ModItem
{
    public override void SetDefaults()
    {
        Item.maxStack = 999;
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DebugBottomRight>());
        Item.rare = ItemRarityID.Green;
    }
}
