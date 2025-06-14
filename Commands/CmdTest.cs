using System.IO;

namespace Structify.Commands;

public class CmdTest : ModCommand
{
    public override string Command => "test";
    public override CommandType Type => CommandType.Chat;

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        Main.NewText(GameQueue.Actions.Count.ToString());
    }
}
