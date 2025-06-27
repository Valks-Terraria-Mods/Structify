using System.Text;
using Structify.Common.Players;
using Structify.Utils;
using StructureHelper.API;
using Terraria.GameContent.UI.Elements;

namespace Structify.UI;

public class StructureCatalogUI : DraggableUIPanelState
{
    public const string PrimaryColorHex = "969696"; // Gray
    public const string SecondaryColorHex = "32FF82"; // Green
    
    private const float MainPanelWidth = 800;
    private const float MainPanelHeight = 400;

    private readonly Color BackgroundColor = new(50, 50, 50); // Blackish
    private readonly Color PrimaryColor = new(150, 150, 150); // Gray
    private readonly Color SecondaryColor = new(50, 255, 130); // Green
    
    private Structure _selectedStructure;
    private UIPanel _pageStructures;
    private UIPanel _pageHome;

    public override void OnInitialize()
    {
        base.OnInitialize();
        _selectedStructure = StructureCatalog.All[0];
        Width = MainPanelWidth;
        Height = MainPanelHeight;
        
        ShowHomePage();
    }

    private void ShowHomePage()
    {
        _pageHome = CreatePagePanel();

        UIText title = new(Helpers.GradientText("Structify", Color.Green, new Color(150, 255, 150)), 0.7f, large: true)
        {
            Top = { Pixels = 20 },
            HAlign = 0.5f
        };

        UIText info = new("Structify, the ever growing structure mod.", 0.9f)
        {
            Top = { Pixels = 60 },
            HAlign = 0.5f,
            TextColor = PrimaryColor
        };

        UIButton structuresPageBtn = new(">> Click to View Structures <<", 0.7f, updateWidth: true, largeText: true)
        {
            HAlign = 0.5f,
            VAlign = 0.5f,
        };
        
        structuresPageBtn.SetTextColor(SecondaryColor);
        structuresPageBtn.OnLeftClick += (evt, elm) =>
        {
            HideHomePage();
            ShowStructuresPage();
        };

        string newBuilders = Builders.GetCurrentBuilders();
        string oldBuilders = Builders.GetPreviousBuilders();

        string thankYou = 
            $"Thank you to [c/{SecondaryColorHex}:{newBuilders}] and the previous builders [c/{SecondaryColorHex}:{oldBuilders}] for helping!";

        UIText someText = new(thankYou, 0.9f)
        {
            HAlign = 0.5f,
            VAlign = 0.85f,
            TextColor = PrimaryColor,
            Width = { Pixels = MainPanelWidth }
        };

        UIButton discordBtn = new("Discord", 1.0f)
        {
            HAlign = 1.0f,
            VAlign = 1.0f
        };

        discordBtn.OnLeftClick += (evt, elm) =>
        {
            Terraria.Utils.OpenToURL("https://discord.gg/j8HQZZ76r8");
        };

        UIButton gitHubBtn = new("GitHub", 1.0f)
        {
            HAlign = 0.0f,
            VAlign = 1.0f
        };

        gitHubBtn.OnLeftClick += (evt, elm) =>
        {
            Terraria.Utils.OpenToURL("https://github.com/Valks-Terraria-Mods/Structify");
        };

        _pageHome.Append(title);
        _pageHome.Append(info);
        _pageHome.Append(someText);
        _pageHome.Append(structuresPageBtn);
        _pageHome.Append(discordBtn);
        _pageHome.Append(gitHubBtn);

        Panel.Append(_pageHome);
    }

    private void HideHomePage()
    {
        _pageHome?.Remove();
    }

    private void HideStructuresPage()
    {
        _pageStructures?.Remove();
    }

    private static UIPanel CreatePagePanel()
    {
        UIPanel panel = new()
        {
            BackgroundColor = Color.Transparent,
            BorderColor = Color.Transparent,
            Width = { Pixels = MainPanelWidth },
            Height = { Pixels = MainPanelHeight },
        };
        
        panel.SetPadding(0);

        return panel;
    }

    private void ShowStructuresPage()
    {
        _pageStructures = CreatePagePanel();
        
        UIScrollbar scrollBar = CreateScrollBar();
        UIList list = CreateItemList(scrollBar);
        UIPanel infoPanel = CreateInfoPanel();
        
        _pageStructures.Append(scrollBar);
        _pageStructures.Append(list);
        _pageStructures.Append(infoPanel);

        UIText title = CreateInfoTitle();
        UIText description = CreateInfoDescription();
        UIButton createBtn = CreatePlaceStructureBtn();
        UIButton goBackBtn = CreateGoBackToHomePageBtn();
        
        infoPanel.Append(title);
        infoPanel.Append(description);
        infoPanel.Append(createBtn);
        infoPanel.Append(goBackBtn);
        
        foreach (UIButton item in StructureCatalog.All.Select(structure => CreateItem(structure, title, description)))
        {
            list.Add(item);
        }
        
        Panel.Append(_pageStructures);
    }

    private UIButton CreateGoBackToHomePageBtn()
    {
        UIButton button = new("Go Back", BackgroundColor, textScale: 1.0f)
        {
            HAlign = 0.0f,
            VAlign = 1.0f
        };

        button.OnLeftClick += (evt, elm) =>
        {
            HideStructuresPage();
            ShowHomePage();
        };

        return button;
    }
    
    private UIButton CreatePlaceStructureBtn()
    {
        UIButton button = new("Place Structure", BackgroundColor, textScale: 1.0f)
        {
            HAlign = 0.5f,
            VAlign = 1.0f
        };

        button.OnLeftClick += (evt, elm) =>
        {
            bool purchased = Main.player[Main.myPlayer].BuyItem(_selectedStructure.Cost);

            if (purchased)
            {
                ModContent.GetInstance<CustomShopSystem>().Hide();
                Main.LocalPlayer.GetModPlayer<StructureSilhouette>().StartDrawingOutline(_selectedStructure);
            }
            else
            {
                int needed = _selectedStructure.Cost - Helpers.GetPlayerCoinCount();
                Main.NewText($"[c/{SecondaryColorHex}:!!!] [c/{PrimaryColorHex}:Could not purchase] [c/{SecondaryColorHex}:{_selectedStructure.DisplayName}] [c/{PrimaryColorHex}:because you are short by] {Helpers.FormatPrice(needed)}[c/{PrimaryColorHex}:.]");
            }
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
        
        itemPanel.SetTextColor(Color.LightGray);
        itemPanel.CenterText();
        
        itemPanel.OnLeftClick += (evt, elm) =>
        {
            title.SetText($"[c/{SecondaryColorHex}:{structure.DisplayName}]");
            description.SetText(Helpers.GetInfo(structure));
            _selectedStructure = structure;
        };
        
        return itemPanel;
    }
    
    private static UIText CreateInfoTitle()
    {
        return new UIText($"[c/{SecondaryColorHex}:{StructureCatalog.All[0].DisplayName}]", 0.6f, true)
        {
            Width = { Percent = 1.0f },
            HAlign = 0.5f
        };
    }

    private static UIText CreateInfoDescription()
    {
        return new UIText(Helpers.GetInfo(StructureCatalog.All[0]), 0.9f)
        {
            Top = { Pixels = 40 },
            Width = { Percent = 1.0f},
            TextOriginX = 0f,
            IsWrapped = true,
            TextColor = Color.Gray
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
            BorderColor = Color.Transparent,
            Left = { Pixels = 220 },
            Width = { Pixels = MainPanelWidth - 240 },
            Height = { Percent = 1.0f }
        };

        return panel;
    }
}
