namespace Structify;

public abstract class BasicItem : ModItem
{
    public abstract int Tile { get; }
    public virtual int Rarity { get; } = ItemRarityID.White;

    public override void SetDefaults()
    {
        Item.maxStack = 999;
        Item.DefaultToPlaceableTile(Tile);
        Item.rare = Rarity;
    }
}
