using PKG1;

namespace Edelstein.Provider.Items.Install
{
    public class PortableChairItemTemplate : ItemBundleTemplate
    {
        public int RecoveryHP { get; set; }
        public int RecoveryMP { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            RecoveryHP = p.ResolveFor<int>("info/recoveryHP") ?? 0;
            RecoveryMP = p.ResolveFor<int>("info/recoveryMP") ?? 0;
        }
    }
}