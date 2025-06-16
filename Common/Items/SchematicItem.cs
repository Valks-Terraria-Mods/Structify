using Structify.Utils;
using StructureHelper.API;

namespace Structify.Common.Items;

public abstract class SchematicItem : StructureItem
{
    public abstract string SchematicName { get; }

    protected override bool UseTheItem(Player player, Point16 mPos)
    {
        StructureUtils.Generate(this, mPos);
        return true;
    }
    
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        Point16 dimensions = StructureUtils.GetDimensions(this);
        tooltips.Add(new TooltipLine(Mod, "Dimensions", $"Width: {dimensions.X}, Height: {dimensions.Y}"));
        
        if (VerticalOffset != 0)
            tooltips.Add(new TooltipLine(Mod, "Offset", $"Rooted {VerticalOffset} tiles beneath the surface"));
    }
}
