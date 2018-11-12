using PKG1;

namespace Edelstein.Provider.Reactors
{
    public class ReactorEventTemplate
    {
        /*
            int nType;
            tagRECT rcCheckArea;
            ZArray<long> aActiveSkillID;
         */

        public int Type { get; set; }
        public int ToState { get; set; }

        public static ReactorEventTemplate Parse(WZProperty p)
        {
            return new ReactorEventTemplate
            {
                Type = p.ResolveFor<int>("type") ?? 0,
                ToState = p.ResolveFor<int>("state") ?? 0
            };
        }
    }
}