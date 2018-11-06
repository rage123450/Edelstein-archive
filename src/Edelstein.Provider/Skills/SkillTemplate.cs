using System;
using System.Collections.Generic;
using System.Linq;
using PKG1;

namespace Edelstein.Provider.Skills
{
    public class SkillTemplate
    {
        public int TemplateID { get; set; }
        
        public short MaxLevel { get; set; }

        public IDictionary<int, int> ReqSkill;
        public IDictionary<int, SkillLevelTemplate> LevelData;

        public static SkillTemplate Parse(int templateId, PackageCollection collection)
        {
            var entry = collection.Resolve($"Skill/{templateId / 10000:D3}.img/skill/{templateId:D7}");
            return Parse(templateId, entry);
        }

        public static SkillTemplate Parse(int templateId, WZProperty p)
        {
            var entry = p.Resolve("common");
            var levelData = new Dictionary<int, SkillLevelTemplate>();

            if (entry != null)
            {
                var maxLevel = entry.ResolveFor<int>("maxLevel");

                for (var i = 1; i <= maxLevel; i++)
                    levelData.Add(i, SkillLevelTemplate.Parse(templateId, i, entry));
            }
            else
            {
                entry = p.Resolve("level");
                levelData = entry.Children
                    .ToDictionary(
                        c => Convert.ToInt32(c.Name),
                        c => SkillLevelTemplate.Parse(templateId, Convert.ToInt32(c.Name), c)
                    );
            }

            return new SkillTemplate
            {
                TemplateID = templateId,
                MaxLevel = (short) levelData.Count,
                ReqSkill = p.Resolve("req")?.Children
                               .ToDictionary(
                                   c => Convert.ToInt32(c.Name),
                                   c => c.ResolveFor<int>() ?? 0
                               ) ?? new Dictionary<int, int>(),
                LevelData = levelData
            };
        }
    }
}