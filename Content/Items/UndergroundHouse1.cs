using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class UndergroundHouse1 : SchematicItem
{
    protected override string ItemName => "Underground House";
    protected override string Description => "The interior is empty allowing you to decorate it yourself.";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
    public override int VerticalOffset => 1;
}
