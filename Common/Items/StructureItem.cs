using Structify.Utils;

namespace Structify.Common.Items;

public abstract class StructureItem : ModItem
{
    protected abstract Ingredient[] Ingredients { get; }
    protected virtual int ItemRarity { get; } = ItemRarityID.LightPurple;
    public virtual int VerticalOffset { get; } = 0;

    private bool _canUseItem;

    public override void SetDefaults()
    {
        Item.maxStack = 100;
        Item.rare = ItemRarity;
        Item.consumable = true;

        // All of this is required to make a item become usable with left-click
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.useAnimation = 20;
        Item.useTime = 20;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.shoot = ProjectileID.BoneArrow;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe().AddTile(TileID.HeavyWorkBench);

        foreach (Ingredient ingredient in Ingredients)
            recipe.AddIngredient(ingredient.ItemId, ingredient.Amount);

        recipe.Register();
    }

    public override bool? UseItem(Player player)
    {
        // UseItem is called for every client so we need to check if this is the local player
        if (Main.myPlayer == player.whoAmI)
        {
            Point16 mPos = Main.MouseWorld.ToTileCoordinates16();
            _canUseItem = UseTheItem(player, mPos);
        }

        return base.UseItem(player);
    }

    protected abstract bool UseTheItem(Player player, Point16 mPos);

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        return false;
    }

    public override bool ConsumeItem(Player player)
    {
        return _canUseItem;
    }
}
