using Structify.Common.Items;
using Structify.Utils;

namespace Structify.Content.Items;

public class JungleFarm : SchematicItem
{
    public override string ItemName => "Jungle Farm";
    public override string Description => "Several rows of mud and grass usually placed deep inside the jungle.";
    public override string[] Authors { get; } = [Builders.Toast];
    public override int VerticalOffset { get; } = 4;
}
