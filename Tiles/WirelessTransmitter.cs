﻿
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Audio;

namespace Wireless.Tiles
{
	public class WirelessTransmitter : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new []{16, 16};
			TileObjectData.addTile(Type);
		}
		
		public override void HitWire(int i, int j)
		{
			if(WirelessSystem.Links.ContainsKey(new Point16(i, j)))
			{
				var coord = WirelessSystem.Links[new Point16(i, j)];
				if(WirelessUtils.IsReceiver(coord))
				{
					//Code below is copied and adapted from Wiring.HitWire()
					for (int side = 0; side < 4; side++)
					{
						int nextI;
						int nextJ;
						switch (side)
						{
							case 0:
								nextI = coord.X;
								nextJ = coord.Y + 1;
								break;
							case 1:
								nextI = coord.X;
								nextJ = coord.Y - 1;
								break;
							case 2:
								nextI = coord.X + 1;
								nextJ = coord.Y;
								break;
							case 3:
								nextI = coord.X - 1;
								nextJ = coord.Y;
								break;
							default:
								nextI = coord.X;
								nextJ = coord.Y + 1;
								break;
						}
						if (nextI >= 2 && nextI < Main.maxTilesX - 2 && nextJ >= 2 && nextJ < Main.maxTilesY - 2)
						{
							var tile = Main.tile[nextI, nextJ];
							if (tile != null)
							{
								var receiverTile = Main.tile[coord.X, coord.Y];
								if (receiverTile != null)
								{
									byte b = 4;
									if (tile.type == TileID.WirePipe || tile.type == TileID.PixelBox)
									{
										b = 0;
									}
//										if (receiverTile.type == TileID.WirePipe)
//										{
//											switch (receiverTile.frameX / 18)
//											{
//												case 0:
//													if (side != wireDirection)
//													{
//														goto IL_315;
//													}
//													break;
//												case 1:
//													if ((wireDirection != 0 || side != 3) && (wireDirection != 3 || side != 0) && (wireDirection != 1 || side != 2))
//													{
//														if (wireDirection != 2)
//														{
//															goto IL_315;
//														}
//														if (side != 1)
//														{
//															goto IL_315;
//														}
//													}
//													break;
//												case 2:
//													if ((wireDirection != 0 || side != 2) && (wireDirection != 2 || side != 0) && (wireDirection != 1 || side != 3) && (wireDirection != 3 || side != 1))
//													{
//														goto IL_315;
//													}
//													break;
//											}
//										}
//										if (receiverTile.type == TileID.PixelBox)
//										{
//											if (side != wireDirection)
//											{
//												goto IL_315;
//											}
//											if (Wiring._PixelBoxTriggers.ContainsKey(point2))
//											{
//												Dictionary<Point16, byte> pixelBoxTriggers;
//												Point16 key;
//												(pixelBoxTriggers = Wiring._PixelBoxTriggers)[key = point2] = (pixelBoxTriggers[key] | ((side == 0 | side == 1) ? 2 : 1));
//											}
//											else
//											{
//												Wiring._PixelBoxTriggers[point2] = ((side == 0 | side == 1) ? 2 : 1);
//											}
//										}
									bool flag;
									switch (Wiring._currentWireColor)
									{
										case 1:
											flag = (tile.sTileHeader & 128) == 128; // Tile.Wire()
											break;
										case 2:
											flag = (tile.sTileHeader & 256) == 256;
											break;
										case 3:
											flag = (tile.sTileHeader & 512) == 512;
											break;
										case 4:
											flag = (tile.bTileHeader & 128) == 128;
											break;
										default:
											flag = false;
											break;
									}
									if (flag)
									{
										var point3 = new Point16(nextI, nextJ);
										byte b2;
										if (Wiring._toProcess.TryGetValue(point3, out b2))
										{
											b2 -= 1;
											if (b2 == 0)
											{
												Wiring._toProcess.Remove(point3);
											}
											else
											{
												Wiring._toProcess[point3] = b2;
											}
										}
										else
										{
											Wiring._wireList.PushBack(point3);
											Wiring._wireDirectionList.PushBack((byte)side);
											if (b > 0)
											{
												Wiring._toProcess.Add(point3, b);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 16, 32, ModContent.ItemType<Items.WirelessTransmitter>());
			if(WirelessSystem.Links.ContainsKey(new Point16(i, j + 1)))
			{
				ModContent.GetInstance<Wireless>().SyncRemoveLink(new Point16(i, j + 1));
			}
		}
		
		public override bool RightClick(int i, int j)
		{
            if (Main.tile[i, j].frameY == 18 && WirelessSystem.Links.ContainsKey(new Point16(i, j)))
            {
                var coord = WirelessSystem.Links[new Point16(i, j)];
				//				Wiring.TripWire(i, j, 1, 1);
				SoundEngine.PlaySound(28, i * 16, j * 16, 0);
                ModContent.GetInstance<Wireless>().SyncActivate(coord);
                return true;

            }
            else
            {
                return false;
            }
		}
		
		public override void MouseOver(int i, int j)
		{
			var player = Main.player[Main.myPlayer];
			
			if(Main.tile[i, j].frameY == 18)
			{
				if(WirelessSystem.Links.ContainsKey(new Point16(i, j)))
				{
					player.cursorItemIconEnabled = true;
					player.noThrow = 2;
					if(player.inventory[player.selectedItem].type != ModContent.ItemType<Items.CoordinateConfigurator>())
					{
						player.cursorItemIconID = ModContent.ItemType<Items.WirelessTransmitter>();
					}
				}
				else if(player.inventory[player.selectedItem].type == ModContent.ItemType<Items.CoordinateConfigurator>())
				{
					player.cursorItemIconEnabled = true;
				}
			}
		}
	}
}
