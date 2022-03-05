
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Wireless
{
	public static class WirelessUtils
	{
		public static bool DoesPlayerReach(Player player)
		{
			return player.position.X / 16f - (float)Player.tileRangeX - (float)player.inventory[player.selectedItem].tileBoost - (float)player.blockRange <= (float)Player.tileTargetX &&
				(player.position.X + (float)player.width) / 16f + (float)Player.tileRangeX + (float)player.inventory[player.selectedItem].tileBoost - 1f + (float)player.blockRange >= (float)Player.tileTargetX &&
				player.position.Y / 16f - (float)Player.tileRangeY - (float)player.inventory[player.selectedItem].tileBoost - (float)player.blockRange <= (float)Player.tileTargetY &&
				(player.position.Y + (float)player.height) / 16f + (float)Player.tileRangeY + (float)player.inventory[player.selectedItem].tileBoost - 2f + (float)player.blockRange >= (float)Player.tileTargetY;
		}
		
		public static bool IsReceiver(Point16 point)
		{
			if(!WorldGen.InWorld(point.X, point.Y))
				return false;
			
			Tile tile = Main.tile[point.X, point.Y];
			if(tile.HasTile && tile.TileFrameY == 18)
				return (tile.TileType == ModContent.TileType<Tiles.WirelessReceiver>()) || (tile.TileType == ModContent.TileType<Tiles.WirelessTransceiver>());
			return false;
		}
		
		public static bool IsTransmitter(Point16 point)
		{
			if(!WorldGen.InWorld(point.X, point.Y))
				return false;
			
			Tile tile = Main.tile[point.X, point.Y];
			if(tile.HasTile && tile.TileFrameY == 18)
				return (tile.TileType == ModContent.TileType<Tiles.WirelessTransmitter>()) || (tile.TileType == ModContent.TileType<Tiles.WirelessTransceiver>());
			return false;
		}

		public static bool AlreadyExists(Point16 transmitter, Point16 reciever)
        {
			if (WirelessSystem.Links.ContainsKey(transmitter))
				return WirelessSystem.Links[transmitter] == reciever;
			return false;
        }

	}
}
