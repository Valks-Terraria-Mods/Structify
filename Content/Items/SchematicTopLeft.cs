namespace ValksStructures.Content.Items;

public class SchematicTopLeft : BasicItem
{
    public override int Tile => ModContent.TileType<Tiles.SchematicTopLeft>();
    public override int Rarity => ItemRarityID.Green;
}
