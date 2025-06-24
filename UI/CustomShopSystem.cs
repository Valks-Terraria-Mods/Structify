using Terraria.UI;

namespace Structify.UI;

public class CustomShopSystem : ModSystem
{
    private UserInterface _interface;
    private CustomShopUI _ui;
    private ModKeybind _toggleKey;
    private bool _visible;
    private GameTime _lastTime;

    public void Hide()
    {
        _visible = false;
    }

    public override void Load()
    {
        if (Main.dedServ) return;
        _toggleKey = KeybindLoader.RegisterKeybind(Mod, "Toggle Shop UI", "Y");
        _ui = new CustomShopUI();
        _interface = new UserInterface();
        _ui.Activate();
    }

    public override void UpdateUI(GameTime gameTime)
    {
        _lastTime = gameTime;
        
        if (_toggleKey.JustPressed)
        {
            _visible = !_visible;
            _interface.SetState(_visible ? _ui : null);
        }
        
        if (_visible)
            _interface.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int idx = layers.FindIndex(l => l.Name == "Vanilla: Mouse Text");
        if (idx != -1)
        {
            layers.Insert(idx, new LegacyGameInterfaceLayer(
                "Structify: Shop",
                () =>
                {
                    if (_visible)
                        _interface.Draw(Main.spriteBatch, _lastTime);
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}