namespace ValksStructures;

public class CmdSave : ModCommand
{
    public override string Command => "save";
    public override CommandType Type => CommandType.World;

    public Schematic Schematic { get; set; }
    public Vector2I TopLeft { get; set; }
    public Vector2I BottomRight { get; set; }

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        // Assuming top left will never be (0, 0)
        if (TopLeft == Vector2I.Zero)
        {
            Main.NewText("Top left position not set");
            return;
        }

        // Assuming bottom right will never be (0, 0)
        if (BottomRight == Vector2I.Zero)
        {
            Main.NewText("Bottom right position not set");
            return;
        }

        if (args.Length == 0)
        {
            Main.NewText("Usage: /save <name>");
            return;
        }

        if (!args[0].All(char.IsLetterOrDigit))
        {
            Main.NewText("File name may only contain letters and numbers");
            return;
        }

        int diffX = BottomRight.X - TopLeft.X;
        int diffY = BottomRight.Y - TopLeft.Y;

        Schematic schematic = Schematic.Create(TopLeft, diffX, diffY);

        // Temporary code
        Schematic = schematic;

        string savePath = $"{Main.SavePath}/{nameof(ValksStructures)}";

        Schematic.Save(schematic, savePath, 
            fileName: args[0]);

        Main.NewText("Saved schematic");

        Utils.OpenFolder(savePath);
    }
}
