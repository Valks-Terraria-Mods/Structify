namespace ValksStructures.Commands;

public class CmdPaste : ModCommand
{
    public override string Command => "paste";
    public override CommandType Type => CommandType.World;

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (args.Length == 0)
        {
            Main.NewText("Usage: /paste <name>");
            return;
        }

        // Args are automatically put to lowercase but this is just to make extra sure
        string structureName = args[0].ToLower();
        
        Schematic schematic = Schematic.Load(structureName);

        if (schematic == null)
        {
            Main.NewText($"Could not find the '{structureName}' schematic");
            return;
        }
        
        Vector2I mPos = new(
            (int)Main.MouseWorld.X / 16,
            (int)Main.MouseWorld.Y / 16);

        bool success = Schematic.Paste(schematic, mPos);

        if (!success)
        {
            Main.NewText($"Could not paste the '{structureName}' schematic");
            return;
        }

        Main.NewText($"Pasted structure {args[0]}");
    }
}
