using Edelstein.Network.Packets;
using Edelstein.Provider.Reactors;
using Edelstein.WvsGame.Packets;

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