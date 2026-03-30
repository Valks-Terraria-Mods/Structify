using StructureHelper.API;
using Structify.Utils;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Structify.UI.StructuresPage;

public static class StructureSelectionFormatter
{
    private static readonly Dictionary<string, Point16> CachedDimensions = [];

    public static string GetDescriptionText(Structure structure)
    {
        string text = structure.Description ?? string.Empty;

        if (structure.NPCs > 0)
            text += $"\nHas [c/{Colors.SecondaryHex}:{structure.NPCs}] NPC rooms.";

        if (!structure.Procedural && structure.Offset > 0)
            text += $"\nRooted [c/{Colors.SecondaryHex}:{structure.Offset}] tiles beneath the surface.";

        return text;
    }

    public static string GetMetadataText(Structure structure)
    {
        string sizeText = GetSizeText(structure);
        string authorText = Helpers.FormatAuthors(structure.Authors);

        if (string.IsNullOrWhiteSpace(authorText))
            authorText = "Unknown";

        return $"Size: {sizeText}\nCost: {Helpers.FormatPrice(structure.Cost)}\nAuthor: [c/{Colors.SecondaryHex}:{authorText}]";
    }

    private static string GetSizeText(Structure structure)
    {
        if (structure.Procedural || string.IsNullOrWhiteSpace(structure.Schematic))
            return $"[c/{Colors.SecondaryHex}:Variable]";

        if (!CachedDimensions.TryGetValue(structure.Schematic, out Point16 dimensions))
        {
            string path = $"Schematics/{structure.Schematic}.shstruct";
            dimensions = Generator.GetStructureDimensions(path, ModContent.GetInstance<Structify>());
            CachedDimensions[structure.Schematic] = dimensions;
        }

        return $"[c/{Colors.SecondaryHex}:{dimensions.X}] x [c/{Colors.SecondaryHex}:{dimensions.Y}]";
    }
}
