using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class WallOfFleshArena : SchematicItem
{
    protected override string ItemName => "Wall of Flesh Arena";
    protected override string Description => "Can be stacked horizontally.";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 5;
}
