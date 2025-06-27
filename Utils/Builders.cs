namespace Structify.Utils;

public static class Builders
{
    public const string Valkyrienyanko = "Valkyrienyanko";
    public const string Burlierbuffalo1 = "Burlierbuffalo1";
    public const string Expination = "Expination";
    public const string Name36154 = "Name36154";
    public const string ColinFour = "ColinFour";
    public const string Grim = "Grim";
    public const string Toast = "Toast";
    
    public static string GetCurrentBuilders()
    {
        string[] builderNames =
        [
            // Valkyrienyanko, // :P
            Expination,
            Grim,
            Toast,
        ];
        
        return string.Join(", ", builderNames);
    }

    public static string GetPreviousBuilders()
    {
        string[] builderNames =
        [
            Burlierbuffalo1,
            Name36154,
            ColinFour,
        ];

        return string.Join(", ", builderNames);
    }
}