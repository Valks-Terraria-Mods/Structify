using System.IO;

namespace Structify.Commands;

public class CmdTest : ModCommand
{
    public override string Command => "test";
    public override CommandType Type => CommandType.World;

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (args.Length == 0)
        {
            Main.NewText("Specify at least one args");
            return;
        }
        
        ValksStructures mod = ModContent.GetInstance<ValksStructures>();

        string resource = mod
            .GetFileNames()
            .FirstOrDefault(n => n
                .StartsWith("Schematics/", StringComparison.OrdinalIgnoreCase) && Path.GetFileNameWithoutExtension(n)
                .Equals(args[0], StringComparison.OrdinalIgnoreCase)
            );
        
        Main.NewText(resource);
    }
}
