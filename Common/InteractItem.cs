namespace ValksStructures;

public abstract class InteractItem : ModItem
{
    private bool _canUseItem;

    public override void SetDefaults()
    {
        Item.stack = 999;
        Item.rare = ItemRarityID.White;

        // All of this is required to make a item become usable
        // with left-click
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.useAnimation = 20;
        Item.useTime = 20;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.shoot = ProjectileID.BoneArrow;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        return false;
    }

    public override bool ConsumeItem(Player player)
    {
        return _canUseItem;
    }

    public override bool? UseItem(Player player)
    {
        Vector2I mPos = new(
            (int)Main.MouseWorld.X / 16,
            (int)Main.MouseWorld.Y / 16);

        _canUseItem = UseTheItem(player, mPos);

        return true;
    }

    public abstract bool UseTheItem(Player player, Vector2I mPos);
}
