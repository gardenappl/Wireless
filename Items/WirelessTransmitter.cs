
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace Wireless.Items
{
	public class WirelessTransmitter : ModItem
	{
		public override void SetStaticDefaults()
		{

		}
		
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.WirelessTransmitter>();
			item.value = Item.buyPrice(0, 2);
			item.rare = 4;
			item.mech = true;
			item.material = true;
		}
	}
}
