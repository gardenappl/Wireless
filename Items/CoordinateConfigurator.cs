
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Creative;

namespace Wireless.Items
{
	public class CoordinateConfigurator : ModItem
	{
		protected Point16 Coordinates = Point16.NegativeOne;
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 30;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.buyPrice(0, 10);
			Item.rare = ItemRarityID.Pink;
			Item.mech = true;
			Item.material = true;
		}
		
		public override bool CanUseItem(Player player)
		{
			var tileClicked = new Point16(Player.tileTargetX, Player.tileTargetY);
			return WirelessUtils.DoesPlayerReach(player) && (WirelessUtils.IsTransmitter(tileClicked) || WirelessUtils.IsReceiver(tileClicked));
		}
		
		public override bool? UseItem(Player player)
		{
			var tileClicked = new Point16(Player.tileTargetX, Player.tileTargetY);
			if(Main.tile[Player.tileTargetX, Player.tileTargetY].TileType == ModContent.TileType<Tiles.WirelessTransceiver>())
			{
				if(tileClicked == Coordinates)
				{
					Main.NewText(Language.GetTextValue("Mods.Wireless.LinkToItself"), Color.Red);
					return true;
				}
			}
			bool successLink = false;
			if(WirelessUtils.IsTransmitter(tileClicked) && WirelessUtils.IsReceiver(Coordinates))
			{
				ModContent.GetInstance<Wireless>().SyncAddLink(tileClicked, Coordinates);
				successLink = true;
			}
			if(WirelessUtils.IsTransmitter(Coordinates) && WirelessUtils.IsReceiver(tileClicked))
			{
				ModContent.GetInstance<Wireless>().SyncAddLink(Coordinates, tileClicked);
				successLink = true;
			}
			if(successLink)
			{
				Coordinates = Point16.NegativeOne;
				Main.NewText(Language.GetTextValue("Mods.Wireless.SuccessLink"), Colors.RarityLime);
			}
			else
			{
				Coordinates = tileClicked;
				Main.NewText(Language.GetTextValue("Mods.Wireless.StartLink"), Colors.RarityYellow);
			}
			return true;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if(Coordinates != Point16.NegativeOne)
				tooltips.Insert(3, new TooltipLine(Mod, "LinkingCoord", Language.GetTextValue("Mods.Wireless.StoredCoords", Coordinates)));
		}
		
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(Coordinates.X);
			writer.Write(Coordinates.Y);
		}
		
		public override void NetReceive(BinaryReader reader)
		{
			Coordinates = new Point16(reader.ReadInt16(), reader.ReadInt16());
		}
		
		public override void SaveData(TagCompound tag)
		{
			tag["x"] = Coordinates.X;
			tag["y"] = Coordinates.Y;

		}
		
		public override void LoadData(TagCompound tag)
		{
			Coordinates = new Point16(tag.GetShort("x"), tag.GetShort("y"));
		}
	}
}
