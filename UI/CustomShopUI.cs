using Microsoft.Xna.Framework.Graphics;
using Structify.Common.Players;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace Structify.UI;

public class CustomShopUI : UIState
{
    private const float MainPanelWidth = 800;
    private const float MainPanelHeight = 400;
    
    private UIPanel _mainPanel;
    private UIList _itemList;
    private UIScrollbar _scrollBar;
    
    private bool _dragging;
    private Vector2 _dragOffset;

    public override void OnInitialize()
    {
        AddMainPanel();
        AddScrollBar();
        AddItemList();
        
        foreach (Structure structure in StructureCatalog.All)
        {
            AddItem(structure);
        }
    }
    
    public override void Update(GameTime gameTime)
    {
        if (!_dragging) 
            return;
        
        Vector2 diff = Main.MouseScreen - _dragOffset;
        _mainPanel.Left.Set(diff.X, 0f);
        _mainPanel.Top.Set(diff.Y, 0f);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        base.DrawSelf(spriteBatch);
        
        // Prevent player from interacting with the world when clicking on the UI
        if (_mainPanel.ContainsPoint(Main.MouseScreen)) {
            Main.LocalPlayer.mouseInterface = true;
        }
        
        // Prevent scrolling the hotbar when scrolling in the custom UI
        if (_mainPanel.IsMouseHovering) {
            PlayerInput.LockVanillaMouseScroll("Structify/ScrollListB"); // The passed in string can be anything.
        }
    }
    
    private void AddMainPanel()
    {
        _mainPanel = new UIPanel
        {
            BackgroundColor = new Color(0, 0, 0, 150),
            Width = { Pixels = MainPanelWidth },
            Height = { Pixels = MainPanelHeight },
            Left = { Percent = 0.5f, Pixels = -MainPanelWidth / 2 },
            Top = { Percent = 0.5f, Pixels = -MainPanelHeight / 2 }
        };
        
        _mainPanel.OnLeftMouseDown += (evt, elm) =>
        {
            if (evt.Target == _mainPanel)
            {
                StartDrag(evt); 
            }
        };
        
        _mainPanel.OnLeftMouseUp += (evt, elm) =>
        {
            _dragging = false; 
        };
        
        Append(_mainPanel);
    }

    private void AddScrollBar()
    {
        _scrollBar = new UIScrollbar()
        {
            Left = { Pixels = 300 }
        };
        _scrollBar.Height.Set(MainPanelHeight, 0);
        _scrollBar.Width.Set(5, 0f);
        _mainPanel.Append(_scrollBar);
    }

    private void AddItemList()
    {
        _itemList = new UIList
        {
            Width = { Pixels = 300 },
            Height = { Pixels = MainPanelHeight },
            Left = { Pixels = 0 },
            Top = { Pixels = 0 },
            ListPadding = 5f
        };
        _itemList.SetScrollbar(_scrollBar);
        _mainPanel.Append(_itemList);
    }
    
    private void AddItem(Structure structure)
    {
        Dictionary<ClickType, Color> colors = new()
        {
            { ClickType.Normal, new Color(30, 30, 30, 200)    },
            { ClickType.Hover,  new Color(80, 80, 80, 200)    },
            { ClickType.Click,  new Color(150, 150, 150, 200) }
        };

        UIPanel itemPanel = new()
        {
            BackgroundColor = colors[ClickType.Normal],
            Width = { Percent = 1f },
            Height = { Pixels = 30 }
        };
        
        itemPanel.SetPadding(4);
        itemPanel.OnLeftClick += (evt, elm) =>
        {
            ModContent.GetInstance<CustomShopSystem>().Hide();
            Main.LocalPlayer.GetModPlayer<StructureSilhouette>().StartDrawingOutline(structure);
        };
        
        itemPanel.OnLeftMouseDown += (evt, elm) => itemPanel.BackgroundColor = colors[ClickType.Click];
        itemPanel.OnLeftMouseUp += (evt, elm) => itemPanel.BackgroundColor = colors[ClickType.Hover];
        itemPanel.OnMouseOver += (evt, elm) => itemPanel.BackgroundColor = colors[ClickType.Hover];
        itemPanel.OnMouseOut += (evt, elm) => itemPanel.BackgroundColor = colors[ClickType.Normal];

        UIText text = new(structure.DisplayName, 0.8f)
        {
            Left = { Pixels = 5 },
            VAlign = 0.5f
        };
        
        itemPanel.Append(text);

        UIText costText = new(structure.Cost.ToString(), 0.8f)
        {
            HAlign = 1f,
            VAlign = 0.5f,
            Left = { Pixels = -5 }
        };
        
        itemPanel.Append(costText);

        _itemList.Add(itemPanel);
    }

    private void StartDrag(UIMouseEvent evt)
    {
        _dragging = true;
        _dragOffset = Main.MouseScreen - new Vector2(_mainPanel.Left.Pixels, _mainPanel.Top.Pixels);
    }
    
    private enum ClickType
    {
        Normal,
        Hover,
        Click
    }
}
