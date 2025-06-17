using Structify.Utils;
using StructureHelper;

namespace Structify.Common.Items;

public abstract class StructureItem : ModItem
{
    protected abstract Ingredient[] Ingredients { get; }
    protected abstract string[] Authors { get; }
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

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        AddMoreTooltips(tooltips);
        
        // Authors always appended last
        string authors = FormatAuthors(Authors);
        
        tooltips.Add(new TooltipLine(Mod, "Authors", $"Built by {authors}"));
    }

    protected virtual void AddMoreTooltips(List<TooltipLine> tooltips) {}

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
    
    /// <summary>
    /// Formats the author string into a nice readable format.
    /// </summary>
    private static string FormatAuthors(IEnumerable<string> authors)
    {
        List<string> authorList = authors.ToList();

        switch (authorList.Count)
        {
            case 0:
                return string.Empty;
            case 1:
                // "A"
                return authorList[0];
            case 2:
                // "A and B"
                return $"{authorList[0]} and {authorList[1]}";
        }

        // For 3 or more authors: "A, B and C"
        IEnumerable<string> allButLast = authorList.Take(authorList.Count - 1);
        string lastAuthor = authorList.Last();

        return $"{string.Join(", ", allButLast)} and {lastAuthor}";
    }
}
