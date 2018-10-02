using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Fields.Objects.Drops
{
    public class FieldDropMoney : FieldDrop
    {
        public int Money { get; set; }

        public override bool IsMoney => true;
        public override int Info => Money;

        public FieldDropMoney(int money)
        {
            Money = money;
        }

        public override void PickUp(FieldUser user)
        {
            user.ModifyStats(s => s.Money += Money, true);
        }
    }
}