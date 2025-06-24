namespace Structify.UI;

public class Structure
{
    public string Schematic { get; init; }
    public string DisplayName { get; init; }
    public string Description { get; init; }
    public int Cost { get; init; }
    public int Offset { get; init; }
    public bool Procedural { get; init; }
    public string[] Authors { get; init; }
}
