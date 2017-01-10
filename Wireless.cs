
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
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
		
		public Wireless()
		{
			Properties = new ModProperties
			{
				Autoload = true,
				AutoloadSounds = true
			};
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
					if(Main.netMode == 2) //Server-side
					{
						SyncAddLink(transmitter, receiver, whoAmI); //Broadcast the change to other clients
					}
					break;
				case MessageType.RemoveLink:
					transmitter = new Point16(reader.ReadInt16(), reader.ReadInt16());
					
					WirelessWorld.Links.Remove(transmitter);
					if(Main.netMode == 2)
					{
						SyncRemoveLink(transmitter, whoAmI);
					}
					break;
				case MessageType.TripWire:
					Wiring.TripWire(reader.ReadInt16(), reader.ReadInt16(), 1, 1);
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
				Wiring.TripWire(receiver.X, receiver.Y, 1, 1);
			}
		}
		
		public static void Log(object message, params object[] formatData)
		{
			ErrorLogger.Log("[Wireless] " + string.Format(message.ToString(), formatData));
		}
	}
}
