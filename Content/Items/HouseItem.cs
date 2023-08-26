namespace ValksStructures.Content.Items;

public abstract class HouseItem : ModItem
{
    protected abstract string SchematicName { get; }
    protected abstract Ingredient[] Ingredients { get; }
    protected virtual int ItemRarity { get; } = ItemRarityID.LightPurple;
    protected virtual int VerticalOffset { get; } = 0;

    public override void SetDefaults()
    {
        Item.maxStack = 999;
        Item.rare = ItemRarity;
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.useAnimation = 20;
        Item.useTime = 20;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.shoot = ProjectileID.BoneArrow;
        Item.consumable = true;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Schematic schematic = Schematic.Load(SchematicName);

        if (schematic == null)
        {
            Main.NewText($"Could not find the '{SchematicName}' schematic");
            return false;
        }

        Schematic.Paste(schematic,
            styleOffset: ModContent.GetInstance<Config>().BuildStyle,
            vOffset: VerticalOffset);

        return false;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe().AddTile(TileID.HeavyWorkBench);

        foreach (Ingredient ingredient in Ingredients)
            recipe.AddIngredient(ingredient.ItemId, ingredient.Amount);

        recipe.Register();
    }
}
