using System.Collections.Generic;
using System.Linq;
using PKG1;

namespace Edelstein.Provider.Items.Consume
{
    public class SkillLearnItemTemplate : ItemBundleTemplate
    {
        public int MasterLevel { get; set; }
        public int ReqLevel { get; set; }
        public ICollection<int> Skill { get; set; }

        public int Success { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            MasterLevel = p.ResolveFor<int>("info/masterLevel") ?? 0;
            ReqLevel = p.ResolveFor<int>("info/reqSkillLevel") ?? 0;
            Skill = p.Resolve("info/skill")?.Children
                        .Select(c => c.ResolveFor<int>() ?? 0)
                        .ToList() ?? new List<int>();
            
            Success = p.ResolveFor<int>("info/success") ?? 0;
        }
    }
}