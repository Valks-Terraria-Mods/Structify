using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class PrisonCell1 : SchematicItem
{
    protected override string ItemName => "Prison Cell (Underground)";
    protected override string Description => "Holds 1 NPC, usually placed deep underground.";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
}
