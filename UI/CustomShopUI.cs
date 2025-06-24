using Microsoft.Xna.Framework.Graphics;
using Structify.Common.Items;
using Structify.Common.Players;
using StructureHelper.API;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace Structify.UI;

public class CustomShopUI : UIState
{
    private UIPanel _mainPanel;
    private UIList _itemList;
    private UIScrollbar _scrollBar;

    private struct Entry(int type, string name, int cost)
    {
        public readonly int Type = type;
        public readonly string Name = name;
        public readonly int Cost = cost;
    }

    private List<Entry> _entries;
    
    private bool _dragging;
    private Vector2 _dragOffset;

    public override void OnInitialize()
    {
        _entries = ModContent.GetContent<StructureItem>()
            .Where(m => m.Mod == ModContent.GetInstance<Structify>())
            .Select(m => new Entry(
                type: m.Type,
                name: m.ItemName,
                cost: m.Item.shopCustomPrice ?? 0
            ))
            .ToList();
        
        // Main draggable panel
        _mainPanel = new UIPanel
        {
            BackgroundColor = new Color(0, 0, 0, 150),
            Width = { Pixels = 400 },
            Height = { Pixels = 500 },
            Left = { Pixels = 200 },
            Top = { Pixels = 100 }
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

        // Scrollbar
        _scrollBar = new UIScrollbar()
        {
            Left = { Pixels = 10 }
        };
        _scrollBar.Height.Set(0, 1f);
        _scrollBar.Width.Set(20, 0f);
        _scrollBar.HAlign = 1;
        _mainPanel.Append(_scrollBar);

        // Item list
        _itemList = new UIList
        {
            Width = { Pixels = 350 },
            Height = { Pixels = 430 },
            HAlign = 0.5f,
            VAlign = 0.5f,
            ListPadding = 5f
        };
        _itemList.SetScrollbar(_scrollBar);
        _mainPanel.Append(_itemList);

        PopulateList();
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

    private void PopulateList()
    {
        _itemList.Clear();
        
        foreach (Entry entry in _entries)
        {
            string name = entry.Name;

            UIPanel itemPanel = new()
            {
                BackgroundColor = new Color(30, 30, 30, 200),
                Width = { Percent = 1f },
                Height = { Pixels = 30 }
            };
            
            itemPanel.SetPadding(4);
            itemPanel.OnLeftClick += (evt, elm) => TrySpawn(entry.Type);
            itemPanel.OnMouseOver += (evt, elm) => itemPanel.BackgroundColor = new Color(80, 80, 80, 200);
            itemPanel.OnMouseOut += (evt, elm) => itemPanel.BackgroundColor = new Color(30, 30, 30, 200);

            UIText text = new(name, 0.8f)
            {
                Left = { Pixels = 5 },
                VAlign = 0.5f
            };
            itemPanel.Append(text);

            UIText costText = new(entry.Cost + "", 0.8f)
            {
                HAlign = 1f,
                VAlign = 0.5f,
                Left = { Pixels = -5 }
            };
            itemPanel.Append(costText);

            _itemList.Add(itemPanel);
        }
    }

    private static void TrySpawn(int type)
    {
        Player player = Main.LocalPlayer;
        player.QuickSpawnItem(player.GetSource_OpenItem(type), type);
    }

    private void StartDrag(UIMouseEvent evt)
    {
        _dragging = true;
        _dragOffset = Main.MouseScreen - new Vector2(_mainPanel.Left.Pixels, _mainPanel.Top.Pixels);
    }
}
