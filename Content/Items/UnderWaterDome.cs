using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class UnderWaterDome : SchematicItem
{
    protected override string ItemName => "Underwater Dome";
    protected override string Description => "A structure you can place deep under water in the sea.";
    protected override string[] Authors { get; } = [Builders.Valkyrienyanko];
}