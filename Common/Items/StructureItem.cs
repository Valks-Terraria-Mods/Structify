namespace Structify.Content.Items;

public abstract class StructureItem : InteractItem
{
    protected abstract Ingredient[] Ingredients { get; }
    protected virtual int ItemRarity { get; } = ItemRarityID.LightPurple;

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.maxStack = 999;
        Item.rare = ItemRarity;
        Item.consumable = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe().AddTile(TileID.HeavyWorkBench);

        foreach (Ingredient ingredient in Ingredients)
            recipe.AddIngredient(ingredient.ItemId, ingredient.Amount);

        recipe.Register();
    }
}
