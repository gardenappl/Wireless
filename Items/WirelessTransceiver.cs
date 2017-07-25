
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Wireless.Items
{
	public class WirelessTransceiver : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.AddTranslation(GameCulture.Russian, "Беспроводной приёмопередатчик");
			Tooltip.SetDefault("Sends and receives signals from linked devices");
			Tooltip.AddTranslation(GameCulture.Russian, "Посылает и принимает сигналы от связанных устройств");
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
			item.value = Item.buyPrice(0, 4);
			item.rare = 4;
			item.mech = true;
		}
		
		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, Names.WirelessTransmitter);
			recipe.AddIngredient(mod, Names.WirelessReceiver);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
