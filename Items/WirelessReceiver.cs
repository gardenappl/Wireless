
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Wireless.Items
{
	public class WirelessReceiver : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Receives signals from linked Wireless Transmitters and Remotes");
			DisplayName.AddTranslation(GameCulture.Russian, "Беспроводной приёмник");
			Tooltip.AddTranslation(GameCulture.Russian, "Принимает сигналы от беспроводных передатчиков и дистанционных пультов");
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
			item.createTile = mod.TileType(GetType().Name);
			item.value = Item.buyPrice(0, 2);
			item.rare = 4;
			item.mech = true;
		}
	}
}
