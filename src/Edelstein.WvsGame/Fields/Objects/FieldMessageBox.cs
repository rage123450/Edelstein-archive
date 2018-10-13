using Edelstein.Network.Packets;
using Edelstein.WvsGame.Packets;

namespace Edelstein.WvsGame.Fields.Objects
{
    public class FieldMessageBox : FieldObject
    {
        private readonly int _templateID;
        private readonly string _hope;
        private readonly string _name;

        public FieldMessageBox(int templateId, string hope, string name)
        {
            _templateID = templateId;
            _hope = hope;
            _name = name;
        }

        public override OutPacket GetEnterFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.MessageBoxEnterField))
            {
                p.Encode<int>(ID);
                p.Encode<int>(_templateID);
                p.Encode<string>(_hope);
                p.Encode<string>(_name);
                p.Encode<short>(X);
                p.Encode<short>(Y);
                return p;
            }
        }

        public override OutPacket GetLeaveFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.MessageBoxLeaveField))
            {
                p.Encode<bool>(true);
                p.Encode<int>(ID);
                return p;
            }
        }
    }
}