using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class PrisonCell1 : SchematicItem
{
    public override string ItemName => "Prison Cell (Underground)";
    public override string Description => "Holds 1 NPC, usually placed deep underground.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
}
