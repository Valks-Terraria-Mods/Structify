using Structify.Utils;
using Terraria.GameContent.UI.Elements;

namespace Structify.UI;

public class HomepageUI
{
    public static void Show(UIPanel panel)
    {
        UIPanel _pageHome = StructureCatalogUI.CreatePagePanel();

        UIText title = new(Helpers.GradientText(nameof(Structify), Color.Green, new Color(150, 255, 150)), 0.7f, large: true)
        {
            Top = { Pixels = 20 },
            HAlign = 0.5f
        };

        UIText info = new($"{nameof(Structify)}, the ever growing structure mod.", 0.9f)
        {
            Top = { Pixels = 60 },
            HAlign = 0.5f,
            TextColor = Colors.Primary
        };

        UIButton structuresPageBtn = new(">> Click to View Structures <<", 0.7f, updateWidth: true, largeText: true)
        {
            HAlign = 0.5f,
            VAlign = 0.5f,
        };
        
        structuresPageBtn.SetTextColor(Colors.Secondary);
        structuresPageBtn.OnLeftClick += (evt, elm) =>
        {
            Hide(_pageHome);
            StructuresPageUI.Show(panel);
        };

        string newBuilders = Builders.GetCurrentBuilders();
        string oldBuilders = Builders.GetPreviousBuilders();

        string thankYou = 
            $"Thank you to [c/{Colors.SecondaryHex}:{newBuilders}] and the previous builders [c/{Colors.SecondaryHex}:{oldBuilders}] for helping!";

        UIText someText = new(thankYou, 0.9f)
        {
            HAlign = 0.5f,
            VAlign = 0.85f,
            TextColor = Colors.Primary,
            Width = { Pixels = StructureCatalogUI.MainPanelWidth }
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

        panel.Append(_pageHome);
    }

    private static void Hide(UIPanel _pageHome)
    {
        _pageHome?.Remove();
    }
}