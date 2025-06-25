using StructureHelper.API;

namespace Structify.Commands;

public class CmdTest : ModCommand
{
    public override string Command => "test";
    public override CommandType Type => CommandType.Chat;

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        // Only allow the host to execute this command
        if (Main.netMode == NetmodeID.MultiplayerClient && !Main.countsAsHostForGameplay[caller.Player.whoAmI])
        {
            Main.NewText("This command can only be used by the host.", Color.Red);
            return;
        }

        string path = "Schematics/LargeHouse1.shstruct";

        Point16 mPos = new((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16);
        Point16 dim = Generator.GetStructureDimensions(path, Mod);

        Point16 origin = mPos - new Point16(0, (int)dim.Y);

        Generator.GenerateStructure(path, origin, Mod);

        Main.NewText("Test finished", Color.Green);
    }
}
