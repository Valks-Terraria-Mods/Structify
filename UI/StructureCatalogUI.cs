using Terraria.GameContent.UI.Elements;

namespace Structify.UI;

public class StructureCatalogUI : DraggableUIPanelState
{
    public const float MainPanelWidth = 800;
    public const float MainPanelHeight = 400;

    public static Structure SelectedStructure { get; set; }

    public override void OnInitialize()
    {
        base.OnInitialize();
        SelectedStructure = StructureCatalog.All[0];
        Width = MainPanelWidth;
        Height = MainPanelHeight;
        
        StructuresPageUI.Show(Panel);
    }
    
    public static UIPanel CreatePagePanel()
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
}
