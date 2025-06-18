using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class Greenhouse1 : SchematicItem
{
    protected override string ItemName => "Greenhouse Type 1";
    protected override string Description => "Simple structure with rows of flower pots. Can hold 1 NPC.";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
}