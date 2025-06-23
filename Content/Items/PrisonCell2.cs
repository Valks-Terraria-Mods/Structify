using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class PrisonCell2 : SchematicItem
{
    protected override string ItemName => "Prison Cell (Wide)";
    protected override string Description => "Holds 1 NPC, entry is from top. Usually placed near surface underground.";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
}
