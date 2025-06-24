using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class Castle1 : SchematicItem
{
    public override string ItemName => "Castle";
    public override string Description => "A mostly empty castle for all your storage needs.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 15;
}
