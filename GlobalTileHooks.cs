namespace ValksStructures;

public class GlobalTileHooks : GlobalTile
{
    public override bool CanDrop(int i, int j, int type)
    {
        // Do not drop any furniture when building a schematic
        // This prevents doors from dropping if the schematic is
        // building over a door
        if (Schematic.IsCurrentlyBuilding)
        {
            if (Utils.IsFurnitureTile(type))
            {
                return false;
            }
        }

        return true;
    }

    public override void PlaceInWorld(int i, int j, int type, Item item)
    {
        if (type == ModContent.TileType<Content.Tiles.SchematicTopLeft>())
        {
            ModContent.GetInstance<CmdSave>().TopLeft = 
                new Vector2I(i, j);

            Main.NewText("Set top left position");
        }

        if (type == ModContent.TileType<Content.Tiles.SchematicBottomRight>())
        {
            ModContent.GetInstance<CmdSave>().BottomRight = 
                new Vector2I(i, j);

            Main.NewText("Set bottom right position");
        }
    }
}
