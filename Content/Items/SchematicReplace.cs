namespace ValksStructures.Content.Items;

public class SchematicReplace : ModItem
{
    public override void SetDefaults()
    {
        Item.maxStack = 999;
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SchematicReplace>());
        Item.rare = ItemRarityID.Master;
    }
}
