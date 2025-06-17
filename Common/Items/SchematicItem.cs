using Structify.Utils;
using StructureHelper.API;

namespace Structify.Common.Items;

public abstract class SchematicItem : StructureItem
{
    public string SchematicName { get; private set; }

    public override void SetDefaults()
    {
        base.SetDefaults();
        SchematicName = GetType().Name;
    }

    protected override bool UseTheItem(Player player, Point16 mPos)
    {
        StructureUtils.Generate(this, mPos);
        return true;
    }

    protected override void AddMoreTooltips(List<TooltipLine> tooltips)
    {
        Point16 dimensions = StructureUtils.GetDimensions(this);
        tooltips.Add(new TooltipLine(Mod, "Dimensions", $"Size: {dimensions.X} x {dimensions.Y}"));
        
        if (VerticalOffset != 0)
            tooltips.Add(new TooltipLine(Mod, "Offset", $"Rooted {VerticalOffset} tiles beneath the surface"));
    }
}
