using System;
using PKG1;

namespace Edelstein.Provider.Items
{
    public class ItemEquipTemplate : ItemTemplate
    {
        public int ReqSTR { get; set; }
        public int ReqDEX { get; set; }
        public int ReqINT { get; set; }
        public int ReqLUK { get; set; }
        public int ReqPOP { get; set; }
        public int ReqJob { get; set; }
        public int ReqLevel { get; set; }

        public int TUC { get; set; }

        public int IncSTR { get; set; }
        public int IncDEX { get; set; }
        public int IncINT { get; set; }
        public int IncLUK { get; set; }
        public int IncMaxHP { get; set; }
        public int IncMaxMP { get; set; }
        public int IncPAD { get; set; }
        public int IncMAD { get; set; }
        public int IncPDD { get; set; }
        public int IncMDD { get; set; }
        public int IncACC { get; set; }
        public int IncEVA { get; set; }
        public int IncCraft { get; set; }
        public int IncSpeed { get; set; }
        public int IncJump { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            ReqSTR = p.ResolveFor<int>("info/reqSTR") ?? 0;
            ReqDEX = p.ResolveFor<int>("info/reqDEX") ?? 0;
            ReqINT = p.ResolveFor<int>("info/reqINT") ?? 0;
            ReqLUK = p.ResolveFor<int>("info/reqLUK") ?? 0;
            ReqPOP = p.ResolveFor<int>("info/reqPOP") ?? 0;
            ReqJob = p.ResolveFor<int>("info/reqJob") ?? 0;
            ReqLevel = p.ResolveFor<int>("info/reqLevel") ?? 0;
            TUC = p.ResolveFor<int>("info/tuc") ?? 0;
            IncSTR = p.ResolveFor<int>("info/incSTR") ?? 0;
            IncDEX = p.ResolveFor<int>("info/incDEX") ?? 0;
            IncINT = p.ResolveFor<int>("info/incINT") ?? 0;
            IncLUK = p.ResolveFor<int>("info/incLUK") ?? 0;
            IncMaxHP = p.ResolveFor<int>("info/incMHP") ?? 0;
            IncMaxMP = p.ResolveFor<int>("info/incMMP") ?? 0;
            IncPAD = p.ResolveFor<int>("info/incPAD") ?? 0;
            IncMAD = p.ResolveFor<int>("info/incMAD") ?? 0;
            IncPDD = p.ResolveFor<int>("info/incPDD") ?? 0;
            IncMDD = p.ResolveFor<int>("info/incMDD") ?? 0;
            IncACC = p.ResolveFor<int>("info/incACC") ?? 0;
            IncEVA = p.ResolveFor<int>("info/incEVA") ?? 0;
            IncCraft = p.ResolveFor<int>("info/incCraft") ?? 0;
            IncSpeed = p.ResolveFor<int>("info/incSpeed") ?? 0;
            IncJump = p.ResolveFor<int>("info/incJump") ?? 0;
        }
    }
}