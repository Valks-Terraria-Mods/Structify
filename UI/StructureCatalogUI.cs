using Structify.Common.Players;
using Terraria.GameContent.UI.Elements;

namespace Structify.UI;

public class StructureCatalogUI : DraggableUIPanelState
{
    private const float MainPanelWidth = 800;
    private const float MainPanelHeight = 400;
    
    private UIList _itemList;
    private UIText _description;
    private UIText _authors;
    private UIButton _placeStructure;
    private UIScrollbar _scrollBar;
    private Structure _selectedStructure;

    public override void OnInitialize()
    {
        base.OnInitialize();
        _selectedStructure = StructureCatalog.All[0];
        Width = MainPanelWidth;
        Height = MainPanelHeight;
        
        AddScrollBar();
        AddItemList();
        AddDescription();
        AddAuthors();
        AddPlaceStructureButton();
        
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

    private void AddPlaceStructureButton()
    {
        _placeStructure = new UIButton("Place " + StructureCatalog.All[0].DisplayName, Color.Green)
        {
            Left = { Pixels = 450 },
            Top = { Pixels = 350 }
        };

        _placeStructure.OnLeftClick += (evt, elm) =>
        {
            ModContent.GetInstance<CustomShopSystem>().Hide();
            Main.LocalPlayer.GetModPlayer<StructureSilhouette>().StartDrawingOutline(_selectedStructure);
        };
        
        Panel.Append(_placeStructure);
    }

    private void AddDescription()
    {
        _description = new UIText(StructureCatalog.All[0].Description, 0.9f)
        {
            Left = { Pixels = 320 },
            Top = { Pixels = 0 },
            Width = { Pixels = MainPanelWidth - 320 },
            TextOriginX = 0f,
            IsWrapped = true
        };
        
        Panel.Append(_description);
    }

    private void AddAuthors()
    {
        _authors = new UIText("This structure was built by " + FormatAuthors(StructureCatalog.All[0].Authors), 0.9f)
        {
            Left = { Pixels = 320 },
            Top = { Pixels = 60 },
            Width = { Pixels = MainPanelWidth - 320 },
            TextOriginX = 0f,
            IsWrapped = true
        };
        
        Panel.Append(_authors);
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
        UIButton itemPanel = new(structure.DisplayName, updateWidth: false)
        {
            Width = { Percent = 1f },
            Height = { Pixels = 30 }
        };
        
        itemPanel.OnLeftClick += (evt, elm) =>
        {
            _description.SetText(structure.Description);
            _authors.SetText("This structure was built by " + FormatAuthors(structure.Authors));
            _placeStructure.SetText("Place " + structure.DisplayName);
            _selectedStructure = structure;
        };

        UIText costText = new(structure.Cost.ToString(), 0.8f)
        {
            HAlign = 1f,
            VAlign = 0.5f,
            Left = { Pixels = -5 }
        };
        
        itemPanel.Append(costText);

        _itemList.Add(itemPanel);
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
