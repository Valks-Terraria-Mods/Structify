using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class PrisonCell3 : SchematicItem
{
    public override string ItemName => "Prison Cell (Sky)";
    public override string Description => "Holds 1 NPC, usually placed in the sky.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
}
