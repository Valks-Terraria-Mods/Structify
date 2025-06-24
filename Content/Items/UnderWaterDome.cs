using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class UnderWaterDome : SchematicItem
{
    public override string ItemName => "Underwater Dome";
    public override string Description => "A structure you can place deep under water in the sea.";
    public override string[] Authors { get; } = [Builders.Valkyrienyanko];
}