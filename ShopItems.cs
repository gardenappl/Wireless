
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Wireless
{
	public class ShopItems : GlobalNPC
	{
		public override void SetupShop(int type, Chest shop, ref int nextSlot)
		{
			switch(type)
			{
				case NPCID.Steampunker:
					shop.item[nextSlot].SetDefaults(mod.ItemType(Names.WirelessTransmitter));
					nextSlot++;
					shop.item[nextSlot].SetDefaults(mod.ItemType(Names.WirelessReceiver));
					nextSlot++;
					shop.item[nextSlot].SetDefaults(mod.ItemType(Names.CoordinateConfigurator));
					nextSlot++;
					break;
			}
		}
	}
}
