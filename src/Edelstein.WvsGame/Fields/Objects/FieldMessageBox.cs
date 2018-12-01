using System;
using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Packets;
using Edelstein.WvsGame.Utils;

namespace Edelstein.WvsGame.Fields.Objects
{
    public class FieldMessageBox : FieldObj, IUpdateable
    {
        private readonly int _templateID;
        private readonly string _hope;
        private readonly string _name;
        public readonly DateTime? _dateExpire;

        public FieldMessageBox(int templateId, string hope, string name, DateTime? dateExpire = null)
        {
            _templateID = templateId;
            _hope = hope;
            _name = name;
            _dateExpire = dateExpire;
        }

        public Task Update(DateTime now)
        {
            if (!_dateExpire.HasValue) return Task.CompletedTask;
            if ((now - _dateExpire.Value).Milliseconds < 0) return Task.CompletedTask;

            Field?.Leave(this);
            return Task.CompletedTask;
        }

        public override OutPacket GetEnterFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.MessageBoxEnterField))
            {
                p.Encode<int>(ID);
                p.Encode<int>(_templateID);
                p.Encode<string>(_hope);
                p.Encode<string>(_name);
                p.Encode<Point>(Position);
                return p;
            }
        }

        public override OutPacket GetLeaveFieldPacket() => GetLeaveFieldPacket(false);

        public OutPacket GetLeaveFieldPacket(bool fadeOut)
        {
            using (var p = new OutPacket(GameSendOperations.MessageBoxLeaveField))
            {
                p.Encode<bool>(fadeOut);
                p.Encode<int>(ID);
                return p;
            }
        }
    }
}