using System.Collections.Generic;
using System.Linq;
using PKG1;

namespace Edelstein.Provider.Items.Consume
{
    public class MobSummonItemTemplate : ItemBundleTemplate
    {
        public int Type { get; set; }
        public ICollection<MobSummonEntry> Mobs { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            Type = p.ResolveFor<int>("info/type") ?? 0;
            Mobs = p.Resolve("mob")?.Children
                       .Select(MobSummonEntry.Parse)
                       .ToList() ?? new List<MobSummonEntry>();
        }
    }

    public class MobSummonEntry
    {
        public int TemplateID { get; set; }
        public int Prob { get; set; }

        public static MobSummonEntry Parse(WZProperty p)
        {
            return new MobSummonEntry
            {
                TemplateID = p.ResolveFor<int>("id") ?? 0,
                Prob = p.ResolveFor<int>("prob") ?? 0
            };
        }
    }
}