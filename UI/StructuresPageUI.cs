using Structify.Common.Players;
using Structify.UI.StructuresPage;
using Structify.Utils;
using Terraria.GameContent.UI.Elements;

namespace Structify.UI;

public class StructuresPageUI
{
    public static void Show(UIPanel panel)
    {
        UIPanel pageStructures = StructureCatalogUI.CreatePagePanel();
        
        UIScrollbar scrollBar = CreateScrollBar();
        UIList list = CreateItemList(scrollBar);
        UIPanel infoPanel = CreateInfoPanel();
        
        pageStructures.Append(scrollBar);
        pageStructures.Append(list);
        pageStructures.Append(infoPanel);

        UIText title = CreateInfoTitle();
        UIText description = CreateInfoDescription();
        UIPanel metadataPanel = CreateMetadataPanel();
        UIText metadata = CreateMetadataText();
        StructurePreviewView previewView = CreatePreviewView();
        UIButton createBtn = CreatePlaceStructureBtn();
        UIButton goBackBtn = CreateGoBackToHomePageBtn(pageStructures, panel);

        metadataPanel.Append(metadata);
        
        infoPanel.Append(title);
        infoPanel.Append(description);
        infoPanel.Append(metadataPanel);
        infoPanel.Append(previewView);
        infoPanel.Append(createBtn);
        infoPanel.Append(goBackBtn);
        
        // Prevent Terraria from automatically sorting when adding elements to the list
        list.ManualSortMethod = _ => { };

        IOrderedEnumerable<Structure> ordered = StructureCatalog.All.OrderBy(x => x.DisplayName);
        
        foreach (UIButton item in ordered.Select(structure => CreateItem(structure, title, description, metadata, previewView)))
        {
            list.Add(item);
        }

        if (StructureCatalog.All.Count > 0)
            SetStructureSelection(StructureCatalog.All[0], title, description, metadata, previewView);
        
        panel.Append(pageStructures);
    }
    
    private static UIButton CreateGoBackToHomePageBtn(UIPanel pageStructures, UIPanel panel)
    {
        UIButton button = new("Go Back", Colors.Background, textScale: 1.0f)
        {
            HAlign = 0.0f,
            VAlign = 1.0f,
            Top = { Pixels = -8 }
        };

        button.OnLeftClick += (evt, elm) =>
        {
            Hide(pageStructures);
            HomepageUI.Show(panel);
        };

        return button;
    }
    
    private static UIButton CreatePlaceStructureBtn()
    {
        UIButton button = new("Place Structure", Colors.Background, textScale: 1.0f)
        {
            HAlign = 0.5f,
            VAlign = 1.0f,
            Top = { Pixels = -8 }
        };

        button.OnLeftClick += (evt, elm) =>
        {
            bool purchased = Main.player[Main.myPlayer].BuyItem(StructureCatalogUI.SelectedStructure.Cost);

            if (purchased)
            {
                ModContent.GetInstance<CustomShopSystem>().Hide();
                Main.LocalPlayer.GetModPlayer<StructureSilhouette>().StartDrawingOutline(StructureCatalogUI.SelectedStructure);
            }
            else
            {
                int needed = StructureCatalogUI.SelectedStructure.Cost - Helpers.GetPlayerCoinCount();
                Main.NewText($"[c/{Colors.SecondaryHex}:!!!] [c/{Colors.PrimaryHex}:Could not purchase] [c/{Colors.SecondaryHex}:{StructureCatalogUI.SelectedStructure.DisplayName}] [c/{Colors.PrimaryHex}:because you are short by] {Helpers.FormatPrice(needed)}[c/{Colors.PrimaryHex}:.]");
            }
        };

        return button;
    }
    
    private static UIButton CreateItem(
        Structure structure,
        UIText title,
        UIText description,
        UIText metadata,
        StructurePreviewView previewView)
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
            SetStructureSelection(structure, title, description, metadata, previewView);
        };
        
        return itemPanel;
    }

    private static void SetStructureSelection(
        Structure structure,
        UIText title,
        UIText description,
        UIText metadata,
        StructurePreviewView previewView)
    {
        title.SetText($"[c/{Colors.SecondaryHex}:{structure.DisplayName}]");
        description.SetText(StructureSelectionFormatter.GetDescriptionText(structure));
        metadata.SetText(StructureSelectionFormatter.GetMetadataText(structure));
        previewView.SetStructure(structure);
        StructureCatalogUI.SelectedStructure = structure;
    }
    
    private static UIText CreateInfoTitle()
    {
        return new UIText(string.Empty, 0.6f, true)
        {
            Width = { Percent = 1.0f },
            HAlign = 0.5f
        };
    }

    private static UIText CreateInfoDescription()
    {
        return new UIText(string.Empty, 0.9f)
        {
            Top = { Pixels = 42 },
            Width = { Percent = 1.0f },
            TextOriginX = 0f,
            IsWrapped = true,
            TextColor = Color.Gray
        };
    }

    private static UIPanel CreateMetadataPanel()
    {
        UIPanel panel = new()
        {
            Top = { Pixels = 132 },
            Width = { Pixels = 270 },
            Height = { Pixels = 186 },
            BackgroundColor = Color.Transparent,
            BorderColor = Color.Transparent
        };

        panel.SetPadding(8f);
        return panel;
    }

    private static UIText CreateMetadataText()
    {
        return new UIText(string.Empty, 0.9f)
        {
            Width = { Percent = 1.0f },
            TextOriginX = 0f,
            IsWrapped = false,
            TextColor = Color.Gray
        };
    }

    private static StructurePreviewView CreatePreviewView()
    {
        return new StructurePreviewView
        {
            Left = { Pixels = 286 },
            Top = { Pixels = 132 },
            Width = { Pixels = StructureCatalogUI.MainPanelWidth - 240 - 286 },
            Height = { Pixels = 186 }
        };
    }
    
    private static UIList CreateItemList(UIScrollbar scrollBar)
    {
        UIList list = new()
        {
            Width = { Pixels = 200 },
            Height = { Pixels = StructureCatalogUI.MainPanelHeight },
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
        
        scrollBar.Height.Set(StructureCatalogUI.MainPanelHeight, 0);
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
            Width = { Pixels = StructureCatalogUI.MainPanelWidth - 240 },
            Height = { Percent = 1.0f }
        };

        return panel;
    }
    
    private static void Hide(UIPanel pageStructures)
    {
        pageStructures?.Remove();
    }
}