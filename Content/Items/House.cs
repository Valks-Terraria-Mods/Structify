using System.Text.Json;
using System.IO;

namespace ValksStructures.Content.Items;

public class House : ModItem
{
    public override void SetDefaults()
    {
        Item.maxStack = 999;
        Item.rare = ItemRarityID.LightPurple;
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
        Schematic schematic = Schematic.Load("House1");

        if (schematic == null)
        {
            Main.NewText("Schematic is null");
            return false;
        }

        Schematic.Paste(schematic, 
            style: ModContent.GetInstance<Config>().BuildStyle);

        return false;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Wood, 50)
            .AddTile(TileID.HeavyWorkBench)
            .Register();
    }
}

public class FurnitureTile
{
    public Vector2I Position { get; set; }
    public int TileFrameX { get; set; }
    public int TileFrameY { get; set; }
    public int Slope { get; set; }
    public int Id { get; set; }
}
