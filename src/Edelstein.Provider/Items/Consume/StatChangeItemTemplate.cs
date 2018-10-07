using PKG1;

namespace Edelstein.Provider.Items.Consume
{
    public class StatChangeItemTemplate : ItemBundleTemplate
    {
        public int HP { get; set; }
        public int MP { get; set; }
        public int HPr { get; set; }
        public int MPr { get; set; }
        
        public short PAD { get; set; }
        public short PDD { get; set; }
        public short MAD { get; set; }
        public short MDD { get; set; }
        public short ACC { get; set; }
        public short EVA { get; set; }
        public short Craft { get; set; }
        public short Speed { get; set; }
        public short Jump { get; set; }

        public short Morph { get; set; }
        public int Time { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            HP = p.ResolveFor<short>("spec/hp") ?? 0;
            MP = p.ResolveFor<short>("spec/mp") ?? 0;
            HPr = p.ResolveFor<short>("spec/hpR") ?? 0;
            MPr = p.ResolveFor<short>("spec/mpR") ?? 0;
            
            PAD = p.ResolveFor<short>("spec/pad") ?? 0;
            PDD = p.ResolveFor<short>("spec/pdd") ?? 0;
            MAD = p.ResolveFor<short>("spec/mad") ?? 0;
            MDD = p.ResolveFor<short>("spec/mdd") ?? 0;
            ACC = p.ResolveFor<short>("spec/acc") ?? 0;
            EVA = p.ResolveFor<short>("spec/eva") ?? 0;
            Craft = p.ResolveFor<short>("spec/craft") ?? 0;
            Speed = p.ResolveFor<short>("spec/speed") ?? 0;
            Jump = p.ResolveFor<short>("spec/jump") ?? 0;

            Morph = p.ResolveFor<short>("spec/morph") ?? 0;
            Time = p.ResolveFor<int>("spec/time") ?? 0;
        }
    }
}