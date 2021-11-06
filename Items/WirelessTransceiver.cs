
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

			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.WirelessTransceiver>();
			Item.value = Item.buyPrice(0, 4);
			Item.rare = ItemRarityID.LightRed;
			Item.mech = true;
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
					.AddTile(TileID.TinkerersWorkbench)
					.AddIngredient(ModContent.ItemType<WirelessTransmitter>(), 1)
					.AddIngredient(ModContent.ItemType<WirelessReceiver>(), 1)
					.Register();
		}
	}
}
