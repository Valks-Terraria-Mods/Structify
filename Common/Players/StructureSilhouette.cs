using Structify.Common.Items;
using Structify.Utils;
using StructureHelper.API;

namespace Structify.Common.Players;

public class StructureSilhouette : ModPlayer
{
    private bool _holdingStructureItem;
    private Point16 _dimensions;
    private int _lastHeldItemType = -1;
    private SchematicItem _heldItem;
    
    public override void PreUpdate()
    {
        if (Player.HeldItem.type == _lastHeldItemType) 
            return;
        
        OnHeldItemChanged(Player.HeldItem);
        _lastHeldItemType = Player.HeldItem.type;
    }
    
    private void OnHeldItemChanged(Item newItem)
    {
        if (newItem.ModItem is SchematicItem item)
        {
            string path = $"Schematics/{item.SchematicName}.shstruct";
            
            _holdingStructureItem = true;
            _heldItem = item;
            _dimensions = Generator.GetStructureDimensions(path, Mod);
        }
        else
        {
            _holdingStructureItem = false;
        }
    }

    public override void PostUpdate()
    {
        if (!_holdingStructureItem)
            return;

        Point16 mPos = Main.MouseWorld.ToTileCoordinates16();
        Point16 bottomLeftAnchor = StructureUtils.GetOrigin(_heldItem, _dimensions, mPos);
        
        // Draw ground level
        for (int x = 1; x <= _dimensions.X; x++)
        {
            Vector2 pos = new((bottomLeftAnchor.X + x) * 16 - 3, (bottomLeftAnchor.Y + _dimensions.Y - _heldItem.VerticalOffset - 1) * 16 - 3);
            
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
