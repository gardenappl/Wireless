
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Wireless.Items
{
	public class CoordinateConfigurator : ModItem
	{
		protected Point16 Coordinates = Point16.NegativeOne;
		
		public override bool CloneNewInstances
		{
			get { return true; }
		}
		
		public override void SetStaticDefaults()
		{

		}
		
		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 30;
			item.useAnimation = 30;
			item.useTime = 30;
			item.UseSound = SoundID.Item1;
			item.useStyle = 1;
			item.value = Item.buyPrice(0, 10);
			item.rare = 5;
			item.mech = true;
			item.material = true;
		}
		
		public override bool CanUseItem(Player player)
		{
			var tileClicked = new Point16(Player.tileTargetX, Player.tileTargetY);
			return WirelessUtils.DoesPlayerReach(player) && (WirelessUtils.IsTransmitter(tileClicked, mod) || WirelessUtils.IsReceiver(tileClicked, mod));
		}
		
		public override bool UseItem(Player player)
		{
			var tileClicked = new Point16(Player.tileTargetX, Player.tileTargetY);
			if(Main.tile[Player.tileTargetX, Player.tileTargetY].type == ModContent.TileType<Tiles.WirelessTransceiver>())
			{
				if(tileClicked == Coordinates)
				{
					Main.NewText(Language.GetTextValue("Mods.Wireless.LinkToItself"), Color.Red);
					return true;
				}
			}
			bool successLink = false;
			if(WirelessUtils.IsTransmitter(tileClicked, mod) && WirelessUtils.IsReceiver(Coordinates, mod))
			{
				ModContent.GetInstance<Wireless>().SyncAddLink(tileClicked, Coordinates);
				successLink = true;
			}
			if(WirelessUtils.IsTransmitter(Coordinates, mod) && WirelessUtils.IsReceiver(tileClicked, mod))
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
				tooltips.Insert(3, new TooltipLine(mod, "LinkingCoord", Language.GetTextValue("Mods.Wireless.StoredCoords", Coordinates)));
		}
		
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(Coordinates.X);
			writer.Write(Coordinates.Y);
		}
		
		public override void NetRecieve(BinaryReader reader)
		{
			Coordinates = new Point16(reader.ReadInt16(), reader.ReadInt16());
		}
		
		public override TagCompound Save()
		{
			return new TagCompound
			{
				{"x", Coordinates.X},
				{"y", Coordinates.Y}
			};
		}
		
		public override void Load(TagCompound tag)
		{
			Coordinates = new Point16(tag.GetShort("x"), tag.GetShort("y"));
		}
	}
}
