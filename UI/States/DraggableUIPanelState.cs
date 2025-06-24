using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace Structify.UI;

public class DraggableUIPanelState : UIState
{
    protected UIPanel Panel { get; private set; }
    protected new float Width { get; set; } = 800;
    protected new float Height { get; set; } = 400;
    
    private bool _dragging;
    private Vector2 _dragOffset;
    
    public override void OnInitialize()
    {
        AddPanel();
    }
    
    public override void Update(GameTime gameTime)
    {
        if (!_dragging) 
            return;
        
        Vector2 diff = Main.MouseScreen - _dragOffset;
        Panel.Left.Set(diff.X, 0f);
        Panel.Top.Set(diff.Y, 0f);
    }
    
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        
        // Prevent player from interacting with the world when clicking on the UI
        if (Panel.ContainsPoint(Main.MouseScreen)) {
            Main.LocalPlayer.mouseInterface = true;
        }
        
        // Prevent scrolling the hotbar when scrolling in the custom UI
        if (Panel.IsMouseHovering) {
            PlayerInput.LockVanillaMouseScroll("Structify/ScrollListB"); // The passed in string can be anything.
        }
    }

    private void AddPanel()
    {
        Panel = new UIPanel
        {
            BackgroundColor = new Color(0, 0, 0, 150),
            Width = { Pixels = Width },
            Height = { Pixels = Height },
            Left = { Percent = 0.5f, Pixels = -Width / 2 },
            Top = { Percent = 0.5f, Pixels = -Height / 2 }
        };
        
        Panel.OnLeftMouseDown += (evt, elm) =>
        {
            if (evt.Target == Panel)
            {
                StartDrag(evt); 
            }
        };
        
        Panel.OnLeftMouseUp += (evt, elm) =>
        {
            _dragging = false; 
        };
        
        Append(Panel);
    }
    
    private void StartDrag(UIMouseEvent evt)
    {
        _dragging = true;
        // Use actual panel screen position to calculate offset
        Vector2 panelPos = Panel.GetDimensions().Position();
        _dragOffset = Main.MouseScreen - panelPos;
    }
}