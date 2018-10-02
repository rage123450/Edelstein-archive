using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Network.Packets;
using MoreLinq.Extensions;

namespace Edelstein.WvsGame.Fields.Objects.Users.Stats
{
    public class ModifyTemporaryStatContext
    {
        private readonly FieldUser _user;
        public readonly List<TemporaryStatEntry> ResetStats;
        public readonly List<TemporaryStatEntry> SetStats;

        public ModifyTemporaryStatContext(FieldUser user)
        {
            _user = user;
            ResetStats = new List<TemporaryStatEntry>();
            SetStats = new List<TemporaryStatEntry>();
        }

        public void Set(TemporaryStatType type, int templateID, short option, DateTime dateExpire)
        {
            Reset(type);
            SetStats.Add(new TemporaryStatEntry
            {
                Type = type,
                TemplateID = templateID,
                Option = option,
                DateExpire = dateExpire
            });
        }

        public void Reset(TemporaryStatType type)
        {
            ResetStats.AddRange(
                _user.TemporaryStat.Entries
                    .Where(s => s.Key == type)
                    .Select(s => s.Value)
                    .ToList()
            );
        }
    }
}