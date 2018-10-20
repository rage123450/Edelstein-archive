using System;
using Edelstein.Network.Packets;
using Edelstein.Provider.Mobs;
using Edelstein.WvsGame.Fields.Movements;
using Edelstein.WvsGame.Fields.Objects.Users;
using Edelstein.WvsGame.Packets;

namespace Edelstein.WvsGame.Fields.Objects
{
    public class FieldMob : FieldObjectControlled
    {
        public MobTemplate Template { get; set; }

        public FieldMob(MobTemplate template)
        {
            Template = template;
        }

        public void Damage(FieldObject source, int damage)
        {
            Console.WriteLine("Total damage: " + damage);
        }
        
        public bool OnPacket(FieldUser controller, GameRecvOperations operation, InPacket packet)
        {
            switch (operation)
            {
                case GameRecvOperations.MobMove:
                    OnMobMove(packet);
                    break;
                default:
                    return false;
            }

            return true;
        }
        
        private void OnMobMove(InPacket packet)
        {
            var mobCtrlSN = packet.Decode<short>();
            var v85 = packet.Decode<byte>(); // v85 = nDistance | 4 * (v184 | 2 * ((unsigned __int8)retaddr | 2 * v72));
            var mobMoveStartResult = (v85 & 0xF) != 0;
            var BYTE3 = packet.Decode<byte>(); // BYTE3(v178)
            var v92 = packet.Decode<int>(); // v92 = v206
            var v8 = packet.Decode<byte>();
            
            var multiTargetForBall = packet.Decode<int>();
            for (var i = 0; i < multiTargetForBall; i++) packet.Decode<long>(); // int, int
            
            var randTimeforAreaAttack = packet.Decode<int>();
            for (var i = 0; i < randTimeforAreaAttack; i++) packet.Decode<int>();

            packet.Decode<int>(); // HackedCode
            packet.Decode<int>(); // idk
            packet.Decode<int>(); // HackedCodeCrc
            packet.Decode<int>(); // idk
            
            var movementPath = new MovementPath();

            movementPath.Decode(packet);
            X = movementPath.X;
            Y = movementPath.Y;
            MoveAction = movementPath.MoveActionLast;
            Foothold = movementPath.FHLast;

            using (var p = new OutPacket(GameSendOperations.MobCtrlAck))
            {
                p.Encode<int>(ID);
                p.Encode<short>(mobCtrlSN);
                p.Encode<bool>(mobMoveStartResult);
                p.Encode<short>(0); // nMP
                p.Encode<byte>(0); // SkillCommand
                p.Encode<byte>(0); // SLV
                Controller?.SendPacket(p);
            }

            using (var p = new OutPacket(GameSendOperations.MobMove))
            {
                p.Encode<int>(ID);
                p.Encode<bool>(mobMoveStartResult);
                p.Encode<byte>(BYTE3); // idk
                p.Encode<byte>(0); // idk
                p.Encode<byte>(0); // idk
                p.Encode<int>(v92); // idk

                p.Encode<int>(0); // MultiTargetForBall
                p.Encode<int>(0); // RandTimeforAreaAttack
                
                movementPath.Encode(p);
                
                if (Controller == null) Field.BroadcastPacket(p);
                else Field.BroadcastPacket(Controller, p);
            }
        }

        public override OutPacket GetEnterFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.MobEnterField))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(1);
                p.Encode<int>(Template.TemplateID);

                p.Encode<long>(0); // Temporary Stat
                p.Encode<long>(0); // Temporary Stat

                p.Encode<short>(X);
                p.Encode<short>(Y);
                p.Encode<byte>(MoveAction);
                p.Encode<short>(Foothold);
                p.Encode<short>(Foothold);

                p.Encode<byte>(unchecked((byte) -1));

                p.Encode<byte>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                return p;
            }
        }

        public override OutPacket GetLeaveFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.MobLeaveField))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(0);
                return p;
            }
        }

        public override OutPacket GetChangeControllerPacket(bool setAsController)
        {
            using (var p = new OutPacket(GameSendOperations.MobChangeController))
            {
                p.Encode<bool>(setAsController);
                p.Encode<int>(ID);

                if (setAsController)
                {
                    p.Encode<byte>(0); // nCalcDamageIndex
                    p.Encode<int>(Template.TemplateID);
                    
                    p.Encode<long>(0); // Temporary Stat
                    p.Encode<long>(0); // Temporary Stat

                    p.Encode<short>(X);
                    p.Encode<short>(Y);
                    p.Encode<byte>(4);
                    p.Encode<short>(Foothold);
                    p.Encode<short>(Foothold);

                    p.Encode<byte>(unchecked((byte) -2));

                    p.Encode<byte>(0);
                    p.Encode<int>(0);
                    p.Encode<int>(0);
                }
                return p;
            }
        }
    }
}