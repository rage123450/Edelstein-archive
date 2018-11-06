using Edelstein.Database.Entities.Types;

namespace Edelstein.Common.Utils
{
    public static class Constants
    {
        public static int GetJobLevel(Job job)
        {
            var id = (int) job;
            var result = 0;

            if (id % 100 > 0 && id != 2001)
            {
                var v1 = id % 10;
                if (id / 10 == 43) v1 = (id - 430) / 2;
                var v2 = v1 + 2;
                if (v2 >= 2 && (v2 <= 4 || v2 <= 10 && (id / 100 == 22 || id == 2001)))
                    result = v2;
            }
            else result = 1;

            return result;
        }
        
        public static bool IsIgnoreMasterLevelForCommon(Skill skill)
        {
            return skill == Skill.HeroCombatMastery ||
                   skill == Skill.PaladinBlessingArmor ||
                   skill == Skill.DarkknightBeholdersRevenge ||
                   skill == Skill.Archmage1MasterMagic ||
                   skill == Skill.Archmage2MasterMagic ||
                   skill == Skill.BishopMasterMagic ||
                   skill == Skill.BowmasterVengeance ||
                   skill == Skill.BowmasterMarkmanShip ||
                   skill == Skill.CrossbowmasterUltimateStrafe ||
                   skill == Skill.CrossbowmasterMarkmanShip ||
                   skill == Skill.NightlordSpiritJavelin ||
                   skill == Skill.ShadowerGrid ||
                   skill == Skill.ViperCounterAttack ||
                   skill == Skill.CaptainCounterAttack ||
                   skill == Skill.BmageEnergize ||
                   skill == Skill.WildhunterWildInstinct;
        }

        public static bool IsSkillNeedMasterLevel(Skill skill)
        {
            if (IsIgnoreMasterLevelForCommon(skill)) return false;

            var id = (int) skill;
            var job = (Job) (id / 10000);
            var v4 = (int) job / 10;

            if (v4 == 22 || job == Job.Evanjr)
            {
                var jobLevel = GetJobLevel(job);

                return jobLevel == 9 ||
                       jobLevel == 10 ||
                       skill == Skill.EvanMagicGuard ||
                       skill == Skill.EvanMagicBooster ||
                       skill == Skill.EvanMagicCritical;
            }

            if (v4 == 43)
            {
                return GetJobLevel(job) == 4 ||
                       skill == Skill.Dual2SlashStorm ||
                       skill == Skill.Dual3HustleDash ||
                       skill == Skill.Dual4MirrorImaging ||
                       skill == Skill.Dual4FlyingAssaulter;
            }

            if ((int) job == 100 * v4) return false;
            return (int) job % 10 == 2;
        }
    }
}