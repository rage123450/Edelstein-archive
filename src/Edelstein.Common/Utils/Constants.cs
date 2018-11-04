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

        public static bool IsSkillNeedMasterLevel(Skill skill)
        {
            var id = (int) skill;
            var job = (Job) (id / 10000);
            var v4 = (int) job / 10;

            if (v4 == 22 || (int) job == 2001)
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