namespace ValksStructures.Content.Items;

public class Test2 : GlobalTile
{
    public override void RightClick(int i, int j, int type)
    {
        Tile tile = Main.tile[i, j];
        Main.NewText($"TileFrameX: {tile.TileFrameX}, TileFrameY: {tile.TileFrameY}");
    }
}

public class Test : InteractItem
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        //Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Test>());
    }

    public override void UseTheItem(Player player, Vector2I mPos)
    {
        WorldGen.Place3x2(mPos.X, mPos.Y, TileID.Tables);

        

        /*for (int i = 0; i < 3; i++)
        {
            Tile tile = Main.tile[mPos.X - 1 + i, mPos.Y];
            Main.NewText(tile.TileFrameX);
        }*/

        

        /*WorldGen.PlaceTile(mPos.X, mPos.Y, TileID.Tables, 
            forced: true,
            plr: Main.myPlayer);*/
    }
}
