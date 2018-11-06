using System.Collections.Generic;
using Edelstein.Common.Packets.Stats;
using Edelstein.Database.Entities.Types;
using Edelstein.Provider.Skills;

namespace Edelstein.Common.Utils.Skills
{
    public static class SkillInfo
    {
        public static IDictionary<TemporaryStatType, short> GetTemporaryStats(this SkillTemplate template, int level)
        {
            return template.LevelData[level].GetTemporaryStats();
        }

        public static IDictionary<TemporaryStatType, short> GetTemporaryStats(this SkillLevelTemplate template)
        {
            var temporaryStats = new Dictionary<TemporaryStatType, short>();

            if (template.PAD > 0) temporaryStats.Add(TemporaryStatType.PAD, template.PAD);
            if (template.PDD > 0) temporaryStats.Add(TemporaryStatType.PDD, template.PDD);
            if (template.MAD > 0) temporaryStats.Add(TemporaryStatType.MAD, template.MAD);
            if (template.MDD > 0) temporaryStats.Add(TemporaryStatType.MDD, template.MDD);
            if (template.ACC > 0) temporaryStats.Add(TemporaryStatType.ACC, template.ACC);
            if (template.EVA > 0) temporaryStats.Add(TemporaryStatType.EVA, template.EVA);
            if (template.Craft > 0) temporaryStats.Add(TemporaryStatType.Craft, template.Craft);
            if (template.Speed > 0) temporaryStats.Add(TemporaryStatType.Speed, template.Speed);
            if (template.Jump > 0) temporaryStats.Add(TemporaryStatType.Jump, template.Jump);
            if (template.Morph > 0) temporaryStats.Add(TemporaryStatType.Morph, template.Morph);

            if (template.EMHP > 0) temporaryStats.Add(TemporaryStatType.EMHP, template.EMHP);
            if (template.EMMP > 0) temporaryStats.Add(TemporaryStatType.EMMP, template.EMMP);
            if (template.EPAD > 0) temporaryStats.Add(TemporaryStatType.EPAD, template.EPAD);
            if (template.EPDD > 0) temporaryStats.Add(TemporaryStatType.EPDD, template.EPDD);
            // if (template.EMAD > 0) temporaryStats.Add(TemporaryStatType.EMAD, template.EMAD);
            if (template.EMDD > 0) temporaryStats.Add(TemporaryStatType.EMDD, template.EMDD);

            var skill = (Skill) template.TemplateID;
            
            if (skill == Skill.RogueDarkSight)
                temporaryStats.Add(TemporaryStatType.DarkSight, template.X);
            
            return temporaryStats;
        }
    }
}