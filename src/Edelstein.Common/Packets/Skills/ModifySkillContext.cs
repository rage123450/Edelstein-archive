using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Types;
using Edelstein.Network.Packets;
using Edelstein.Provider.Skills;

namespace Edelstein.Common.Packets.Skills
{
    public class ModifySkillContext : IEncodable
    {
        private readonly Character _character;
        private readonly ICollection<SkillRecord> _records;

        public ModifySkillContext(Character character)
        {
            _character = character;
            _records = new List<SkillRecord>();
        }

        public void Add(SkillTemplate template)
        {
            var record = _character.SkillRecords.SingleOrDefault(r => (int) r.Skill == template.TemplateID);

            Set(template, record?.Info + 1 ?? 1);
        }

        public void Set(SkillTemplate template, int level, int masterLevel = 0)
        {
            var record = _character.SkillRecords.SingleOrDefault(r => (int) r.Skill == template.TemplateID);

            if (!template.LevelData.ContainsKey(level)) return;
            if (record == null)
            {
                record = new SkillRecord
                {
                    Skill = (Skill) template.TemplateID,
                    Info = level,
                    MasterLevel = masterLevel
                };

                _character.SkillRecords.Add(record);
            }
            else record.Info = level;

            _records.Add(record);
        }

        public void Encode(OutPacket packet)
        {
            packet.Encode<short>((short) _records.Count);
            foreach (var record in _records) // TODO: extension method to remove redundant code with characterdata
            {
                packet.Encode<Skill>(record.Skill);
                packet.Encode<int>(record.Info);
                packet.Encode<int>(record.MasterLevel);
                packet.Encode<long>(0); // skillRecord.DateExpire;
            }
        }
    }
}