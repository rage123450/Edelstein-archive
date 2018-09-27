using Edelstein.Database.Entities;
using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets.Stats
{
    public class ModifyStatContext : IEncodable
    {
        private readonly Character _character;

        private ModifyStatType flag = 0;

        public byte Skin
        {
            get => _character.Skin;
            set
            {
                flag |= ModifyStatType.Skin;
                _character.Skin = value;
            }
        }

        public int Face
        {
            get => _character.Face;
            set
            {
                flag |= ModifyStatType.Face;
                _character.Face = value;
            }
        }

        public int Hair
        {
            get => _character.Hair;
            set
            {
                flag |= ModifyStatType.Hair;
                _character.Hair = value;
            }
        }

        // TODO: Pet stuff

        public byte Level
        {
            get => _character.Level;
            set
            {
                flag |= ModifyStatType.Level;
                _character.Level = value;
            }
        }

        public short Job
        {
            get => _character.Job;
            set
            {
                flag |= ModifyStatType.Job;
                _character.Job = value;
            }
        }

        public short STR
        {
            get => _character.STR;
            set
            {
                flag |= ModifyStatType.STR;
                _character.STR = value;
            }
        }

        public short DEX
        {
            get => _character.DEX;
            set
            {
                flag |= ModifyStatType.DEX;
                _character.DEX = value;
            }
        }

        public short INT
        {
            get => _character.INT;
            set
            {
                flag |= ModifyStatType.INT;
                _character.INT = value;
            }
        }

        public short LUK
        {
            get => _character.LUK;
            set
            {
                flag |= ModifyStatType.LUK;
                _character.LUK = value;
            }
        }

        public int HP
        {
            get => _character.HP;
            set
            {
                flag |= ModifyStatType.HP;
                _character.HP = value;
            }
        }

        public int MaxHP
        {
            get => _character.MaxHP;
            set
            {
                flag |= ModifyStatType.MaxHP;
                _character.MaxHP = value;
            }
        }

        public int MP
        {
            get => _character.MP;
            set
            {
                flag |= ModifyStatType.MP;
                _character.MP = value;
            }
        }

        public int MaxMP
        {
            get => _character.MaxMP;
            set
            {
                flag |= ModifyStatType.MaxMP;
                _character.MaxMP = value;
            }
        }

        public short AP
        {
            get => _character.AP;
            set
            {
                flag |= ModifyStatType.AP;
                _character.AP = value;
            }
        }

        public short SP
        {
            get => _character.SP;
            set
            {
                flag |= ModifyStatType.SP;
                _character.SP = value;
            }
        }

        public int EXP
        {
            get => _character.EXP;
            set
            {
                flag |= ModifyStatType.EXP;
                _character.EXP = value;
            }
        }

        public short POP
        {
            get => _character.POP;
            set
            {
                flag |= ModifyStatType.POP;
                _character.POP = value;
            }
        }

        public ModifyStatContext(Character character)
        {
            _character = character;
        }

        public void Encode(OutPacket packet)
        {
            packet.Encode<int>((int) flag);

            if ((flag & ModifyStatType.Skin) != 0) packet.Encode<byte>(Skin);
            if ((flag & ModifyStatType.Face) != 0) packet.Encode<int>(Face);
            if ((flag & ModifyStatType.Hair) != 0) packet.Encode<int>(Hair);

            if ((flag & ModifyStatType.Pet) != 0) packet.Encode<long>(0);
            if ((flag & ModifyStatType.Pet2) != 0) packet.Encode<long>(0);
            if ((flag & ModifyStatType.Pet3) != 0) packet.Encode<long>(0);

            if ((flag & ModifyStatType.Level) != 0) packet.Encode<byte>(Level);
            if ((flag & ModifyStatType.Job) != 0) packet.Encode<short>(Job);
            if ((flag & ModifyStatType.STR) != 0) packet.Encode<short>(STR);
            if ((flag & ModifyStatType.DEX) != 0) packet.Encode<short>(DEX);
            if ((flag & ModifyStatType.INT) != 0) packet.Encode<short>(INT);
            if ((flag & ModifyStatType.LUK) != 0) packet.Encode<short>(LUK);

            if ((flag & ModifyStatType.HP) != 0) packet.Encode<int>(HP);
            if ((flag & ModifyStatType.MaxHP) != 0) packet.Encode<int>(MaxHP);
            if ((flag & ModifyStatType.MP) != 0) packet.Encode<int>(MP);
            if ((flag & ModifyStatType.MaxMP) != 0) packet.Encode<int>(MaxMP);

            if ((flag & ModifyStatType.AP) != 0) packet.Encode<short>(AP);
            if ((flag & ModifyStatType.SP) != 0)
            {
                if (Job / 1000 != 3 && Job / 100 != 22 && Job != 2001)
                    packet.Encode<short>(SP);
                else packet.Encode<byte>(0);
            }

            if ((flag & ModifyStatType.EXP) != 0) packet.Encode<int>(EXP);
            if ((flag & ModifyStatType.POP) != 0) packet.Encode<short>(POP);

            if ((flag & ModifyStatType.EXP) != 0) packet.Encode<int>(EXP);
            if ((flag & ModifyStatType.POP) != 0) packet.Encode<short>(POP);
        }
    }
}