using System.Text;
using Structify.UI;
using StructureHelper.API;

namespace Structify.Utils;

public static class Helpers
{
    /// <summary>
    /// Wraps each character of <paramref name="text"/> in a [c/rrggbb:char] tag,
    /// interpolating from <paramref name="start"/> to <paramref name="end"/>.
    /// </summary>
    public static string GradientText(string text, Color start, Color end)
    {
        if (string.IsNullOrEmpty(text))
            return "";

        StringBuilder sb = new();
        int len = text.Length;
        for (int i = 0; i < len; i++)
        {
            float t = i / (float)(len - 1);
            Color c = new Color(
                (byte)MathHelper.Lerp(start.R, end.R, t),
                (byte)MathHelper.Lerp(start.G, end.G, t),
                (byte)MathHelper.Lerp(start.B, end.B, t)
            );
            // format as RRBBGG
            string hex = $"{c.R:X2}{c.G:X2}{c.B:X2}";
            sb.Append($"[c/{hex}:{text[i]}]");
        }
        return sb.ToString();
    }
    
    public static int GetPlayerCoinCount()
    {
        Player player = Main.LocalPlayer;
        
        int playerCoins = 0;
        playerCoins += player.CountItem(ItemID.PlatinumCoin) * 1_000_000;
        playerCoins += player.CountItem(ItemID.GoldCoin)     * 10_000;
        playerCoins += player.CountItem(ItemID.SilverCoin)   * 100;
        playerCoins += player.CountItem(ItemID.CopperCoin);

        return playerCoins;
    }
    
    /// <summary>
    /// Formats the author string into a nice readable format.
    /// </summary>
    public static string FormatAuthors(IEnumerable<string> authors)
    {
        List<string> authorList = authors.ToList();

        switch (authorList.Count)
        {
            case 0:
                return string.Empty;
            case 1:
                // "A"
                return authorList[0];
            case 2:
                // "A and B"
                return $"{authorList[0]} and {authorList[1]}";
        }

        // For 3 or more authors: "A, B and C"
        IEnumerable<string> allButLast = authorList.Take(authorList.Count - 1);
        string lastAuthor = authorList.Last();

        return $"{string.Join(", ", allButLast)} and {lastAuthor}";
    }
    
    public static string FormatPrice(int price) {
        int platinum = price / 1_000_000;
        int gold     = (price /   10_000) % 100;
        int silver   = (price /      100) % 100;
        int copper   = price % 100;
        
        List<string> parts = [];
        if (platinum > 0) parts.Add($"{platinum} platinum");
        if (gold     > 0) parts.Add($"{gold} gold");
        if (silver   > 0) parts.Add($"{silver} silver");
        if (copper   > 0) parts.Add($"{copper} copper");

        if (parts.Count == 0)
            return $"[c/969696:No value]"; // Dark Gray (aka Color.Gray)

        string combined = string.Join(" ", parts);
        
        string hex;
        if (platinum > 0) hex = "E5E4E2";   // Platinum
        else if (gold > 0) hex = "FFD700";   // Gold
        else if (silver > 0) hex = "C0C0C0"; // Silver
        else               hex = "B87333";   // Copper
        
        return $"[c/{hex}:{combined}]";
    }
    
    public static string GetInfo(Structure structure)
    {
        string info = $"{structure.Description}";
        
        if (structure.NPCs > 0)
            info += $"\nHas [c/{StructureCatalogUI.SecondaryColorHex}:{structure.NPCs}] NPC rooms.";

        info += "\n";
        
        if (!structure.Procedural && structure.Offset > 0)
        {
            info += $"\nRooted [c/{StructureCatalogUI.SecondaryColorHex}:{structure.Offset}] tiles beneath the surface.";
        }

        if (!structure.Procedural)
        {
            string path = $"Schematics/{structure.Schematic}.shstruct";
            Point16 dimensions = Generator.GetStructureDimensions(path, ModContent.GetInstance<Structify>());
            info += $"\nSize: [c/{StructureCatalogUI.SecondaryColorHex}:{dimensions.X}] x [c/{StructureCatalogUI.SecondaryColorHex}:{dimensions.Y}]";
        }

        info += $"\nCost: {FormatPrice(structure.Cost)}";

        info += $"\n\nBuilt by [c/{StructureCatalogUI.SecondaryColorHex}:{FormatAuthors(structure.Authors)}].";
        
        return info;
    }
}