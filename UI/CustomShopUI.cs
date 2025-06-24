using Microsoft.Xna.Framework.Graphics;
using Structify.Common.Players;
using Structify.Utils;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace Structify.UI;

public class Structure()
{
    public string Schematic { get; init; }
    public string DisplayName { get; init; }
    public string Description { get; init; }
    public int Cost { get; init; }
    public int Offset { get; init; }
    public bool Procedural { get; init; }
    public string[] Authors { get; init; }
}

public class CustomShopUI : UIState
{
    private const float MainPanelWidth = 800;
    private const float MainPanelHeight = 400;
    
    private UIPanel _mainPanel;
    private UIList _itemList;
    private UIScrollbar _scrollBar;
    
    private readonly List<Structure> _structures =
    [
        new Structure
        {
            Schematic = "BossArena",
            DisplayName = "Boss Arena",
            Description = "A small section of an arena meant to be stacked adjacently with itself.",
            Cost = 1000,
            Offset = 4,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "Castle1",
            DisplayName = "Castle",
            Description = "A mostly empty castle for all your storage needs.",
            Cost = 30000,
            Offset = 15,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "FishingPond1",
            DisplayName = "Fishing Pond",
            Description = "A small pond surrounded by a small hut on either side.",
            Cost = 2500,
            Offset = 7,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "Greenhouse1",
            DisplayName = "Greenhouse",
            Description = "The structure comes with many flower pots.",
            Cost = 2500,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "Greenhouse2",
            DisplayName = "Overgrown House",
            Description = "The interior is empty so you can place what you want.",
            Cost = 3000,
            Offset = 16,
            Authors = [Builders.Grim]
        },
        new Structure
        {
            Schematic = "JungleFarm",
            DisplayName = "Jungle Farm",
            Description = "Several rows of mud and grass usually placed deep inside the jungle.",
            Cost = 500,
            Offset = 4,
            Authors = [Builders.Toast]
        },
        new Structure
        {
            Schematic = "LargeHouse1",
            DisplayName = "Large House",
            Description = "A large house that can hold 3 NPC's and comes with a basement and actic.",
            Cost = 3000,
            Offset = 6,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "MediumHouse1",
            DisplayName = "Medium House",
            Description = "A medium house that can hold 2 NPC's and comes with a basement.",
            Cost = 2000,
            Offset = 6,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "SmallHouse1",
            DisplayName = "Small House",
            Description = "A small house that can hold 1 NPC and comes with a basement.",
            Cost = 1000,
            Offset = 5,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "PrisonCell1",
            DisplayName = "Underground Prison Cell",
            Description = "Holds 1 NPC, usually placed deep underground.",
            Cost = 100,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "PrisonCell2",
            DisplayName = "Wide Prison Cell",
            Description = "Holds 1 NPC, entry is from top. Usually placed near surface underground.",
            Cost = 100,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "PrisonCell3",
            DisplayName = "Sky Prison Cell",
            Description = "Holds 1 NPC, usually placed in the sky.",
            Cost = 100,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "Ship1",
            DisplayName = "Boat",
            Description = "A boat to place on the water for fishing perhaps?",
            Cost = 200,
            Offset = 5,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "TowerGate1",
            DisplayName = "Tower Gate",
            Description = "Defensive structure commonly used to divide biomes.",
            Cost = 400,
            Offset = 2,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "UndergroundHouse1",
            DisplayName = "Underground House",
            Description = "The interior is empty allowing you to decorate it yourself.",
            Cost = 250,
            Offset = 1,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "UnderWaterDome",
            DisplayName = "Underwater Dome",
            Description = "A structure you can place deep under water in the sea.",
            Cost = 100,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            Schematic = "WallOfFleshArena",
            DisplayName = "Wall of Flesh Arena",
            Description = "Can be stacked horizontally.",
            Cost = 100,
            Offset = 5,
            Authors = [Builders.Valkyrienyanko]
        },
        new Structure
        {
            DisplayName = "Hellavator",
            Description = "An elevator to hell.",
            Cost = 10000,
            Authors = [Builders.Valkyrienyanko],
            Procedural = true
        }
    ];
    
    private bool _dragging;
    private Vector2 _dragOffset;

    public override void OnInitialize()
    {
        AddMainPanel();
        AddScrollBar();
        AddItemList();
        
        foreach (Structure structure in _structures)
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

    private enum ClickType
    {
        Normal,
        Hover,
        Click
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
}
