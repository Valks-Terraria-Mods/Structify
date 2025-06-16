using Structify.Common.Items;
using StructureHelper.API;

namespace Structify.Common.Players;

public class StructureSilhouette : ModPlayer
{
    private bool _holdingStructureItem;
    private Point16 _dimensions;
    private int _lastHeldItemType = -1;
    
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
        
        Point16 mPos = new((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16);
        Point16 bottomLeftAnchor = mPos - new Point16(0, _dimensions.Y - 1);

        for (int x = 0; x < _dimensions.X; x++)
        {
            for (int y = 0; y < _dimensions.Y; y++)
            {
                // Skip inner tiles — only draw border
                if (x != 0 && x != _dimensions.X - 1 && y != 0 && y != _dimensions.Y - 1)
                    continue;
                
                Vector2 pos = new((bottomLeftAnchor.X + x) * 16 + 8 - 3, (bottomLeftAnchor.Y + y) * 16 + 8 - 3);
                
                int dustId = Dust.NewDust(pos, 1, 1, DustID.UltraBrightTorch);
        
                Main.dust[dustId].noGravity = true;
                Main.dust[dustId].scale = 0.6f;
                Main.dust[dustId].velocity *= 0.2f;
            }
        }
    }
}
