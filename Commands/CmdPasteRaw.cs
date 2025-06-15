namespace Structify.Commands;

public class CmdPasteRaw : ModCommand
{
    public override string Command => "pasteraw";
    public override CommandType Type => CommandType.Chat;

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (args.Length == 0)
        {
            Schematic savedSchematic = ModContent.GetInstance<CmdSave>().Schematic;

            if (savedSchematic == null)
            {
                Main.NewText("Usage: /pasteraw <name>");
            }
            else
            {
                Paste(savedSchematic);
            }

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

        Paste(schematic);
    }

    private static void Paste(Schematic schematic)
    {
        Vector2I mPos = new(
            (int)Main.MouseWorld.X / 16,
            (int)Main.MouseWorld.Y / 16);

        bool success = Schematic.Paste(schematic, mPos, showBlueTiles: true);

        if (!success)
        {
            Main.NewText($"Could not paste structure");
            return;
        }

        Main.NewText($"Pasted structure");
    }
}