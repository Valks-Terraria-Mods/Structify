using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class WallOfFleshArena : SchematicItem
{
    public override string ItemName => "Wall of Flesh Arena";
    public override string Description => "Can be stacked horizontally.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 5;
}
