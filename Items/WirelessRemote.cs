
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Wireless.Items
{
	public class WirelessRemote : CoordinateConfigurator
	{
		public override void SetDefaults()
		{
			item.name = "Wireless Remote";
			item.width = 14;
			item.height = 30;
			AddTooltip("Sends signals to linked Wireless Receivers");
			item.useAnimation = 30;
			item.useTime = 30;
			item.UseSound = new LegacySoundStyle(SoundID.Mech, 0);
			item.useStyle = 4;
			item.value = Item.sellPrice(0, 2);
			item.rare = 5;
			item.mech = true;
		}
		
		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.AddIngredient(mod.ItemType(Names.CoordinateConfigurator));
			recipe.AddIngredient(mod.ItemType(Names.WirelessTransmitter));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		public override bool CanUseItem(Player player)
		{
			if(DoesPlayerReach(player) && IsReceiver(new Point16(Player.tileTargetX, Player.tileTargetY)))
			{
				item.UseSound = SoundID.Item1;
				item.useStyle = 1;
				return true;
			}
			if(IsReceiver(coord))
			{
				item.UseSound = new LegacySoundStyle(SoundID.Mech, 0);
				item.useStyle = 4;
				return true;
			}
			return false;
		}
		
		public override bool UseItem(Player player)
		{
			var tileClicked = new Point16(Player.tileTargetX, Player.tileTargetY);
			if(IsReceiver(tileClicked))
			{
				coord = tileClicked;
				Main.NewText("Linked successfully!", Color.Green.R, Color.Green.G, Color.Green.B);
				return true;
			}
			if(IsReceiver(coord))
			{
				(mod as Wireless).SyncActivate(coord);
				return true;
			}
			return false;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if(coord.X > 0 && coord.Y > 0)
			{
				tooltips.Insert(2, new TooltipLine(mod, "LinkingCoord", "Stored coordinates: " + coord));
			}
		}
	}
}
