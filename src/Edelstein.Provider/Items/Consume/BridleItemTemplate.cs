using PKG1;

namespace Edelstein.Provider.Items.Consume
{
    public class BridleItemTemplate : ItemBundleTemplate
    {
        // tagRECT rc;
        public int CreateItemID { get; set; }
        public int CreateItemPeriod { get; set; }
        public int CatchPercentageHP { get; set; }
        public int BridleMsgType { get; set; }
        public int UseDelay { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            CreateItemID = p.ResolveFor<int>("info/create") ?? 0;
            CreateItemPeriod = p.ResolveFor<int>("info/period") ?? 0;
            CatchPercentageHP = p.ResolveFor<int>("info/mobHP") ?? 0;
            BridleMsgType = p.ResolveFor<int>("info/bridleMsgType") ?? 0;
            UseDelay = p.ResolveFor<int>("info/useDelay") ?? 0;
        }
    }
}