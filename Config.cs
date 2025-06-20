namespace Structify;

public class Config : ModConfig
{
    public static Config Instance { get; private set; }

    public override ConfigScope Mode => ConfigScope.ServerSide;
    public override void OnLoaded()
    {
        Instance = this;
    }
    
    [DefaultValue(false)]
    public bool BossArenaIgnoresTiles { get; set; }
}
