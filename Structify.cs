global using Microsoft.Xna.Framework;
global using Terraria;
global using Terraria.ID;
global using Terraria.DataStructures;
global using Terraria.ModLoader;
global using Terraria.ObjectData;
global using Terraria.ModLoader.Config;
global using System;
global using System.Collections.Generic;
global using System.ComponentModel;
global using System.Linq;

using Structify.Content.Items;
using System.IO;
using Terraria.Localization;
using Terraria.Chat;

namespace Structify;

public class Structify : Mod
{
    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        MessageType msgType = (MessageType)reader.ReadByte();

        switch (msgType)
        {
            case MessageType.SpawnHellavator:
                short x = reader.ReadInt16();
                short y = reader.ReadInt16();

                Point16 mPos = new(x, y);

                // Only the server should build the hellavator
                if (Main.netMode == NetmodeID.Server)
                {
                    Hellavator.BuildHellavator(mPos);

                    // Now broadcast updated tiles back to all clients in slices
                    int xStart = mPos.X - 4;
                    int width = 8;
                    int worldHeight = Main.maxTilesY;
                    for (int tileY = mPos.Y; tileY < worldHeight; tileY += 100)
                    {
                        int slice = Math.Min(100, worldHeight - tileY);
                        NetMessage.SendTileSquare(
                            whoAmi: -1,      // -1 = broadcast to everyone
                            tileX: xStart,
                            tileY: tileY,
                            xSize: width,
                            ySize: slice
                        );
                    }
                }
                
                break;
        }
    }
}