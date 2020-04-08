
using System;
using Terraria;

using Terraria.ID;
using Terraria.ModLoader;

namespace Wireless.Items
{
	public class WirelessTransceiver : ModItem
	{
		
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
			item.createTile = ModContent.TileType<Tiles.WirelessTransceiver>();
			item.value = Item.buyPrice(0, 4);
			item.rare = 4;
			item.mech = true;
		}
		
		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<WirelessTransmitter>());
			recipe.AddIngredient(ModContent.ItemType<WirelessReceiver>());
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
