using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class Ship1 : SchematicItem
{
    protected override string ItemName => "Boat";
    protected override string Description => "A boat to place on the water for fishing perhaps?";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 5;
}
