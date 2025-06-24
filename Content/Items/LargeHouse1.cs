using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class LargeHouse1 : SchematicItem
{
    public override string ItemName => "Large House";
    public override string Description => "Can hold 3 NPCs and comes with a basement.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 6;
}
