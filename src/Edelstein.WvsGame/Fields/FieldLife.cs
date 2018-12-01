namespace Edelstein.WvsGame.Fields
{
    public abstract class FieldLife : FieldObj
    {
        public byte MoveAction { get; set; }
        public short Foothold { get; set; }
    }
}