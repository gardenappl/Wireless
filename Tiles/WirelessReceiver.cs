
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Wireless.Tiles
{
	public class WirelessReceiver : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
//			TileObjectData.newTile.Height = 2;
//			TileObjectData.newTile.CoordinateWidth = 16;
//			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new []{16, 16};
			TileObjectData.addTile(Type);
		}
		
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 16, 32, ModContent.ItemType<Items.WirelessReceiver>());
		}
		
		public override void MouseOver(int i, int j)
		{
			var player = Main.player[Main.myPlayer];
			
			if((player.inventory[player.selectedItem].type == ModContent.ItemType<Items.CoordinateConfigurator>() ||
				player.inventory[player.selectedItem].type == ModContent.ItemType<Items.WirelessRemote>()) && Main.tile[i, j].frameY == 18)
			{
				player.cursorItemIconEnabled = true;
			}
		}
	}
}
