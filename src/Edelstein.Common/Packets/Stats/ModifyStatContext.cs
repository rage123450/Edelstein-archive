using Edelstein.Database.Entities;
using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets.Stats
{
    public class ModifyStatContext : IEncodable
    {
        private readonly Character _character;

        private ModifyStatType _flag = 0;

        public byte Skin
        {
            get => _character.Skin;
            set
            {
                _flag |= ModifyStatType.Skin;
                _character.Skin = value;
            }
        }

        public int Face
        {
            get => _character.Face;
            set
            {
                _flag |= ModifyStatType.Face;
                _character.Face = value;
            }
        }

        public int Hair
        {
            get => _character.Hair;
            set
            {
                _flag |= ModifyStatType.Hair;
                _character.Hair = value;
            }
        }

        // TODO: Pet stuff

        public byte Level
        {
            get => _character.Level;
            set
            {
                _flag |= ModifyStatType.Level;
                _character.Level = value;
            }
        }

        public short Job
        {
            get => _character.Job;
            set
            {
                _flag |= ModifyStatType.Job;
                _character.Job = value;
            }
        }

        public short STR
        {
            get => _character.STR;
            set
            {
                _flag |= ModifyStatType.STR;
                _character.STR = value;
            }
        }

        public short DEX
        {
            get => _character.DEX;
            set
            {
                _flag |= ModifyStatType.DEX;
                _character.DEX = value;
            }
        }

        public short INT
        {
            get => _character.INT;
            set
            {
                _flag |= ModifyStatType.INT;
                _character.INT = value;
            }
        }

        public short LUK
        {
            get => _character.LUK;
            set
            {
                _flag |= ModifyStatType.LUK;
                _character.LUK = value;
            }
        }

        public int HP
        {
            get => _character.HP;
            set
            {
                _flag |= ModifyStatType.HP;
                _character.HP = value;
            }
        }

        public int MaxHP
        {
            get => _character.MaxHP;
            set
            {
                _flag |= ModifyStatType.MaxHP;
                _character.MaxHP = value;
            }
        }

        public int MP
        {
            get => _character.MP;
            set
            {
                _flag |= ModifyStatType.MP;
                _character.MP = value;
            }
        }

        public int MaxMP
        {
            get => _character.MaxMP;
            set
            {
                _flag |= ModifyStatType.MaxMP;
                _character.MaxMP = value;
            }
        }

        public short AP
        {
            get => _character.AP;
            set
            {
                _flag |= ModifyStatType.AP;
                _character.AP = value;
            }
        }

        public short SP
        {
            get => _character.SP;
            set
            {
                _flag |= ModifyStatType.SP;
                _character.SP = value;
            }
        }

        public int EXP
        {
            get => _character.EXP;
            set
            {
                _flag |= ModifyStatType.EXP;
                _character.EXP = value;
            }
        }

        public short POP
        {
            get => _character.POP;
            set
            {
                _flag |= ModifyStatType.POP;
                _character.POP = value;
            }
        }

        public int Money
        {
            get => _character.Money;
            set
            {
                _flag |= ModifyStatType.Money;
                _character.Money = value;
            }
        }

        public int TempEXP
        {
            get => _character.TempEXP;
            set
            {
                _flag |= ModifyStatType.TempEXP;
                _character.TempEXP = value;
            }
        }

        public ModifyStatContext(Character character)
        {
            _character = character;
        }

        public void Encode(OutPacket packet)
        {
            packet.Encode<int>((int) _flag);

            if ((_flag & ModifyStatType.Skin) != 0) packet.Encode<byte>(Skin);
            if ((_flag & ModifyStatType.Face) != 0) packet.Encode<int>(Face);
            if ((_flag & ModifyStatType.Hair) != 0) packet.Encode<int>(Hair);

            if ((_flag & ModifyStatType.Pet) != 0) packet.Encode<long>(0);
            if ((_flag & ModifyStatType.Pet2) != 0) packet.Encode<long>(0);
            if ((_flag & ModifyStatType.Pet3) != 0) packet.Encode<long>(0);

            if ((_flag & ModifyStatType.Level) != 0) packet.Encode<byte>(Level);
            if ((_flag & ModifyStatType.Job) != 0) packet.Encode<short>(Job);
            if ((_flag & ModifyStatType.STR) != 0) packet.Encode<short>(STR);
            if ((_flag & ModifyStatType.DEX) != 0) packet.Encode<short>(DEX);
            if ((_flag & ModifyStatType.INT) != 0) packet.Encode<short>(INT);
            if ((_flag & ModifyStatType.LUK) != 0) packet.Encode<short>(LUK);

            if ((_flag & ModifyStatType.HP) != 0) packet.Encode<int>(HP);
            if ((_flag & ModifyStatType.MaxHP) != 0) packet.Encode<int>(MaxHP);
            if ((_flag & ModifyStatType.MP) != 0) packet.Encode<int>(MP);
            if ((_flag & ModifyStatType.MaxMP) != 0) packet.Encode<int>(MaxMP);

            if ((_flag & ModifyStatType.AP) != 0) packet.Encode<short>(AP);
            if ((_flag & ModifyStatType.SP) != 0)
            {
                if (Job / 1000 != 3 && Job / 100 != 22 && Job != 2001)
                    packet.Encode<short>(SP);
                else packet.Encode<byte>(0);
            }

            if ((_flag & ModifyStatType.EXP) != 0) packet.Encode<int>(EXP);
            if ((_flag & ModifyStatType.POP) != 0) packet.Encode<short>(POP);

            if ((_flag & ModifyStatType.Money) != 0) packet.Encode<int>(Money);
            if ((_flag & ModifyStatType.TempEXP) != 0) packet.Encode<int>(TempEXP);
        }
    }
}