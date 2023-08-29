namespace ValksStructures.Content.Items;

public class Hellavator : StructureItem
{
    protected override Ingredient[] Ingredients => new Ingredient[]
    {
        new(ItemID.StoneBlock, 100)
    };
    protected override int ItemRarity => ItemRarityID.Red;

    public override void UseTheItem(Player player, Vector2I mPos)
    {
        IsCurrentlyBuilding = true;

        VModSystem.AddAction(() =>
        {
            PlaceLeftWall(mPos);
            PlaceRightWall(mPos);
            KillEverythingBetweenWalls(mPos);
            PlaceChain(mPos);
            PlaceBackgroundWalls(mPos);
        });
        
        VModSystem.AddAction(() =>
        {
            PlaceTorches(mPos);
        });

        VModSystem.StartActions();
    }

    void PlaceLeftWall(Vector2I mPos)
    {
        for (int x = -4; x < -2; x++)
        {
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                Vector2I pos = mPos + new Vector2I(x, y);
                Utils.KillEverything(pos);
                PlaceTile(pos, TileID.StoneSlab);
            }
        }
    }

    void PlaceRightWall(Vector2I mPos)
    {
        for (int x = 4; x > 2; x--)
        {
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                Vector2I pos = mPos + new Vector2I(x, y);
                Utils.KillEverything(pos);
                PlaceTile(pos, TileID.StoneSlab);
            }
        }
    }

    void PlaceChain(Vector2I mPos)
    {
        for (int y = 0; y < Main.maxTilesY; y++)
        {
            Vector2I pos = mPos + new Vector2I(0, y);
            Utils.KillEverything(pos);
            PlaceTile(pos, TileID.Chain);
        }
    }
    
    void KillEverythingBetweenWalls(Vector2I mPos)
    {
        for (int x = -2; x <= 2; x++)
        {
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                Vector2I pos = mPos + new Vector2I(x, y);
                Utils.KillEverything(pos);
            }
        }
    }

    void PlaceBackgroundWalls(Vector2I mPos)
    {
        // Place background walls
        for (int x = -2; x <= 2; x++)
        {
            for (int y = 1; y < Main.maxTilesY; y++)
            {
                Vector2I pos = mPos + new Vector2I(x, y);
                PlaceWall(pos, WallID.StoneSlab);
            }
        }
    }

    void PlaceTorches(Vector2I mPos)
    {
        // Place torches
        foreach (int x in new int[] { -2, 2 })
        {
            for (int y = 30; y < Main.maxTilesY; y += 30)
            {
                Vector2I pos = mPos + new Vector2I(x, y);
                int redTorch = 2;
                PlaceTile(pos, TileID.Torches, redTorch);
            }
        }
    }

    static void PlaceTile(Vector2I pos, int tileId, int style = 0)
    {
        if (!Utils.IsInWorld(pos))
            return;

        WorldGen.PlaceTile(pos.X, pos.Y, tileId,
            mute: true,
            forced: true,
            plr: Main.myPlayer,
            style: style);

        if (Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendTileSquare(Main.myPlayer, pos.X, pos.Y);
    }

    static void PlaceWall(Vector2I pos, int wallId)
    {
        if (!Utils.IsInWorld(pos))
            return;

        WorldGen.PlaceWall(pos.X, pos.Y, wallId,
            mute: true);

        if (Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendTileSquare(Main.myPlayer, pos.X, pos.Y);
    }

    static void KillWall(Vector2I pos)
    {
        if (!Utils.IsInWorld(pos))
            return;

        WorldGen.KillWall(pos.X, pos.Y,
            fail: false);

        if (Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendTileSquare(Main.myPlayer, pos.X, pos.Y);
    }
}
