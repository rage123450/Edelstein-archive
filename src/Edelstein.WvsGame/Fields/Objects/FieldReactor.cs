using System.Linq;
using Edelstein.Network.Packets;
using Edelstein.Provider.Reactors;
using Edelstein.WvsGame.Fields.Objects.Users;
using Edelstein.WvsGame.Packets;
using MoreLinq;

namespace Edelstein.WvsGame.Fields.Objects
{
    public class FieldReactor : FieldObject
    {
        public ReactorTemplate Template { get; set; }
        private byte _state;
        private bool _flip;

        public FieldReactor(ReactorTemplate template)
        {
            Template = template;
            _state = 0;
            _flip = false;
        }

        public bool OnPacket(FieldUser controller, GameRecvOperations operation, InPacket packet)
        {
            switch (operation)
            {
                case GameRecvOperations.ReactorHit:
                    OnReactorHit(packet);
                    break;
                default:
                    return false;
            }

            return true;
        }

        private void OnReactorHit(InPacket packet)
        {
            packet.Decode<int>();
            var hitOption = packet.Decode<int>();
            var delay = packet.Decode<short>();
            packet.Decode<int>();

            var state = Template.States[_state];

            state.Events.ForEach(e =>
            {
                if (e.Type != 0) return;
                var newState = (byte) (_state + 1);

                if (newState < Template.StateCount) SetState(newState, delay);
                else Field.Leave(this);
            });
        }

        public void SetState(byte state, short delay = 0, byte properEventIdx = 0, byte unk3 = 0)
        {
            lock (this)
            {
                _state = state;

                using (var p = new OutPacket(GameSendOperations.ReactorChangeState))
                {
                    p.Encode<int>(ID);
                    p.Encode<byte>(_state);
                    p.Encode<short>(X);
                    p.Encode<short>(Y);

                    p.Encode<short>(delay);
                    p.Encode<byte>(properEventIdx);
                    p.Encode<byte>(unk3);

                    Field?.BroadcastPacket(p);
                }
            }
        }

        public override OutPacket GetEnterFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.ReactorEnterField))
            {
                p.Encode<int>(ID);
                p.Encode<int>(Template.TemplateID);
                p.Encode<byte>(_state);
                p.Encode<short>(X);
                p.Encode<short>(Y);
                p.Encode<bool>(_flip);
                p.Encode<string>("");
                return p;
            }
        }

        public override OutPacket GetLeaveFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.ReactorLeaveField))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(_state);
                p.Encode<short>(X);
                p.Encode<short>(Y);
                return p;
            }
        }
    }
}