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

        public int RUC { get; set; }
        public int TUC { get; set; }

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

        public int IUC { get; set; }
    }
}