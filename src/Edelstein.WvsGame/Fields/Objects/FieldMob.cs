using System;
using Edelstein.Network.Packets;
using Edelstein.Provider.Mobs;
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
                p.Encode<byte>(4);
                p.Encode<short>(Foothold);
                p.Encode<short>(Foothold);

                p.Encode<byte>(unchecked((byte) -2));

                p.Encode<byte>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                return p;
            }
        }

        public override OutPacket GetLeaveFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.MobEnterField))
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