using Structify.Common.Players;
using Terraria.GameContent.UI.Elements;

namespace Structify.UI;

public class StructureCatalogUI : DraggableUIPanelState
{
    private const float MainPanelWidth = 800;
    private const float MainPanelHeight = 400;
    
    private Structure _selectedStructure;

    public override void OnInitialize()
    {
        base.OnInitialize();
        _selectedStructure = StructureCatalog.All[0];
        Width = MainPanelWidth;
        Height = MainPanelHeight;
        
        UIScrollbar scrollBar = CreateScrollBar();
        UIList list = CreateItemList(scrollBar);
        UIPanel infoPanel = CreateInfoPanel();
        
        Panel.Append(scrollBar);
        Panel.Append(list);
        Panel.Append(infoPanel);

        UIText title = CreateInfoTitle();
        UIText description = CreateInfoDescription();
        UIButton createBtn = CreatePlaceStructureBtn();
        
        infoPanel.Append(title);
        infoPanel.Append(description);
        infoPanel.Append(createBtn);
        
        foreach (UIButton item in StructureCatalog.All.Select(structure => CreateItem(structure, title, description)))
        {
            list.Add(item);
        }
    }
    
    private UIButton CreatePlaceStructureBtn()
    {
        UIButton button = new("Place Structure", new Color(50, 50, 50), textScale: 1.0f)
        {
            HAlign = 0.5f,
            VAlign = 1.0f
        };

        button.OnLeftClick += (evt, elm) =>
        {
            ModContent.GetInstance<CustomShopSystem>().Hide();
            Main.LocalPlayer.GetModPlayer<StructureSilhouette>().StartDrawingOutline(_selectedStructure);
        };

        return button;
    }
    
    private UIButton CreateItem(Structure structure, UIText title, UIText description)
    {
        UIButton itemPanel = new(structure.DisplayName, updateWidth: false)
        {
            Width = { Percent = 1f },
            Height = { Pixels = 30 }
        };
        
        itemPanel.CenterText();
        
        itemPanel.OnLeftClick += (evt, elm) =>
        {
            title.SetText($"[c/32FF82:{structure.DisplayName}]");
            description.SetText(GetInfo(structure));
            _selectedStructure = structure;
        };
        
        return itemPanel;
    }
    
    private static UIText CreateInfoTitle()
    {
        return new UIText($"[c/32FF82:{StructureCatalog.All[0].DisplayName}]", 0.6f, true)
        {
            Width = { Percent = 1.0f },
            HAlign = 0.5f
        };
    }

    private static UIText CreateInfoDescription()
    {
        return new UIText(GetInfo(StructureCatalog.All[0]), 0.9f)
        {
            Top = { Pixels = 30 },
            Width = { Percent = 1.0f},
            TextOriginX = 0f,
            IsWrapped = true
        };
    }
    
    private static UIList CreateItemList(UIScrollbar scrollBar)
    {
        UIList list = new()
        {
            Width = { Pixels = 200 },
            Height = { Pixels = MainPanelHeight },
            Left = { Pixels = 0 },
            Top = { Pixels = 0 },
            ListPadding = 5f
        };
        
        list.SetScrollbar(scrollBar);

        return list;
    }
    
    private static UIScrollbar CreateScrollBar()
    {
        UIScrollbar scrollBar = new()
        {
            Left = { Pixels = 200 }
        };
        
        scrollBar.Height.Set(MainPanelHeight, 0);
        scrollBar.Width.Set(5, 0f);
        
        return scrollBar;
    }
    
    private static UIPanel CreateInfoPanel()
    {
        UIPanel panel = new()
        {
            BackgroundColor = Color.Transparent,
            Left = { Pixels = 220 },
            Width = { Pixels = MainPanelWidth - 240 },
            Height = { Percent = 1.0f }
        };

        return panel;
    }
    
    private static string GetInfo(Structure structure)
    {
        string info = 
            $"{structure.Description}\n\n" +
            $"Built by [c/32FF82:{FormatAuthors(structure.Authors)}].";
        
        return info;
    }
    
    /// <summary>
    /// Formats the author string into a nice readable format.
    /// </summary>
    private static string FormatAuthors(IEnumerable<string> authors)
    {
        List<string> authorList = authors.ToList();

        switch (authorList.Count)
        {
            case 0:
                return string.Empty;
            case 1:
                // "A"
                return authorList[0];
            case 2:
                // "A and B"
                return $"{authorList[0]} and {authorList[1]}";
        }

        // For 3 or more authors: "A, B and C"
        IEnumerable<string> allButLast = authorList.Take(authorList.Count - 1);
        string lastAuthor = authorList.Last();

        return $"{string.Join(", ", allButLast)} and {lastAuthor}";
    }
}
