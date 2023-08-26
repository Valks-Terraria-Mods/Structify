namespace ValksStructures.Content.Items;

public class SchematicTopLeft : ModItem
{
    public override void SetDefaults()
    {
        Item.maxStack = 999;
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SchematicTopLeft>());
        Item.rare = ItemRarityID.Green;
    }
}
