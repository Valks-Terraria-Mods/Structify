using Structify.Content.Items;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.Utilities;

namespace Structify.Content.NPCs;

[AutoloadHead]
public class StructureMerchant : ModNPC
{
	public const string ShopName = "Shop";
	
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

		NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
		NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
		NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
		NPCID.Sets.AttackType[Type] = 0; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
		NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
		NPCID.Sets.AttackAverageChance[Type] = 30; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
		NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.
		NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

		NPCID.Sets.ShimmerTownTransform[Type] = true; // Allows for this NPC to have a different texture after touching the Shimmer liquid.
    }
    
    public override void SetDefaults() 
    {
	    NPC.townNPC = true; // Sets NPC to be a Town NPC
	    NPC.friendly = true; // NPC Will not attack player
	    NPC.width = 18;
	    NPC.height = 40;
	    NPC.aiStyle = 7;
	    NPC.damage = 10;
	    NPC.defense = 15;
	    NPC.lifeMax = 250;
	    NPC.HitSound = SoundID.NPCHit1;
	    NPC.DeathSound = SoundID.NPCDeath1;
	    NPC.knockBackResist = 0.5f;

	    AnimationType = NPCID.Guide;
    }
    
    public override List<string> SetNPCNameList() {
	    return
	    [
		    "Bob"
	    ];
    }

public override string GetChat()
{
    WeightedRandom<string> chat = new();
    chat.Add("Building stuff is my superpower. What's yours?");
    chat.Add("My favorite building material? Lego's. Obviously.");
    chat.Add("Huh? Oh, sorry, I was daydreaming about a castle made of marshmallows.");
    chat.Add("Got a question about construction? Because I can always nail it.");
    
    return chat;
}

    public override bool CanTownNPCSpawn(int numTownNPCs)
    {
        return true;
    }

    public override void SetChatButtons(ref string button, ref string button2)
    {
	    button = Language.GetTextValue("LegacyInterface.28");
    }

    public override void OnChatButtonClicked(bool firstButton, ref string shopName)
    {
	    shopName = ShopName;
    }

    private NPCShop _shop;

	public override void AddShops()
	{
		_shop = new NPCShop(Type, ShopName);
		AddItemsToShop();
		_shop.Register();
	}

	private void AddItemsToShop()
	{
		AddItem<SmallHouse1>(Item.buyPrice(silver: 20));
		AddItem<SmallHouse2>(Item.buyPrice(silver: 25));
		AddItem<Greenhouse1>(Item.buyPrice(silver: 30));
		AddItem<Greenhouse2>(Item.buyPrice(silver: 35));
		AddItem<FishingPond1>(Item.buyPrice(silver: 35));
		AddItem<TowerCluster1>(Item.buyPrice(silver: 45));
		AddItem<TowerGate1>(Item.buyPrice(silver: 45));
		AddItem<LargeHouse1>(Item.buyPrice(gold: 1));
		AddItem<Hellavator>(Item.buyPrice(gold: 1));
		AddItem<BossArenaOutdoorsLarge>(Item.buyPrice(gold: 2));
	}

	private void AddItem<T>(int cost) where T : ModItem
	{
		AddShopItem<T>(_shop, cost);
	}

	private static void AddShopItem<T>(NPCShop shop, int cost) where T : ModItem
	{
		shop.Add(new Item(ModContent.ItemType<T>(), stack: 1)
		{
			shopCustomPrice = cost
		});
	}
    
    public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
	    damage = 20;
	    knockback = 4f;
    }

    public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
	    cooldown = 30;
	    randExtraCooldown = 30;
    }

    public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
    {
	    projType = ProjectileID.Shuriken;
	    attackDelay = 1;
    }

    public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
	    multiplier = 12f;
	    randomOffset = 2f;
    }
}
