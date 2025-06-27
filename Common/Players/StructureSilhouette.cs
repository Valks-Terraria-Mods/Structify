using Structify.Content.Items;
using Structify.UI;
using Structify.Utils;
using StructureHelper.API;

namespace Structify.Common.Players;

public class StructureSilhouette : ModPlayer
{
    private bool _drawOutline;
    private Point16 _dimensions;
    private Structure _structure;
    
    private bool _previousMouseLeft;
    private bool _previousMouseRight;
    
    public void StartDrawingOutline(Structure structure)
    {
        if (structure.Procedural)
        {
            _dimensions = new Point16(1, 1);
        }
        else
        {
            string path = $"Schematics/{structure.Schematic}.shstruct";
            _dimensions = Generator.GetStructureDimensions(path, Mod);
        }
        
        _structure = structure;
        _drawOutline = true;
    }

    public override void PostUpdate()
    {
        if (!_drawOutline)
            return;

        Point16 mPos = Main.MouseWorld.ToTileCoordinates16();
        DrawDust(mPos);
        HandleClicks(mPos);
    }

    private void OnLeftClick(Point16 mPos)
    {
        _drawOutline = false;

        if (_structure.Procedural)
        {
            Hellavator.Build(Mod, Main.player[Main.myPlayer], mPos);
            return;
        }

        if (_structure.Schematic == "PylonUniversal")
        {
            Player player = Main.LocalPlayer;

            string biomeSuffix = BiomeConditions.FirstOrDefault(kvp => kvp.Value(player)).Key ?? "Universal";
            string schematic = "Pylon" + biomeSuffix;
            
            StructureUtils.Generate(schematic, _structure.Offset, Mod, mPos);
            return;
        }

        StructureUtils.Generate(_structure.Schematic, _structure.Offset, Mod, mPos);
    }
    
    private static readonly Dictionary<string, Func<Player, bool>> BiomeConditions = new()
    {
        ["Cavern"]    = p => p.ZoneNormalCaverns,
        ["Desert"]    = p => p.ZoneDesert || p.ZoneUndergroundDesert,
        ["Forest"]    = p => p.ZoneForest,
        ["Hallowed"]  = p => p.ZoneHallow,
        ["Jungle"]    = p => p.ZoneJungle,
        ["Mushroom"]  = p => p.ZoneGlowshroom,
        ["Ocean"]     = p => p.ZoneBeach,
        ["Snow"]      = p => p.ZoneSnow,
    };

    private void OnRightClick()
    {
        _drawOutline = false;
        
        // Refund player
        GivePlayerCoins(Main.LocalPlayer, _structure.Cost);
    }
    
    private static void GivePlayerCoins(Player player, int totalCopper)
    {
        int platinum = totalCopper / 1_000_000;
        int gold     = (totalCopper /    10_000) % 100;
        int silver   = (totalCopper /       100) % 100;
        int copper   = totalCopper % 100;
        
        EntitySource_Misc source = new("CoinHelper.GivePlayerCoins");
        
        if (platinum > 0)
            player.QuickSpawnItem(source, ItemID.PlatinumCoin, platinum);
        if (gold > 0)
            player.QuickSpawnItem(source, ItemID.GoldCoin, gold);
        if (silver > 0)
            player.QuickSpawnItem(source, ItemID.SilverCoin, silver);
        if (copper > 0)
            player.QuickSpawnItem(source, ItemID.CopperCoin, copper);
    }

    private void HandleClicks(Point16 mPos)
    {
        bool currentMouseLeft = Main.mouseLeft;
        bool currentMouseRight = Main.mouseRight;
        
        if (currentMouseLeft && !_previousMouseLeft)
        {
            OnLeftClick(mPos);
        }

        if (currentMouseRight && !_previousMouseRight)
        {
            OnRightClick();
        }

        _previousMouseLeft = currentMouseLeft;
        _previousMouseRight = currentMouseRight;
    }

    private void DrawDust(Point16 mPos)
    {
        Point16 bottomLeftAnchor = StructureUtils.GetOrigin(_structure.Offset, _dimensions, mPos);
        
        // Draw ground level
        for (int x = 1; x <= _dimensions.X; x++)
        {
            Vector2 pos = new((bottomLeftAnchor.X + x) * 16 - 3, (bottomLeftAnchor.Y + _dimensions.Y - _structure.Offset - 1) * 16 - 3);
            
            int dustId = Dust.NewDust(pos, 1, 1, DustID.GreenTorch);
        
            Main.dust[dustId].noGravity = true;
            Main.dust[dustId].scale = 1f;
            Main.dust[dustId].velocity *= 0.2f;
            Main.dust[dustId].noLight = true;
        }

        // Draw border
        for (int x = 0; x <= _dimensions.X; x++)
        {
            for (int y = 0; y <= _dimensions.Y; y++)
            {
                // Skip inner tiles — only draw border
                if (x != 0 && x != _dimensions.X && y != 0 && y != _dimensions.Y)
                    continue;
                
                Vector2 pos = new((bottomLeftAnchor.X + x) * 16 - 3, (bottomLeftAnchor.Y + y) * 16 - 3);
                
                int dustId = Dust.NewDust(pos, 1, 1, DustID.UltraBrightTorch);
        
                Main.dust[dustId].noGravity = true;
                Main.dust[dustId].scale = 0.6f;
                Main.dust[dustId].velocity *= 0.2f;
            }
        }
    }
}
