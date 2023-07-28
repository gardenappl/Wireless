
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Wireless
{
	public class ShopItems : GlobalNPC
	{
		public override void ModifyShop(NPCShop shop)
		{
			switch(shop.NpcType)
			{
				case NPCID.Steampunker:
					shop.Add(ModContent.ItemType<Items.WirelessTransmitter>());
					shop.Add(ModContent.ItemType<Items.WirelessReceiver>());
					shop.Add(ModContent.ItemType<Items.CoordinateConfigurator>());
					break;
			}
		}
	}
}
