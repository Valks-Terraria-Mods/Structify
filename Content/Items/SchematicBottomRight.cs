namespace ValksStructures.Content.Items;

public class SchematicBottomRight : ModItem
{
    public override void SetDefaults()
    {
        Item.maxStack = 999;
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SchematicBottomRight>());
        Item.rare = ItemRarityID.Green;
    }
}
