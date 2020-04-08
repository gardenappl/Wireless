
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
					if(itemType != ModContent.ItemType<Items.WirelessRemote>() && itemType != ModContent.ItemType<Items.CoordinateConfigurator>())
						Main.LocalPlayer.showItemIcon2 = mod.ItemType(GetType().Name);
				}
				else
				{
					int itemType = Main.LocalPlayer.HeldItem.type;
					if(itemType == ModContent.ItemType<Items.WirelessRemote>() || itemType == ModContent.ItemType<Items.CoordinateConfigurator>())
						Main.LocalPlayer.showItemIcon = true;
				}
			}
		}
	}
}
