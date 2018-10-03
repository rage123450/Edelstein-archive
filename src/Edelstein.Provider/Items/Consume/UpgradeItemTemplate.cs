using System.Collections.Generic;
using System.Linq;
using PKG1;

namespace Edelstein.Provider.Items.Consume
{
    public class UpgradeItemTemplate : ItemBundleTemplate
    {
        public short IncSTR { get; set; }
        public short IncDEX { get; set; }
        public short IncINT { get; set; }
        public short IncLUK { get; set; }
        public int IncMaxHP { get; set; }
        public int IncMaxMP { get; set; }
        public short IncPAD { get; set; }
        public short IncMAD { get; set; }
        public short IncPDD { get; set; }
        public short IncMDD { get; set; }
        public short IncACC { get; set; }
        public short IncEVA { get; set; }
        public short IncCraft { get; set; }
        public short IncSpeed { get; set; }
        public short IncJump { get; set; }

        public bool PreventSlip { get; set; }
        public bool WarmSupport { get; set; }

        // recover

        public int Success { get; set; }
        public int Cursed { get; set; }

        // successRates, cursedRates

        public bool RandomStat { get; set; }

        public ICollection<int> ReqItem { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            IncSTR = p.ResolveFor<short>("info/incSTR") ?? 0;
            IncDEX = p.ResolveFor<short>("info/incDEX") ?? 0;
            IncINT = p.ResolveFor<short>("info/incINT") ?? 0;
            IncLUK = p.ResolveFor<short>("info/incLUK") ?? 0;
            IncMaxHP = p.ResolveFor<int>("info/incMHP") ?? 0;
            IncMaxMP = p.ResolveFor<int>("info/incMMP") ?? 0;
            IncPAD = p.ResolveFor<short>("info/incPAD") ?? 0;
            IncMAD = p.ResolveFor<short>("info/incMAD") ?? 0;
            IncPDD = p.ResolveFor<short>("info/incPDD") ?? 0;
            IncMDD = p.ResolveFor<short>("info/incMDD") ?? 0;
            IncACC = p.ResolveFor<short>("info/incACC") ?? 0;
            IncEVA = p.ResolveFor<short>("info/incEVA") ?? 0;
            IncCraft = p.ResolveFor<short>("info/incCraft") ?? 0;
            IncSpeed = p.ResolveFor<short>("info/incSpeed") ?? 0;
            IncJump = p.ResolveFor<short>("info/incJump") ?? 0;

            PreventSlip = p.ResolveFor<bool>("info/preventslip") ?? false;
            WarmSupport = p.ResolveFor<bool>("info/warmsupport") ?? false;

            Success = p.ResolveFor<int>("info/success") ?? 0;
            Cursed = p.ResolveFor<int>("info/cursed") ?? 0;

            RandomStat = p.ResolveFor<bool>("info/randstat") ?? false;

            ReqItem = p.Resolve("req")?.Children
                          .Select(c => c.ResolveFor<int>() ?? 0)
                          .ToList() ?? new List<int>();
        }
    }
}