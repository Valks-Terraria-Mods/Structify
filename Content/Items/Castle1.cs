using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class Castle1 : SchematicItem
{
    protected override string ItemName => "Castle";
    protected override string Description => "A mostly empty castle for all your storage needs.";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 15;
}
