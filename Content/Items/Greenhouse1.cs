using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class Greenhouse1 : SchematicItem
{
    public override string ItemName => "Small Greenhouse";
    public override string Description => "Simple structure with rows of flower pots. Can hold 1 NPC.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
}