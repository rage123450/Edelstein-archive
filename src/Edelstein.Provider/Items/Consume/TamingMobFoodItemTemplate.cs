using PKG1;

namespace Edelstein.Provider.Items.Consume
{
    public class TamingMobFoodItemTemplate : ItemBundleTemplate
    {
        public short IncFatigue { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            IncFatigue = p.ResolveFor<short>("spec/incFatigue") ?? 0;
        }
    }
}