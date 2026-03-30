using Microsoft.Xna.Framework.Input;
using Structify.Utils;
using Terraria.UI;

namespace Structify.UI;

public class CustomShopSystem : ModSystem
{
    private UserInterface _interface;
    private StructureCatalogUI _ui;
    private ModKeybind _toggleKey;
    private ModKeybind _undoKey;
    private bool _visible;
    private GameTime _lastTime;

    public void Hide()
    {
        _visible = false;
    }

    public override void Load()
    {
        if (Main.dedServ) return;
        _toggleKey = KeybindLoader.RegisterKeybind(Mod, "Structure Catalog", "Y");
        _undoKey = KeybindLoader.RegisterKeybind(Mod, "Undo Last Placement", "U");
        _ui = new StructureCatalogUI();
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

        if (!Main.gameMenu && _undoKey.JustPressed)
        {
            bool undone = StructureUtils.UndoLastPlacement();
            Main.NewText(undone ? "Undid last structure placement." : "No recent structure placement to undo.", undone ? Color.LightGreen : Color.Gray);
        }
        
        if (_visible && Main.keyState.IsKeyDown(Keys.Escape) && Main.oldKeyState.IsKeyUp(Keys.Escape))
        {
            Hide();
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