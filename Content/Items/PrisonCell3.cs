using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class PrisonCell3 : SchematicItem
{
    protected override string ItemName => "Prison Cell (Sky)";
    protected override string Description => "Holds 1 NPC, usually placed in the sky.";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
}
