using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class MediumHouse1 : SchematicItem
{
    protected override string ItemName => "Medium House";
    protected override string Description => "Can hold 2 NPCs and comes with a basement.";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 6;
}
