using PKG1;

namespace Edelstein.Provider.Items.Consume
{
    public class PortalScrollItemTemplate : ItemBundleTemplate
    {
        public bool RandomMoveInFieldSet { get; set; }
        public bool IgnoreContinent { get; set; }
        public int MoveTo { get; set; }
        public int ReturnMapQR { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            RandomMoveInFieldSet = p.ResolveFor<bool>("spec/randomMoveInFieldSet") ?? false;
            IgnoreContinent = p.ResolveFor<bool>("spec/ignoreContinent") ?? false;
            MoveTo = p.ResolveFor<int>("spec/moveTo") ?? 999999999;
            ReturnMapQR = p.ResolveFor<int>("spec/returnMapQR") ?? 0;
        }
    }
}