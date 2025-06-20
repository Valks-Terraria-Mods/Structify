using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class PrisonCell1 : SchematicItem
{
    protected override string ItemName => "Prison Cell";
    protected override string Description => "Can hold 1 NPC but commonly stacked adjacent to other prison cells to hold more.";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
}
