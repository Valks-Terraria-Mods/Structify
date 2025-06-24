using Structify.Common.Players;
using Terraria.GameContent.UI.Elements;

namespace Structify.UI;

public class StructureCatalogUI : DraggableUIPanelState
{
    private const float MainPanelWidth = 800;
    private const float MainPanelHeight = 400;
    
    private UIList _itemList;
    private UIScrollbar _scrollBar;

    public override void OnInitialize()
    {
        base.OnInitialize();
        Width = MainPanelWidth;
        Height = MainPanelHeight;
        
        AddScrollBar();
        AddItemList();
        
        foreach (Structure structure in StructureCatalog.All)
        {
            AddItem(structure);
        }
    }

    private void AddScrollBar()
    {
        _scrollBar = new UIScrollbar()
        {
            Left = { Pixels = 300 }
        };
        
        _scrollBar.Height.Set(MainPanelHeight, 0);
        _scrollBar.Width.Set(5, 0f);
        
        Panel.Append(_scrollBar);
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
        
        Panel.Append(_itemList);
    }
    
    private void AddItem(Structure structure)
    {
        UIButton itemPanel = new()
        {
            Width = { Percent = 1f },
            Height = { Pixels = 30 }
        };
        
        itemPanel.OnLeftClick += (evt, elm) =>
        {
            ModContent.GetInstance<CustomShopSystem>().Hide();
            Main.LocalPlayer.GetModPlayer<StructureSilhouette>().StartDrawingOutline(structure);
        };

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
}
