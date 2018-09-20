using System.Collections.Generic;

namespace Edelstein.Network.Interop.Game
{
    public class WorldInformation
    {
        public byte ID { get; set; }
        public string Name { get; set; }
        public byte State { get; set; }
        public string EventDesc { get; set; }
        public short EventEXP { get; set; }
        public short EventDrop { get; set; }
        public bool BlockCharCreation { get; set; }

        public ICollection<ChannelInformation> Channels { get; set; }
    }
}