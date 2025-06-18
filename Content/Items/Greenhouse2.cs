using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class Greenhouse2 : SchematicItem
{
    protected override string ItemName => "Greenhouse Type 2";
    protected override string Description => "The interior is empty so you can place what you want.";
    public override int VerticalOffset => 16;
    protected override string[] Authors { get; } = [Builders.Grim];
}