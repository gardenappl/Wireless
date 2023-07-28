
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Wireless.Tiles
{
	public class WirelessTransceiver : WirelessTransmitter
	{
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			if (WirelessSystem.Links.ContainsKey(new Point16(i, j + 1)))
			{
				ModContent.GetInstance<Wireless>().SyncRemoveLink(new Point16(i, j + 1));
			}
		}

		public override void MouseOver(int i, int j)
		{
			if(Main.tile[i, j].TileFrameY == 18)
			{
				if(WirelessSystem.Links.ContainsKey(new Point16(i, j)))
				{
					Main.LocalPlayer.cursorItemIconEnabled = true;
					Main.LocalPlayer.noThrow = 2;
					
					int itemType = Main.LocalPlayer.HeldItem.type;
					if(itemType != ModContent.ItemType<Items.WirelessRemote>() && itemType != ModContent.ItemType<Items.CoordinateConfigurator>())
						Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<Items.WirelessTransceiver>();
				}
				else
				{
					int itemType = Main.LocalPlayer.HeldItem.type;
					if(itemType == ModContent.ItemType<Items.WirelessRemote>() || itemType == ModContent.ItemType<Items.CoordinateConfigurator>())
						Main.LocalPlayer.cursorItemIconEnabled = true;
				}
			}
		}
	}
}
