using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Database.Entities;
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
            var record = _character.SkillRecords.SingleOrDefault(r => r.SkillID == template.TemplateID);

            Set(template, record?.Info + 1 ?? 1);
        }

        public void Set(SkillTemplate template, int level)
        {
            var record = _character.SkillRecords.SingleOrDefault(r => r.SkillID == template.TemplateID);

            if (!template.LevelData.ContainsKey(level)) return;
            if (record == null)
            {
                record = new SkillRecord
                {
                    SkillID = template.TemplateID,
                    Info = level,
                    MasterLevel = 0, // TODO
                    DateExpire = DateTime.Now // TODO
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
                packet.Encode<int>(record.SkillID);
                packet.Encode<int>(record.Info);
                packet.Encode<int>(record.MasterLevel);
                packet.Encode<long>(0); // skillRecord.DateExpire;
            }
        }
    }
}