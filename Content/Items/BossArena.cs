using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class BossArena : SchematicItem
{
    protected override string ItemName => "Boss Arena";
    protected override string Description => "Section of a boss arena meant to be stacked adjacently with itself.";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset { get; } = 4;
}
