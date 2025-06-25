using System.Text;
using Structify.Common.Players;
using Structify.Utils;
using StructureHelper.API;
using Terraria.GameContent.UI.Elements;

namespace Structify.UI;

public class StructureCatalogUI : DraggableUIPanelState
{
    private const float MainPanelWidth = 800;
    private const float MainPanelHeight = 400;

    private readonly Color BackgroundColor = new(50, 50, 50); // Blackish
    private readonly Color PrimaryColor = new(150, 150, 150); // Gray
    private readonly Color SecondaryColor = new(50, 255, 130); // Green
    private const string PrimaryColorHex = "969696"; // Gray
    private const string SecondaryColorHex = "32FF82"; // Green
    
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

        UIText title = new(GradientText("Structify", Color.Green, new Color(150, 255, 150)), 0.7f, large: true)
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
    
    /// <summary>
    /// Wraps each character of <paramref name="text"/> in a [c/rrggbb:char] tag,
    /// interpolating from <paramref name="start"/> to <paramref name="end"/>.
    /// </summary>
    public static string GradientText(string text, Color start, Color end)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        StringBuilder sb = new();
        int len = text.Length;
        for (int i = 0; i < len; i++)
        {
            float t = i / (float)(len - 1);
            Color c = new Color(
                (byte)MathHelper.Lerp(start.R, end.R, t),
                (byte)MathHelper.Lerp(start.G, end.G, t),
                (byte)MathHelper.Lerp(start.B, end.B, t)
            );
            // format as RRBBGG
            string hex = $"{c.R:X2}{c.G:X2}{c.B:X2}";
            sb.Append($"[c/{hex}:{text[i]}]");
        }
        return sb.ToString();
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
                int needed = _selectedStructure.Cost - GetPlayerCoinCount();
                Main.NewText($"[c/{SecondaryColorHex}:!!!] [c/{PrimaryColorHex}:Could not purchase] [c/{SecondaryColorHex}:{_selectedStructure.DisplayName}] [c/{PrimaryColorHex}:because you are short by] {FormatPrice(needed)}[c/{PrimaryColorHex}:.]");
            }
        };

        return button;
    }

    private static int GetPlayerCoinCount()
    {
        Player player = Main.LocalPlayer;
        
        int playerCoins = 0;
        playerCoins += player.CountItem(ItemID.PlatinumCoin) * 1_000_000;
        playerCoins += player.CountItem(ItemID.GoldCoin)     * 10_000;
        playerCoins += player.CountItem(ItemID.SilverCoin)   * 100;
        playerCoins += player.CountItem(ItemID.CopperCoin);

        return playerCoins;
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
            description.SetText(GetInfo(structure));
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
        return new UIText(GetInfo(StructureCatalog.All[0]), 0.9f)
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
    
    private static string GetInfo(Structure structure)
    {
        string info = $"{structure.Description}";
        
        if (structure.NPCs > 0)
            info += $"\nHas [c/{SecondaryColorHex}:{structure.NPCs}] NPC rooms.";

        info += "\n";
        
        if (!structure.Procedural && structure.Offset > 0)
        {
            info += $"\nRooted [c/{SecondaryColorHex}:{structure.Offset}] tiles beneath the surface.";
        }

        if (!structure.Procedural)
        {
            string path = $"Schematics/{structure.Schematic}.shstruct";
            Point16 dimensions = Generator.GetStructureDimensions(path, ModContent.GetInstance<Structify>());
            info += $"\nSize: [c/{SecondaryColorHex}:{dimensions.X}] x [c/{SecondaryColorHex}:{dimensions.Y}]";
        }

        info += $"\nCost: {FormatPrice(structure.Cost)}";

        info += $"\n\nBuilt by [c/{SecondaryColorHex}:{FormatAuthors(structure.Authors)}].";
        
        return info;
    }
    
    private static string FormatPrice(int price) {
        int platinum = price / 1_000_000;
        int gold     = (price /   10_000) % 100;
        int silver   = (price /      100) % 100;
        int copper   = price % 100;
        
        List<string> parts = [];
        if (platinum > 0) parts.Add($"{platinum} platinum");
        if (gold     > 0) parts.Add($"{gold} gold");
        if (silver   > 0) parts.Add($"{silver} silver");
        if (copper   > 0) parts.Add($"{copper} copper");

        if (parts.Count == 0)
            return $"[c/{PrimaryColorHex}:No value]"; // Dark Gray (aka Color.Gray)

        string combined = string.Join(" ", parts);
        
        string hex;
        if (platinum > 0) hex = "E5E4E2";   // Platinum
        else if (gold > 0) hex = "FFD700";   // Gold
        else if (silver > 0) hex = "C0C0C0"; // Silver
        else               hex = "B87333";   // Copper
        
        return $"[c/{hex}:{combined}]";
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
