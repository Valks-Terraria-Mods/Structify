namespace ValksStructures.Content.Items;

public class SchematicReplace : BasicItem
{
    public override int Tile => ModContent.TileType<Tiles.SchematicReplace>();
    public override int Rarity => ItemRarityID.Blue;
}
