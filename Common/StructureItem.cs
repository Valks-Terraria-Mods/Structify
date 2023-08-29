namespace ValksStructures.Content.Items;

public abstract class StructureItem : ModItem
{
    public static bool IsCurrentlyBuilding { get; set; }

    protected abstract Ingredient[] Ingredients { get; }
    protected virtual int ItemRarity { get; } = ItemRarityID.LightPurple;

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

    public override bool? UseItem(Player player)
    {
        PlaceStructure(player);
        return true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe().AddTile(TileID.HeavyWorkBench);

        foreach (Ingredient ingredient in Ingredients)
            recipe.AddIngredient(ingredient.ItemId, ingredient.Amount);

        recipe.Register();
    }

    public abstract void PlaceStructure(Player player);
}
