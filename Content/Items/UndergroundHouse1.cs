using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class UndergroundHouse1 : SchematicItem
{
    public override string ItemName => "Underground House";
    public override string Description => "The interior is empty allowing you to decorate it yourself.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 1;
}
