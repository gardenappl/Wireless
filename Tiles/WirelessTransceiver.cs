
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Wireless.Tiles
{
	public class WirelessTransceiver : WirelessTransmitter
	{
		public override void MouseOver(int i, int j)
		{
			if(Main.tile[i, j].frameY == 18)
			{
				if(WirelessWorld.Links.ContainsKey(new Point16(i, j)))
				{
					Main.LocalPlayer.showItemIcon = true;
					Main.LocalPlayer.noThrow = 2;
					
					int itemType = Main.LocalPlayer.HeldItem.type;
					if(itemType != mod.ItemType(Names.CoordinateConfigurator) && itemType != mod.ItemType(Names.WirelessRemote))
						Main.LocalPlayer.showItemIcon2 = mod.ItemType(GetType().Name);
				}
				else
				{
					int itemType = Main.LocalPlayer.HeldItem.type;
					if(itemType == mod.ItemType(Names.WirelessRemote) || itemType == mod.ItemType(Names.CoordinateConfigurator))
						Main.LocalPlayer.showItemIcon = true;
				}
			}
		}
	}
}
