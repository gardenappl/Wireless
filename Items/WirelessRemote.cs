
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Wireless.Items
{
	public class WirelessRemote : CoordinateConfigurator
	{
		
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 30;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.UseSound = new LegacySoundStyle(SoundID.Mech, 0);
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = Item.sellPrice(0, 2);
			Item.rare = ItemRarityID.Pink;
			Item.mech = true;
		}
		
		public override void AddRecipes()
		{
				CreateRecipe()
					.AddTile(TileID.TinkerersWorkbench)
					.AddIngredient(ModContent.ItemType<WirelessTransmitter>(), 1)
					.AddIngredient(ModContent.ItemType<CoordinateConfigurator>(), 1)
					.Register();
		}
		
		public override bool CanUseItem(Player player)
		{
			if(WirelessUtils.DoesPlayerReach(player) && WirelessUtils.IsReceiver(new Point16(Player.tileTargetX, Player.tileTargetY)))
			{
				Item.UseSound = SoundID.Item1;
				Item.useStyle = ItemUseStyleID.Swing;
				return true;
			}
			if(Coordinates != Point16.NegativeOne)
			{
				Item.UseSound = new LegacySoundStyle(SoundID.Mech, 0);
				Item.useStyle = ItemUseStyleID.HoldUp;
				return true;
			}
			return false;
		}
		
		public override bool? UseItem(Player player)
		{
			var tileClicked = new Point16(Player.tileTargetX, Player.tileTargetY);
			if(WirelessUtils.IsReceiver(tileClicked))
			{
				Coordinates = tileClicked;
				Main.NewText(Language.GetTextValue("Mods.Wireless.SuccessLink"), Colors.RarityLime);
				return true;
			}
			if(WirelessUtils.IsReceiver(Coordinates))
			{
				ModContent.GetInstance<Wireless>().SyncActivate(Coordinates);
				return true;
			}
			return false;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if(Coordinates != Point16.NegativeOne)
				tooltips.Insert(2, new TooltipLine(Mod, "LinkingCoord", Language.GetTextValue("Mods.Wireless.StoredCoords", Coordinates)));
		}
	}
}
