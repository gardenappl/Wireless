
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Wireless
{
	public class WirelessSystem : ModSystem
	{
		public static Dictionary<Point16, Point16> Links;
		
		public override void OnModLoad()
		{
			Links = new Dictionary<Point16, Point16>();
		}
		
		//Don't ask me why I'm torturing myself by doing it this way...
		//I could've just used an int array...
		//Or a list of bytes... But I guess I'm just masochistic :/
		public override void SaveWorldData(TagCompound tag)
		{
			byte[] bytes = new byte[Links.Count * 8];
			int i = 0;
			foreach (var kvp in Links)
			{
				bytes[i] = BitConverter.GetBytes(kvp.Key.X)[0];
				bytes[i + 1] = BitConverter.GetBytes(kvp.Key.X)[1];
				bytes[i + 2] = BitConverter.GetBytes(kvp.Key.Y)[0];
				bytes[i + 3] = BitConverter.GetBytes(kvp.Key.Y)[1];
				bytes[i + 4] = BitConverter.GetBytes(kvp.Value.X)[0];
				bytes[i + 5] = BitConverter.GetBytes(kvp.Value.X)[1];
				bytes[i + 6] = BitConverter.GetBytes(kvp.Value.Y)[0];
				bytes[i + 7] = BitConverter.GetBytes(kvp.Value.Y)[1];
				i += 8;
			}
			tag["WirelessCoords"] = bytes;
		}
		
		public override void LoadWorldData(TagCompound tag)
		{
			byte[] bytes = tag.GetByteArray("WirelessCoords");
			for (int i = 0; i < bytes.Length; i += 8)
			{
				var transmitter = new Point16(BitConverter.ToInt16(bytes, i), BitConverter.ToInt16(bytes, i + 2));
				var receiver = new Point16(BitConverter.ToInt16(bytes, i + 4), BitConverter.ToInt16(bytes, i + 6));
				//				Wireless.Log("{0}, {1}", transmitter, receiver);
				if (!WirelessUtils.AlreadyExists(transmitter, receiver))
					Links.Add(transmitter, receiver);
			}
		}
		
//		public override void LoadLegacy(BinaryReader reader)
//		{
//			Wireless.Log("LOAD LEGACY");
			
//			int version = reader.ReadInt32();
			
//			int linksCount = reader.ReadInt32();
//			for(int i = 0; i < linksCount; i++)
//			{
//				var transmitter = new Point16(reader.ReadInt16(), reader.ReadInt16());
//				var receiver = new Point16(reader.ReadInt16(), reader.ReadInt16());
//				Wireless.Log("{0}, {1}", transmitter, receiver);
//				Links.Add(transmitter, receiver);
//			}
//		}
		
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(Links.Count);
			foreach(var kvp in Links)
			{
				writer.Write(kvp.Key.X);
				writer.Write(kvp.Key.Y);
				writer.Write(kvp.Value.X);
				writer.Write(kvp.Value.Y);
			}
		}
		
		public override void NetReceive(BinaryReader reader)
		{
			int linksCount = reader.ReadInt32();
			Links = new Dictionary<Point16, Point16>(linksCount);
			for (int i = 0; i < linksCount; i++)
            {
				Point16 transmitter = new Point16(reader.ReadInt16(), reader.ReadInt16());
				Point16 receiver = new Point16(reader.ReadInt16(), reader.ReadInt16());
				if (!WirelessUtils.AlreadyExists(transmitter, receiver))
					Links.Add(new Point16(reader.ReadInt16(), reader.ReadInt16()), new Point16(reader.ReadInt16(), reader.ReadInt16()));
			}
				
		}
	}
}
