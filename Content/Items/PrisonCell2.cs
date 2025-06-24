using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class PrisonCell2 : SchematicItem
{
    public override string ItemName => "Prison Cell (Wide)";
    public override string Description => "Holds 1 NPC, entry is from top. Usually placed near surface underground.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
}
