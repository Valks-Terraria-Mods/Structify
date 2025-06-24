using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class Greenhouse2 : SchematicItem
{
    public override string ItemName => "Overgrown House";
    public override string Description => "The interior is empty so you can place what you want.";
    public override int VerticalOffset => 16;
    public override string[] Authors { get; } = [Builders.Grim];
}