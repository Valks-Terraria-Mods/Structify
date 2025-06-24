using Structify.Common.Players;
using StructureHelper.API;
using Terraria.GameContent.UI.Elements;

namespace Structify.UI;

public class StructureCatalogUI : DraggableUIPanelState
{
    private const float MainPanelWidth = 800;
    private const float MainPanelHeight = 400;
    private const string HighlightColor = "32FF82";
    
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
            bool purchased = Main.player[Main.myPlayer].BuyItem(_selectedStructure.Cost);

            if (purchased)
            {
                ModContent.GetInstance<CustomShopSystem>().Hide();
                Main.LocalPlayer.GetModPlayer<StructureSilhouette>().StartDrawingOutline(_selectedStructure);
            }
            else
            {
                int needed = _selectedStructure.Cost - GetPlayerCoinCount();
                Main.NewText($"[c/808080:Could not purchase] [c/{HighlightColor}:{_selectedStructure.DisplayName}] [c/808080:because you are short by] {FormatPrice(needed)}[c/808080:.]");
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
            title.SetText($"[c/{HighlightColor}:{structure.DisplayName}]");
            description.SetText(GetInfo(structure));
            _selectedStructure = structure;
        };
        
        return itemPanel;
    }
    
    private static UIText CreateInfoTitle()
    {
        return new UIText($"[c/{HighlightColor}:{StructureCatalog.All[0].DisplayName}]", 0.6f, true)
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
            info += $"\nHas [c/{HighlightColor}:{structure.NPCs}] NPC rooms.";

        info += "\n";
        
        if (!structure.Procedural && structure.Offset > 0)
        {
            info += $"\nRooted [c/{HighlightColor}:{structure.Offset}] tiles beneath the surface.";
        }

        if (!structure.Procedural)
        {
            string path = $"Schematics/{structure.Schematic}.shstruct";
            Point16 dimensions = Generator.GetStructureDimensions(path, ModContent.GetInstance<Structify>());
            info += $"\nSize: [c/{HighlightColor}:{dimensions.X}] x [c/{HighlightColor}:{dimensions.Y}]";
        }

        info += $"\nCost: {FormatPrice(structure.Cost)}";

        info += $"\n\nBuilt by [c/{HighlightColor}:{FormatAuthors(structure.Authors)}].";
        
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
            return "[c/808080:No value]"; // Dark Gray (aka Color.Gray)

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
