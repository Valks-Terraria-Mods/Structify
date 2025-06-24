using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class Ship1 : SchematicItem
{
    public override string ItemName => "Boat";
    public override string Description => "A boat to place on the water for fishing perhaps?";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 5;
}
