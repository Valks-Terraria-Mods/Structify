namespace ValksStructures;

[BackgroundColor(0, 0, 0, 100)]
public class Config : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [DefaultValue(false)]
    [BackgroundColor(0, 0, 0, 100)]
    public bool BuildInstantly;

    [DefaultValue(0)]
    [BackgroundColor(0, 0, 0, 100)]
    public int BuildStyle;
}
