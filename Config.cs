namespace ValksStructures;

[BackgroundColor(0, 0, 0, 100)]
public class Config : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [DefaultValue(false)]
    [BackgroundColor(0, 0, 0, 100)]
    public bool BuildInstantly;

    [DefaultValue(1)]
    [BackgroundColor(0, 0, 0, 100)]
    [Range(1, 100)]
    public int BuildTickRate;

    public override void OnChanged()
    {
        GameQueue.BuildTickRate = BuildTickRate;
    }
}
