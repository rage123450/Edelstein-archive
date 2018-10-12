using PKG1;

namespace Edelstein.Provider.Items.Set
{
    public class SetItemEffectTemplate
    {
        public short IncSTR { get; set; }
        public short IncDEX { get; set; }
        public short IncINT { get; set; }
        public short IncLUK { get; set; }
        public short IncMaxHP { get; set; }
        public short IncMaxMP { get; set; }
        public short IncPAD { get; set; }
        public short IncMAD { get; set; }
        public short IncPDD { get; set; }
        public short IncMDD { get; set; }
        public short IncACC { get; set; }
        public short IncEVA { get; set; }
        public short IncCraft { get; set; }
        public short IncSpeed { get; set; }
        public short IncJump { get; set; }

        public static SetItemEffectTemplate Parse(WZProperty p)
        {
            return new SetItemEffectTemplate
            {
                IncSTR = p.ResolveFor<short>("incSTR") ?? 0,
                IncDEX = p.ResolveFor<short>("incDEX") ?? 0,
                IncINT = p.ResolveFor<short>("incINT") ?? 0,
                IncLUK = p.ResolveFor<short>("incLUK") ?? 0,
                IncMaxHP = p.ResolveFor<short>("incMHP") ?? 0,
                IncMaxMP = p.ResolveFor<short>("incMMP") ?? 0,
                IncPAD = p.ResolveFor<short>("incPAD") ?? 0,
                IncMAD = p.ResolveFor<short>("incMAD") ?? 0,
                IncPDD = p.ResolveFor<short>("incPDD") ?? 0,
                IncMDD = p.ResolveFor<short>("incMDD") ?? 0,
                IncACC = p.ResolveFor<short>("incACC") ?? 0,
                IncEVA = p.ResolveFor<short>("incEVA") ?? 0,
                IncCraft = p.ResolveFor<short>("incCraft") ?? 0,
                IncSpeed = p.ResolveFor<short>("incSpeed") ?? 0,
                IncJump = p.ResolveFor<short>("incJump") ?? 0,
            };
        }
    }
}