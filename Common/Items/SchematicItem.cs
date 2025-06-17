using Structify.Utils;
using StructureHelper.API;

namespace Structify.Common.Items;

public abstract class SchematicItem : StructureItem
{
    public string SchematicName { get; private set; }
    protected virtual bool CanReplaceTiles => true;

    public override void SetDefaults()
    {
        base.SetDefaults();
        SchematicName = GetType().Name;
    }

    protected override bool UseTheItem(Player player, Point16 mPos)
    {
        if (!CanReplaceTiles)
        {
            Point16 dimensions = StructureUtils.GetDimensions(this);
            Point16 bottomLeftAnchor = StructureUtils.GetOrigin(this, dimensions, mPos);

            for (int x = bottomLeftAnchor.X; x < bottomLeftAnchor.X + dimensions.X; x++)
            {
                for (int y = bottomLeftAnchor.Y; y < bottomLeftAnchor.Y + dimensions.Y; y++)
                {
                    if (!Main.tile[x, y].HasTile && Main.tile[x, y].WallType == WallID.None) 
                        continue;
                    
                    Main.NewText("There is a tile or wall blocking the structure from being placed.", Color.Red);
                    return false;
                }
            }
        }
        
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
