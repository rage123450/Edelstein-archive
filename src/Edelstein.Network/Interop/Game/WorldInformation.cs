using System;
using System.Collections.Generic;
using Edelstein.Network.Packets;

namespace Edelstein.Network.Interop.Game
{
    public class WorldInformation : IEncodable, IDecodable
    {
        public byte ID { get; set; }
        public string Name { get; set; }
        public byte State { get; set; }
        public string EventDesc { get; set; }
        public short EventEXP { get; set; }
        public short EventDrop { get; set; }
        public bool BlockCharCreation { get; set; }

        public ICollection<ChannelInformation> Channels { get; set; }

        public WorldInformation()
        {
            this.Channels = new List<ChannelInformation>();
        }

        public void Encode(OutPacket packet)
        {
            packet.Encode<byte>(ID);
            packet.Encode<string>(Name);
            packet.Encode<byte>(State);
            packet.Encode<string>(EventDesc);
            packet.Encode<short>(EventEXP);
            packet.Encode<short>(EventDrop);
            packet.Encode<bool>(BlockCharCreation);

            packet.Encode<byte>((byte) Channels.Count);
            foreach (var channelInformation in Channels)
                channelInformation.Encode(packet);
        }

        public void Decode(InPacket packet)
        {
            ID = packet.Decode<byte>();
            Name = packet.Decode<string>();
            State = packet.Decode<byte>();
            EventDesc = packet.Decode<string>();
            EventEXP = packet.Decode<short>();
            EventDrop = packet.Decode<short>();
            BlockCharCreation = packet.Decode<bool>();

            var channelCount = packet.Decode<byte>();
            for (var i = 0; i < channelCount; i++)
            {
                var channelInformation = new ChannelInformation();

                channelInformation.Decode(packet);
                Channels.Add(channelInformation);
            }
        }
    }
}