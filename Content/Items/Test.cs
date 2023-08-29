namespace ValksStructures.Content.Items;

public class Test : BasicItem
{
    public override int Tile => ModContent.TileType<Tiles.Test>();
    public override int Rarity => ItemRarityID.White;

    public override bool? UseItem(Player player)
    {
        
        return true;
    }
}
