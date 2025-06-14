namespace Structify.Content.Items;

public class Test2 : GlobalTile
{
    public override void RightClick(int i, int j, int type)
    {
        //Tile tile = Main.tile[i, j];
        //Main.NewText($"Name: {tile.TileType}, TileFrameX: {tile.TileFrameX}, TileFrameY: {tile.TileFrameY}");
    }
}

public class Test : InteractItem
{
    private static int GetGrassYAtX(int x)
    {
        int lowestTrunkY = -1;

        for (int y = 0; y < Main.maxTilesY; y++)
        {
            Tile tile = Main.tile[x, y];

            if (tile.HasTile)
            {
                if (TileID.Sets.IsATreeTrunk[tile.TileType])
                {
                    lowestTrunkY = y;
                }
            }
        }

        return lowestTrunkY + 1;
    }

    public override bool UseTheItem(Player player, Vector2I mPos)
    {
        //WorldGen.Place3x2(mPos.X, mPos.Y, TileID.Tables);

        int y = GetGrassYAtX(mPos.X);

        if (y != -1)
            WorldGen.PlaceTile(mPos.X, y, TileID.Diamond, forced: true);
        //WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(mPos.X, y, TileID.Diamond);

        //WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(mPos.X, mPos.Y, TileID.Diamond);

        /*for (int i = 0; i < 3; i++)
        {
            Tile tile = Main.tile[mPos.X - 1 + i, mPos.Y];
            Main.NewText(tile.TileFrameX);
        }*/



        /*WorldGen.PlaceTile(mPos.X, mPos.Y, TileID.Tables, 
            forced: true,
            plr: Main.myPlayer);*/

        return true;
    }
}
