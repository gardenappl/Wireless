
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace Wireless
{
	public class Wireless : Mod
	{
		public enum MessageType : byte
		{
			AddLink,
			RemoveLink,
			TripWire
		}
		
		public override void Load()
		{
			var text = CreateTranslation("StartLink");
			text.SetDefault("Starting link...");
			text.AddTranslation(GameCulture.Russian, "Координаты сохранены");
			AddTranslation(text);
			text = CreateTranslation("SuccessLink");
			text.SetDefault("Linked successfully!");
			text.AddTranslation(GameCulture.Russian, "Соединено успешно!");
			AddTranslation(text);
			text = CreateTranslation("StoredCoords");
			text.SetDefault("Stored coordinates: {0}");
			text.AddTranslation(GameCulture.Russian, "Сохранённые кординаты: {0}");
			AddTranslation(text);
			text = CreateTranslation("LinkToItself");
			text.SetDefault("A Transciever cannot be linked to itself.");
			text.AddTranslation(GameCulture.Russian, "Нельзя присоединить приёмопередатчик самому к себе.");
			AddTranslation(text);
		}
		
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			var msgType = (MessageType)reader.ReadByte();
			switch(msgType)
			{
				case MessageType.AddLink:
					var transmitter = new Point16(reader.ReadInt16(), reader.ReadInt16());
					var receiver = new Point16(reader.ReadInt16(), reader.ReadInt16());
					WirelessWorld.Links[transmitter] = receiver;
					
					if(Main.netMode == NetmodeID.Server) //Server-side
						SyncAddLink(transmitter, receiver, whoAmI); //Broadcast the change to other clients
					break;
				case MessageType.RemoveLink:
					transmitter = new Point16(reader.ReadInt16(), reader.ReadInt16());
					WirelessWorld.Links.Remove(transmitter);
					
					if(Main.netMode == NetmodeID.Server)
						SyncRemoveLink(transmitter, whoAmI);
					break;
				case MessageType.TripWire:
					receiver = new Point16(reader.ReadInt16(), reader.ReadInt16());
					ActivateReceiver(receiver);
					
					if(Main.netMode == NetmodeID.Server)
						RemoteClient.CheckSection(whoAmI, receiver.ToWorldCoordinates());
					break;
			}
		}
		
		public void SyncAddLink(Point16 transmitter, Point16 receiver, int ignoreClient = -1)
		{
			WirelessWorld.Links[transmitter] = receiver;
			if(Main.netMode != 0)
			{
				var msg = GetPacket();
				msg.Write((byte)MessageType.AddLink);
				msg.Write(transmitter.X);
				msg.Write(transmitter.Y);
				msg.Write(receiver.X);
				msg.Write(receiver.Y);
				msg.Send(ignoreClient: ignoreClient);
			}
		}
		
		public void SyncRemoveLink(Point16 transmitter, int ignoreClient = -1)
		{
			WirelessWorld.Links.Remove(transmitter);
			if(Main.netMode != 0)
			{
				var msg = GetPacket();
				msg.Write((byte)MessageType.RemoveLink);
				msg.Write(transmitter.X);
				msg.Write(transmitter.Y);
				msg.Send(ignoreClient: ignoreClient);
			}
		}
		
		public void SyncActivate(Point16 receiver)
		{
			if(Main.netMode == 1)
			{
				var msg = GetPacket();
				msg.Write((byte)MessageType.TripWire);
				msg.Write(receiver.X);
				msg.Write(receiver.Y);
				msg.Send();
			}
			else
			{
//				if(WirelessUtils.IsReceiver(receiver, this))
//				{
//					Wiring.TripWire(receiver.X, receiver.Y, 1, 1);
//					int currentColor = Wiring._currentWireColor;
//					for(int i = 1; i <= 4; i++)
//					{
//						Wiring._currentWireColor = i;
//						if(Main.tile[receiver.X, receiver.Y].type == TileType(Names.WirelessTransceiver))
//							GetTile(Names.WirelessTransceiver).HitWire(receiver.X, receiver.Y);
//					}
//					Wiring._currentWireColor = currentColor;
//				}
				ActivateReceiver(receiver);
			}
		}
		
		void ActivateReceiver(Point16 receiver)
		{
			if(WirelessUtils.IsReceiver(receiver, this))
			{
				Wiring.TripWire(receiver.X, receiver.Y, 1, 1);
				if(Main.tile[receiver.X, receiver.Y].type == TileType(Names.WirelessTransceiver) && WirelessWorld.Links.ContainsKey(receiver))
					Wiring.TripWire(WirelessWorld.Links[receiver].X, WirelessWorld.Links[receiver].Y, 1, 1);
			}
		}
		
		public static void Log(object message, params object[] formatData)
		{
			ErrorLogger.Log("[Wireless] " + string.Format(message.ToString(), formatData));
		}

		//Hamstar's Mod Helpers integration
		public static string GithubUserName { get { return "goldenapple3"; } }
		public static string GithubProjectName { get { return "Wireless"; } }
	}
}
