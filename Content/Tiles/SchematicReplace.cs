namespace ValksStructures.Content.Tiles;

public class SchematicReplace : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
    }
}
