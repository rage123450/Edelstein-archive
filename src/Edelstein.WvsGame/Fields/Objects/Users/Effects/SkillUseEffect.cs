using System;
using Edelstein.Network.Packets;

namespace Edelstein.WvsGame.Fields.Objects.Users.Effects
{
    public class SkillUseEffect : UserEffect
    {
        private readonly int _skillTemplateID;
        private readonly byte _skillLevel;
        private readonly Action<OutPacket> _additional;

        public SkillUseEffect(int skillTemplateId, byte skillLevel, Action<OutPacket> additional = null)
            : base(UserEffectType.SkillUse)
        {
            _skillTemplateID = skillTemplateId;
            _skillLevel = skillLevel;
            _additional = additional;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<int>(_skillTemplateID);
            packet.Encode<byte>(0);
            packet.Encode<byte>(_skillLevel);

            _additional?.Invoke(packet);
        }
    }
}