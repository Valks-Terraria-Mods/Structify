using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class LargeHouse1 : SchematicItem
{
    protected override string ItemName => "Large House";
    protected override string Description => "Can hold 5 NPCs and comes with a basement.";
    protected override string[] Authors { get; } = [Builders.Name36154, Builders.Valkyrienyanko];
    public override int VerticalOffset => 12;
}
