using PKG1;

namespace Edelstein.Provider.Items.Consume
{
    public class BulletItemTemplate : ItemBundleTemplate
    {
        public short IncPAD { get; set; }
        public byte ReqLevel { get; set; }
        public float SellUnitPrice { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            IncPAD = p.ResolveFor<short>("info/incPAD") ?? 0;
            ReqLevel = p.ResolveFor<byte>("info/reqLevel") ?? 0;
            SellUnitPrice = p.ResolveFor<float>("info/unitPrice") ?? 0.0f;
        }
    }
}