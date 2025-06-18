using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class BossArenaOutdoorsLarge : SchematicItem
{
    protected override string ItemName => "Large Outdoors Boss Arena";
    protected override string Description => "Equipped with campfires and heart lanterns.";
    protected override bool CanReplaceTiles => false;
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
}
