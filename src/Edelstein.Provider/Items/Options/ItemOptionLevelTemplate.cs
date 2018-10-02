using PKG1;

namespace Edelstein.Provider.Items.Options
{
    public class ItemOptionLevelTemplate
    {
        public int Prob { get; set; }
        public int Time { get; set; }

        public short IncSTR { get; set; }
        public short IncDEX { get; set; }
        public short IncINT { get; set; }
        public short IncLUK { get; set; }
        public int IncHP { get; set; }
        public int IncMP { get; set; }
        public short IncACC { get; set; }
        public short IncEVA { get; set; }
        public short IncSpeed { get; set; }
        public short IncJump { get; set; }
        public int IncMaxHP { get; set; }
        public int IncMaxMP { get; set; }
        public short IncPAD { get; set; }
        public short IncMAD { get; set; }
        public short IncPDD { get; set; }
        public short IncMDD { get; set; }

        public short IncSTRr { get; set; }
        public short IncDEXr { get; set; }
        public short IncINTr { get; set; }
        public short IncLUKr { get; set; }
        public int IncMaxHPr { get; set; }
        public int IncMaxMPr { get; set; }
        public short IncACCr { get; set; }
        public short IncEVAr { get; set; }
        public short IncPADr { get; set; }
        public short IncMADr { get; set; }
        public short IncPDDr { get; set; }
        public short IncMDDr { get; set; }

        public short IncCr { get; set; }

        // public short IncCDr { get; set; }
        // public short IncMAMr { get; set; }
        // public short IncSkill { get; set; }
        public short IncAllSkill { get; set; }
        public short RecoveryHP { get; set; }
        public short RecoveryMP { get; set; }
        public short RecoveryUP { get; set; }
        public short MPConReduce { get; set; }
        public short MPConRestore { get; set; }
        public short IgnoreTargetDEF { get; set; }
        public short IgnoreDAM { get; set; }
        public short IgnoreDAMr { get; set; }
        public short IncDAMr { get; set; }
        public short DAMReflect { get; set; }
        public short AttackType { get; set; }
        public int IncMesoProb { get; set; }
        public int IncRewardProb { get; set; }
        public short Level { get; set; }
        public short Boss { get; set; }

        public static ItemOptionLevelTemplate Parse(WZProperty p)
        {
            return new ItemOptionLevelTemplate
            {
                Prob = p.ResolveFor<int>("prop") ?? 0,
                Time = p.ResolveFor<int>("time") ?? 0,
                IncSTR = p.ResolveFor<short>("incSTR") ?? 0,
                IncDEX = p.ResolveFor<short>("incDEX") ?? 0,
                IncINT = p.ResolveFor<short>("incINT") ?? 0,
                IncLUK = p.ResolveFor<short>("incLUK") ?? 0,
                IncHP = p.ResolveFor<int>("incHP") ?? 0,
                IncMP = p.ResolveFor<int>("incMP") ?? 0,
                IncACC = p.ResolveFor<short>("incACC") ?? 0,
                IncEVA = p.ResolveFor<short>("incEVA") ?? 0,
                IncSpeed = p.ResolveFor<short>("incSpeed") ?? 0,
                IncJump = p.ResolveFor<short>("incJump") ?? 0,
                IncMaxHP = p.ResolveFor<short>("incMHP") ?? 0,
                IncMaxMP = p.ResolveFor<short>("incMMP") ?? 0,
                IncPAD = p.ResolveFor<short>("incPAD") ?? 0,
                IncMAD = p.ResolveFor<short>("incMAD") ?? 0,
                IncPDD = p.ResolveFor<short>("incPDD") ?? 0,
                IncMDD = p.ResolveFor<short>("incMDD") ?? 0,

                IncSTRr = p.ResolveFor<short>("incSTRr") ?? 0,
                IncDEXr = p.ResolveFor<short>("incDEXr") ?? 0,
                IncINTr = p.ResolveFor<short>("incINTr") ?? 0,
                IncLUKr = p.ResolveFor<short>("incLUKr") ?? 0,
                IncACCr = p.ResolveFor<short>("incACCr") ?? 0,
                IncEVAr = p.ResolveFor<short>("incEVAr") ?? 0,
                IncMaxHPr = p.ResolveFor<short>("incMHPr") ?? 0,
                IncMaxMPr = p.ResolveFor<short>("incMMPr") ?? 0,
                IncPADr = p.ResolveFor<short>("incPADr") ?? 0,
                IncMADr = p.ResolveFor<short>("incMADr") ?? 0,
                IncPDDr = p.ResolveFor<short>("incPDDr") ?? 0,
                IncMDDr = p.ResolveFor<short>("incMDDr") ?? 0,

                IncCr = p.ResolveFor<short>("incCr") ?? 0,
                IncAllSkill = p.ResolveFor<short>("incAllskill") ?? 0,
                RecoveryHP = p.ResolveFor<short>("RecoveryHP") ?? 0,
                RecoveryMP = p.ResolveFor<short>("RecoveryMP") ?? 0,
                RecoveryUP = p.ResolveFor<short>("RecoveryUP") ?? 0,
                MPConReduce = p.ResolveFor<short>("mpconReduce") ?? 0,
                MPConRestore = p.ResolveFor<short>("mpRestore") ?? 0,
                IgnoreTargetDEF = p.ResolveFor<short>("ignoreTargetDEF") ?? 0,
                IgnoreDAM = p.ResolveFor<short>("ignoreDAM") ?? 0,
                IgnoreDAMr = p.ResolveFor<short>("ignoreDAMr") ?? 0,
                IncDAMr = p.ResolveFor<short>("incDAMr") ?? 0,
                DAMReflect = p.ResolveFor<short>("DAMreflect") ?? 0,
                AttackType = p.ResolveFor<short>("attackType") ?? 0,
                IncMesoProb = p.ResolveFor<int>("incMesoProp") ?? 0,
                IncRewardProb = p.ResolveFor<int>("incRewardProp") ?? 0,
                Level = p.ResolveFor<short>("level") ?? 0,
                Boss = p.ResolveFor<short>("boss") ?? 0
            };
        }
    }
}