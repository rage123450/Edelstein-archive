using System.Collections.Generic;
using Edelstein.Database.Entities.Types;
using Edelstein.Network.Packets;
using MoreLinq;

namespace Edelstein.Common.Packets.Messages
{
    public class SkillExpireMessage : Message
    {
        protected override byte Type => 0xE;
        private readonly ICollection<Skill> _skills;

        public SkillExpireMessage(ICollection<Skill> skills)
        {
            _skills = skills;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<byte>((byte) _skills.Count);
            _skills.ForEach(s => packet.Encode<int>((int) s));
        }
    }
}