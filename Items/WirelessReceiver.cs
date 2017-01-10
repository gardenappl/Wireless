
using System;
using Terraria;
using Terraria.ModLoader;

namespace Wireless.Items
{
	public class WirelessReceiver : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Wireless Receiver";
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			AddTooltip("Receives signals from linked Wireless Transmitters and Remotes");
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType(Names.WirelessReceiver);
			item.value = Item.buyPrice(0, 2);
			item.rare = 4;
			item.mech = true;
		}
	}
}
