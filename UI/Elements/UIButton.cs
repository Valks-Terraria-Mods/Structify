using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Structify.UI;

public class UIButton : UIPanel
{
    private readonly Dictionary<ClickType, Color> _colors = new()
    {
        { ClickType.Normal, new Color(30, 30, 30, 200)    },
        { ClickType.Hover,  new Color(80, 80, 80, 200)    },
        { ClickType.Click,  new Color(150, 150, 150, 200) }
    };
    
    public UIButton()
    {
        BackgroundColor = _colors[ClickType.Normal];
        Width = new StyleDimension(0, 1f);
        Height = new StyleDimension(30, 0f);
        SetPadding(4);
    }

    public override void LeftMouseDown(UIMouseEvent evt)
    {
        base.LeftMouseDown(evt);
        BackgroundColor = _colors[ClickType.Click];
    }

    public override void LeftMouseUp(UIMouseEvent evt)
    {
        base.LeftMouseUp(evt);
        BackgroundColor = _colors[ClickType.Hover];
    }

    public override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);
        BackgroundColor = _colors[ClickType.Hover];
    }

    public override void MouseOut(UIMouseEvent evt)
    {
        base.MouseOut(evt);
        BackgroundColor = _colors[ClickType.Normal];
    }
    
    private enum ClickType
    {
        Normal,
        Hover,
        Click
    }
}
