using Edelstein.Network.Packets;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Fields
{
    public abstract class FieldObjectControlled : FieldObject
    {
        public FieldUser Controller { get; private set; }

        public void ChangeController(FieldUser controller)
        {
            if (Controller?.Field == Field)
                Controller?.SendPacket(GetChangeControllerPacket(false));
            Controller = controller;
            
            if (controller == null) return;
            Controller.SendPacket(GetChangeControllerPacket(true));
        }

        public abstract OutPacket GetChangeControllerPacket(bool setAsController);
    }
}