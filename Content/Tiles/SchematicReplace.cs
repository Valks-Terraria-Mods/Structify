namespace Structify.Content.Tiles;

// This ModTile is a platform so it does not change the slope of surrounding tiles when placed
// ExamplePlatform.cs: https://github.com/tModLoader/tModLoader/blob/73dc09057d52503d77499ea907beb4307618a326/ExampleMod/Content/Tiles/Furniture/ExamplePlatform.cs
public class SchematicReplace : ModTile
{
    public override void SetStaticDefaults()
    {
        // Properties
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileSolidTop[Type] = true;
        Main.tileSolid[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileTable[Type] = true;
        Main.tileLavaDeath[Type] = true;
        TileID.Sets.Platforms[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
        AddMapEntry(new Color(200, 200, 200));

        DustType = DustID.ShimmerSpark;
        AdjTiles = [TileID.Platforms];

        // Placement
        TileObjectData.newTile.CoordinateHeights = [16];
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.StyleMultiplier = 27;
        TileObjectData.newTile.StyleWrapLimit = 27;
        TileObjectData.newTile.UsesCustomCanPlace = false;
        TileObjectData.newTile.LavaDeath = true;
        TileObjectData.addTile(Type);
    }

    public override void PostSetDefaults() => Main.tileNoSunLight[Type] = false;

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
}
