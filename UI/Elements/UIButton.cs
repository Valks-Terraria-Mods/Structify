using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Structify.UI;

public class UIButton : UIPanel
{
    private readonly Color _baseColor;
    private readonly Color _hoverColor;
    private readonly Color _clickColor;

    private readonly UIText _labelText;

    public UIButton(string text, bool updateWidth = true) : this(text, new Color(30, 30, 30), updateWidth) { }

    public UIButton(string text, Color baseColor, bool updateWidth = true)
    {
        // Initialize colors
        _baseColor = baseColor;
        _baseColor.A = 200;
        _hoverColor = AddToColor(baseColor, 50);
        _clickColor = AddToColor(baseColor, 120);

        // Initial styling
        BackgroundColor = _baseColor;
        SetPadding(4);

        // Label
        _labelText = new UIText(text, 0.8f)
        {
            Left = { Pixels = 5 },
            VAlign = 0.5f
        };
        
        // Update width when text changes
        if (updateWidth)
            _labelText.OnInternalTextChange += UpdateWidth;

        Append(_labelText);

        // Initial size
        Height = new StyleDimension(30, 0f);
        
        if (updateWidth)
            UpdateWidth();
    }

    private void UpdateWidth()
    {
        float widthPixels = _labelText.MinWidth.Pixels + 20;
        Width = new StyleDimension(widthPixels, 0f);
    }

    public void SetText(string text)
    {
        _labelText.SetText(text);
    }

    public override void LeftMouseDown(UIMouseEvent evt)
    {
        base.LeftMouseDown(evt);
        BackgroundColor = _clickColor;
    }

    public override void LeftMouseUp(UIMouseEvent evt)
    {
        base.LeftMouseUp(evt);
        BackgroundColor = _hoverColor;
    }

    public override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);
        BackgroundColor = _hoverColor;
    }

    public override void MouseOut(UIMouseEvent evt)
    {
        base.MouseOut(evt);
        BackgroundColor = _baseColor;
    }

    private static Color AddToColor(Color color, byte value)
    {
        color.R = (byte)MathHelper.Clamp(color.R + value, 0, 255);
        color.G = (byte)MathHelper.Clamp(color.G + value, 0, 255);
        color.B = (byte)MathHelper.Clamp(color.B + value, 0, 255);
        return color;
    }
}
