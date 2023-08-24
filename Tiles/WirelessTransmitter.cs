
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Audio;
using Microsoft.Xna.Framework;

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
			if (WirelessSystem.Links.ContainsKey(new Point16(i, j)))
			{
				var coord = WirelessSystem.Links[new Point16(i, j)];
				if (WirelessUtils.IsReceiver(coord))
				{
					//Code below is copied and adapted from Wiring.HitWire()
					for (int direction = 0; direction < 4; direction++)
					{
						int CurrentX;
						int CurrentY;
						switch (direction)
						{
							case 0:
								CurrentX = coord.X;
								CurrentY = coord.Y + 1;
								break;
							case 1:
								CurrentX = coord.X;
								CurrentY = coord.Y - 1;
								break;
							case 2:
								CurrentX = coord.X + 1;
								CurrentY = coord.Y;
								break;
							case 3:
								CurrentX = coord.X - 1;
								CurrentY = coord.Y;
								break;
							default:
								CurrentX = coord.X;
								CurrentY = coord.Y + 1;
								break;
						}
						if (CurrentX >= 2 && CurrentX < Main.maxTilesX - 2 && CurrentY >= 2 && CurrentY < Main.maxTilesY - 2)
						{
							Tile CurrentWire = Main.tile[CurrentX, CurrentY];
							if (CurrentWire.HasTile)
							{
								Tile RecieverTile = Main.tile[coord.X, coord.Y];
								if (RecieverTile.HasTile)
								{
									byte flags = 3;
									if (CurrentWire.TileType == TileID.WirePipe)
									{
										flags = 0;
									}
									if (RecieverTile.TileType == TileID.WirePipe)
									{

                                    }
									bool HasWire;
									switch (Wiring._currentWireColor)
									{
										case 1:
											HasWire = CurrentWire.RedWire;
											break;
										case 2:
											HasWire = CurrentWire.BlueWire;
											break;
										case 3:
											HasWire = CurrentWire.GreenWire;
											break;
										case 4:
											HasWire = CurrentWire.YellowWire;
											break;
										default:
											HasWire = false;
											break;
									}
									if (HasWire)
									{
										Point16 CurrentWirePoint = new Point16(CurrentX, CurrentY);
										byte value;
										if (Wiring._toProcess.TryGetValue(CurrentWirePoint, out value))
										{
											value -= 1;
											if (value == 0)
											{
												Wiring._toProcess.Remove(CurrentWirePoint);
											}
											else
											{
												Wiring._toProcess[CurrentWirePoint] = value;
											}
										}
										else
										{
											Wiring._wireList.PushBack(CurrentWirePoint);
											Wiring._wireDirectionList.PushBack((byte)direction);
											if (flags > 0)
											{
												Wiring._toProcess.Add(CurrentWirePoint, flags);
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
			if(WirelessSystem.Links.ContainsKey(new Point16(i, j + 1)))
			{
				ModContent.GetInstance<Wireless>().SyncRemoveLink(new Point16(i, j + 1));
			}
		}
		
		public override bool RightClick(int i, int j)
		{
			if (Main.tile[i, j].TileFrameY == 18 && WirelessSystem.Links.ContainsKey(new Point16(i, j)))
            {
                var coord = WirelessSystem.Links[new Point16(i, j)];
				//				Wiring.TripWire(i, j, 1, 1);
				Vector2? position = new(i * 16, j * 16);
			    SoundEngine.PlaySound(SoundID.Mech, position);
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
			
			if(Main.tile[i, j].TileFrameY == 18)
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
