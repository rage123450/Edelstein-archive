using PKG1;

namespace Edelstein.Provider.Items
{
    public class ItemEquipTemplate : ItemTemplate
    {
        public short ReqSTR { get; set; }
        public short ReqDEX { get; set; }
        public short ReqINT { get; set; }
        public short ReqLUK { get; set; }
        public short ReqPOP { get; set; }
        public short ReqJob { get; set; }
        public byte ReqLevel { get; set; }

        public byte TUC { get; set; }
        public short IncSTR { get; set; }
        public short IncDEX { get; set; }
        public short IncINT { get; set; }
        public short IncLUK { get; set; }
        public int IncMaxHP { get; set; }
        public int IncMaxMP { get; set; }
        public int IncMaxHPr { get; set; }
        public int IncMaxMPr { get; set; }
        public short IncPAD { get; set; }
        public short IncMAD { get; set; }
        public short IncPDD { get; set; }
        public short IncMDD { get; set; }
        public short IncACC { get; set; }
        public short IncEVA { get; set; }
        public short IncCraft { get; set; }
        public short IncSpeed { get; set; }
        public short IncJump { get; set; }

        // fs, swim, tamingmob
        // public int IUC { get; set; }
        // public byte MinGrade { get; set; }

        public bool OnlyEquip { get; set; }
        public bool TradeBlockEquip { get; set; }

        // nirPoison, nirIce, nirFire, nirLight, nirHoly
        // other random stuff

        public bool NotExtend { get; set; }
        public bool SharableOnce { get; set; }

        public byte AppliableKarmaType { get; set; }

        public int SetItemID { get; set; }

        public int Durability { get; set; }
        // public int EnchantCategory { get; set; }
        // public int Transform { get; set; }
        // public int IUCMax { get; set; }

        public override void Parse(int templateId, WZProperty p)
        {
            base.Parse(templateId, p);

            ReqSTR = p.ResolveFor<short>("info/reqSTR") ?? 0;
            ReqDEX = p.ResolveFor<short>("info/reqDEX") ?? 0;
            ReqINT = p.ResolveFor<short>("info/reqINT") ?? 0;
            ReqLUK = p.ResolveFor<short>("info/reqLUK") ?? 0;
            ReqPOP = p.ResolveFor<short>("info/reqPOP") ?? 0;
            ReqJob = p.ResolveFor<short>("info/reqJob") ?? 0;
            ReqLevel = p.ResolveFor<byte>("info/reqLevel") ?? 0;

            TUC = p.ResolveFor<byte>("info/tuc") ?? 0;
            IncSTR = p.ResolveFor<short>("info/incSTR") ?? 0;
            IncDEX = p.ResolveFor<short>("info/incDEX") ?? 0;
            IncINT = p.ResolveFor<short>("info/incINT") ?? 0;
            IncLUK = p.ResolveFor<short>("info/incLUK") ?? 0;
            IncMaxHP = p.ResolveFor<int>("info/incMHP") ?? 0;
            IncMaxMP = p.ResolveFor<int>("info/incMMP") ?? 0;
            IncMaxHPr = p.ResolveFor<int>("info/incMHPr") ?? 0;
            IncMaxMPr = p.ResolveFor<int>("info/incMMPr") ?? 0;
            IncPAD = p.ResolveFor<short>("info/incPAD") ?? 0;
            IncMAD = p.ResolveFor<short>("info/incMAD") ?? 0;
            IncPDD = p.ResolveFor<short>("info/incPDD") ?? 0;
            IncMDD = p.ResolveFor<short>("info/incMDD") ?? 0;
            IncACC = p.ResolveFor<short>("info/incACC") ?? 0;
            IncEVA = p.ResolveFor<short>("info/incEVA") ?? 0;
            IncCraft = p.ResolveFor<short>("info/incCraft") ?? 0;
            IncSpeed = p.ResolveFor<short>("info/incSpeed") ?? 0;
            IncJump = p.ResolveFor<short>("info/incJump") ?? 0;

            OnlyEquip = p.ResolveFor<bool>("info/onlyEquip") ?? false;
            TradeBlockEquip = p.ResolveFor<bool>("info/equipTradeBlock") ?? false;

            NotExtend = p.ResolveFor<bool>("info/notExtend") ?? false;
            SharableOnce = p.ResolveFor<bool>("info/sharableOnce") ?? false;

            AppliableKarmaType = p.ResolveFor<byte>("info/tradeAvailable") ?? 0;

            SetItemID = p.ResolveFor<int>("info/setItemID") ?? 0;
            Durability = p.ResolveFor<int>("info/durability") ?? -1;
        }
    }
}