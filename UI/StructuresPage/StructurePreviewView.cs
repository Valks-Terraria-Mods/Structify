using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria;

namespace Structify.UI.StructuresPage;

public class StructurePreviewView : UIPanel
{
    private readonly UIImage _previewImage;
    private readonly UIText _statusText;

    private Structure _selectedStructure;
    private bool _shouldPollPreview;

    public StructurePreviewView()
    {
        BackgroundColor = new Color(0, 0, 0, 60);
        BorderColor = Color.Transparent;
        SetPadding(8f);

        _previewImage = new UIImage(TextureAssets.MagicPixel)
        {
            Width = { Percent = 1.0f },
            Height = { Percent = 1.0f },
            HAlign = 0.5f,
            VAlign = 0.5f,
            ScaleToFit = true,
            Color = Color.Transparent
        };

        _statusText = new UIText("", 0.85f)
        {
            Width = { Percent = 0.9f },
            HAlign = 0.5f,
            VAlign = 0.5f,
            TextColor = Color.Gray,
            IsWrapped = true
        };

        Append(_previewImage);
        Append(_statusText);
    }

    public void SetStructure(Structure structure)
    {
        _selectedStructure = structure;
        _previewImage.SetImage(TextureAssets.MagicPixel);
        _previewImage.Color = Color.Transparent;
        _statusText.SetText("Loading preview...");
        _shouldPollPreview = true;

        Main.QueueMainThreadAction(() =>
        {
            if (_selectedStructure == structure)
                RefreshPreview();
        });
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_shouldPollPreview)
            RefreshPreview();
    }

    private void RefreshPreview()
    {
        if (_selectedStructure == null)
            return;

        StructurePreviewResult result = StructurePreviewCache.GetOrRequestPreview(_selectedStructure);

        switch (result.State)
        {
            case StructurePreviewState.Ready:
                _previewImage.SetImage(result.Texture!);
                _previewImage.Color = Color.White;
                _statusText.SetText(string.Empty);
                _shouldPollPreview = false;
                break;

            case StructurePreviewState.Loading:
                _previewImage.SetImage(TextureAssets.MagicPixel);
                _previewImage.Color = Color.Transparent;
                _statusText.SetText("Loading preview...");
                _shouldPollPreview = true;
                break;

            default:
                _previewImage.SetImage(TextureAssets.MagicPixel);
                _previewImage.Color = Color.Transparent;
                _statusText.SetText("No preview available.");
                _shouldPollPreview = false;
                break;
        }
    }
}
