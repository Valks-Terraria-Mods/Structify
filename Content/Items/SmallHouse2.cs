using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class SmallHouse2 : SchematicItem
{
    protected override string ItemName => "Small House Type 2";
    protected override string Description => "Can hold 1 NPC.";
    protected override string[] Authors { get; } = [Builders.Name36154, Builders.Valkyrienyanko];
}
