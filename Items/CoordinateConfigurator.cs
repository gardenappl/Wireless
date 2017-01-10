
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Wireless.Items
{
	public class CoordinateConfigurator : ModItem
	{
		protected Point16 coord = Point16.NegativeOne;
		
		public override bool CloneNewInstances
		{
			get { return true; }
		}
		
		public override void SetDefaults()
		{
			item.name = "Coordinate Configurator";
			item.width = 14;
			item.height = 30;
			AddTooltip("Links Wireless Transmitters to Wireless Receivers");
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
			return DoesPlayerReach(player) && (IsTransmitter(tileClicked) || IsReceiver(tileClicked));
		}
		
		public override bool UseItem(Player player)
		{
			var tileClicked = new Point16(Player.tileTargetX, Player.tileTargetY);
			if(IsTransmitter(tileClicked) && IsReceiver(coord))
			{
				(mod as Wireless).SyncAddLink(tileClicked, coord);
				coord = Point16.NegativeOne;
				Main.NewText("Linked successfully!", Color.Green.R, Color.Green.G, Color.Green.B);
				return true;
			}
			if(IsTransmitter(coord) && IsReceiver(tileClicked))
			{
				(mod as Wireless).SyncAddLink(coord, tileClicked);
				coord = Point16.NegativeOne;
				Main.NewText("Linked successfully!", Color.Green.R, Color.Green.G, Color.Green.B);
				return true;
			}
			coord = tileClicked;
			Main.NewText("Starting link...", Color.YellowGreen.R, Color.YellowGreen.G, Color.YellowGreen.B);
			return true;
		}
		
		public bool IsReceiver(Point16 point)
		{
			if(!WorldGen.InWorld(point.X, point.Y))
				return false;
			var tile = Main.tile[point.X, point.Y];
			return tile.active() && tile.type == mod.TileType(Names.WirelessReceiver) && tile.frameY == 18;
		}
		
		public bool IsTransmitter(Point16 point)
		{
			if(!WorldGen.InWorld(point.X, point.Y))
				return false;
			var tile = Main.tile[point.X, point.Y];
			return tile.active() && tile.type == mod.TileType(Names.WirelessTransmitter) && tile.frameY == 18;
		}
		
		public bool DoesPlayerReach(Player player)
		{
			return player.position.X / 16f - (float)Player.tileRangeX - (float)player.inventory[player.selectedItem].tileBoost - (float)player.blockRange <= (float)Player.tileTargetX &&
				(player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)player.inventory[player.selectedItem].tileBoost - 1f + (float)player.blockRange >= (float)Player.tileTargetX &&
				player.position.Y / 16f - (float)Player.tileRangeY - (float)player.inventory[player.selectedItem].tileBoost - (float)player.blockRange <= (float)Player.tileTargetY &&
				(player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)player.inventory[player.selectedItem].tileBoost - 2f + (float)player.blockRange >= (float)Player.tileTargetY;
		}
		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if(coord.X > 0 && coord.Y > 0)
			{
				tooltips.Insert(2, new TooltipLine(mod, "Linking", "Currently linking..."));
				tooltips.Insert(3, new TooltipLine(mod, "LinkingCoord", "Stored coordinates: " + coord));
			}
		}
		
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(coord.X);
			writer.Write(coord.Y);
		}
		
		public override void NetRecieve(BinaryReader reader)
		{
			coord = new Point16(reader.ReadInt16(), reader.ReadInt16());
		}
		
		public override TagCompound Save()
		{
			return new TagCompound
			{
				{"x", coord.X},
				{"y", coord.Y}
			};
		}
		
		public override void Load(TagCompound tag)
		{
			coord = new Point16(tag.GetShort("x"), tag.GetShort("y"));
		}
	}
}
