using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class FishingPond1 : SchematicItem
{
    public override string ItemName => "Fishing Pond";
    public override string Description => "Two small huts are on either side for 2 NPCs.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 7;
}