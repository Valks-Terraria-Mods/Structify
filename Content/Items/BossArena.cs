using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class BossArena : SchematicItem
{
    public override string ItemName => "Boss Arena";
    public override string Description => "Section of a boss arena meant to be stacked adjacently with itself.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset { get; } = 4;
}
