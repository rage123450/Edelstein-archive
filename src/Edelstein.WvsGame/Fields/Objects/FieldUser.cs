using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Packets;
using Edelstein.Common.Packets.Stats;
using Edelstein.Database.Entities;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Fields.Movements;
using Edelstein.WvsGame.Packets;
using Edelstein.WvsGame.Sockets;
using MoreLinq;

namespace Edelstein.WvsGame.Fields.Objects
{
    public class FieldUser : FieldObject
    {
        public GameClientSocket Socket { get; set; }
        public Character Character { get; set; }

        public FieldUser(GameClientSocket socket, Character character)
        {
            Socket = socket;
            Character = character;
        }

        public Task ModifyStats(Action<ModifyStatContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyStatContext(Character);

            action?.Invoke(context);
            using (var p = new OutPacket(GameSendOperations.StatChanged))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(false);
                p.Encode<bool>(false);
                return SendPacket(p);
            }
        }

        public bool OnPacket(GameRecvOperations operation, InPacket packet)
        {
            switch (operation)
            {
                case GameRecvOperations.UserTransferFieldRequest:
                    OnUserTransferFieldRequest(packet);
                    break;
                case GameRecvOperations.UserMove:
                    OnUserMove(packet);
                    break;
                case GameRecvOperations.UserChat:
                    OnUserChat(packet);
                    break;
                case GameRecvOperations.UserEmotion:
                    OnUserEmotion(packet);
                    break;
                case GameRecvOperations.UserAbilityUpRequest:
                    OnUserAbilityUpRequest(packet);
                    break;
                case GameRecvOperations.UserAbilityMassUpRequest:
                    OnUserAbilityMassUpRequest(packet);
                    break;
                case GameRecvOperations.UserCharacterInfoRequest:
                    OnUserCharacterInfoRequest(packet);
                    break;
                default:
                    return false;
            }

            return true;
        }

        private void OnUserTransferFieldRequest(InPacket packet)
        {
            packet.Decode<byte>();
            packet.Decode<int>();

            var portalName = packet.Decode<string>();
            var portal = Field.Template.Portals.Values.Single(p => p.Name.Equals(portalName));
            var targetField = Socket.WvsGame.FieldFactory.Get(portal.ToMap);
            var targetPortal = targetField.Template.Portals.Values.Single(p => p.Name.Equals(portal.ToName));

            Character.FieldPortal = (byte) targetPortal.ID;
            targetField.Enter(this);
        }

        private void OnUserMove(InPacket packet)
        {
            packet.Decode<long>();
            packet.Decode<byte>();
            packet.Decode<long>();
            packet.Decode<int>();
            packet.Decode<int>();
            packet.Decode<int>();

            var movementPath = new MovementPath();

            movementPath.Decode(packet);

            using (var p = new OutPacket(GameSendOperations.UserMove))
            {
                p.Encode(ID);
                movementPath.Encode(p);

                X = movementPath.X;
                Y = movementPath.Y;
                MoveAction = movementPath.MoveActionLast;
                Foothold = movementPath.FHLast;
                Field.BroadcastPacket(this, p);
            }
        }

        private void OnUserChat(InPacket packet)
        {
            packet.Decode<int>();

            var message = packet.Decode<string>();
            var onlyBalloon = packet.Decode<bool>();

            if (message.StartsWith("!"))
            {
                try
                {
                    Socket.WvsGame.CommandRegistry.Process(
                        this,
                        message.Substring(1)
                    );
                }
                catch (Exception)
                {
                    // TODO: do something
                }

                return;
            }

            using (var p = new OutPacket(GameSendOperations.UserChat))
            {
                p.Encode<int>(ID);
                p.Encode<bool>(false);
                p.Encode<string>(message);
                p.Encode<bool>(onlyBalloon);
                Field.BroadcastPacket(p);
            }
        }

        private void OnUserEmotion(InPacket packet)
        {
            var emotion = packet.Decode<int>();
            var duration = packet.Decode<int>();
            var byItemOption = packet.Decode<bool>();

            using (var p = new OutPacket(GameSendOperations.UserEmotion))
            {
                p.Encode<int>(ID);
                p.Encode<int>(emotion);
                p.Encode<int>(duration);
                p.Encode<bool>(byItemOption);
                Field.BroadcastPacket(this, p);
            }
        }

        public OutPacket GetSetFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.SetField))
            {
                p.Encode<short>(0); // ClientOpt

                p.Encode<int>(0);
                p.Encode<int>(0);

                p.Encode<bool>(true); // sNotifierMessage._m_pStr
                p.Encode<bool>(!Socket.IsInstantiated);
                p.Encode<short>(0); // nNotifierCheck, loops

                if (!Socket.IsInstantiated)
                {
                    p.Encode<int>((int) Socket.Random.Seed1);
                    p.Encode<int>((int) Socket.Random.Seed2);
                    p.Encode<int>((int) Socket.Random.Seed3);

                    Character.EncodeData(p);

                    p.Encode<int>(0);
                    for (var i = 0; i < 3; i++) p.Encode<int>(0);
                }
                else
                {
                    p.Encode<byte>(0);
                    p.Encode<int>(Character.FieldID);
                    p.Encode<byte>(Character.FieldPortal);
                    p.Encode<int>(Character.HP);
                    p.Encode<bool>(false);
                }

                p.Encode<long>(0);
                return p;
            }
        }

        private void OnUserAbilityUpRequest(InPacket packet)
        {
            packet.Decode<int>();
            var type = (ModifyStatType) packet.Decode<int>();

            if (Character.AP > 0)
            {
                ModifyStats(s =>
                {
                    switch (type)
                    {
                        case ModifyStatType.STR:
                            s.STR++;
                            break;
                        case ModifyStatType.DEX:
                            s.DEX++;
                            break;
                        case ModifyStatType.INT:
                            s.INT++;
                            break;
                        case ModifyStatType.LUK:
                            s.LUK++;
                            break;
                    }

                    s.AP--;
                }, true);
            }
        }

        private void OnUserAbilityMassUpRequest(InPacket packet)
        {
            packet.Decode<int>();
            var count = packet.Decode<int>();
            var inc = new Dictionary<int, int>();

            for (var i = 0; i < count; i++)
                inc.Add(packet.Decode<int>(), packet.Decode<int>());

            var total = inc.Values.Sum();

            if (Character.AP < total) return;

            ModifyStats(s =>
            {
                inc.ForEach(p =>
                {
                    var type = (ModifyStatType) p.Key;
                    var value = p.Value;

                    switch (type)
                    {
                        case ModifyStatType.STR:
                            s.STR += Convert.ToInt16(value);
                            break;
                        case ModifyStatType.DEX:
                            s.DEX += Convert.ToInt16(value);
                            break;
                        case ModifyStatType.INT:
                            s.INT += Convert.ToInt16(value);
                            break;
                        case ModifyStatType.LUK:
                            s.LUK += Convert.ToInt16(value);
                            break;
                    }
                });

                s.AP -= Convert.ToInt16(total);
            }, true);
        }

        private void OnUserCharacterInfoRequest(InPacket packet)
        {
            packet.Decode<int>();
            var user = Field.GetUser(packet.Decode<int>());
            if (user == null) return;

            using (var p = new OutPacket(GameSendOperations.CharacterInfo))
            {
                var c = user.Character;

                p.Encode<int>(ID);
                p.Encode<byte>(c.Level);
                p.Encode<short>(c.Job);
                p.Encode<short>(c.POP);

                p.Encode<byte>(0);

                p.Encode<string>(""); // sCommunity
                p.Encode<string>(""); // sAlliance

                p.Encode<byte>(0);
                p.Encode<byte>(0);
                p.Encode<byte>(0); // TamingMobInfo
                p.Encode<byte>(0); // WishItemInfo

                p.Encode<int>(0); // MedalAchievementInfo
                p.Encode<short>(0);

                p.Encode<int>(0); // ChairItemInfo
                SendPacket(p);
            }
        }

        public override OutPacket GetEnterFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.UserEnterField))
            {
                p.Encode<int>(ID);

                p.Encode<byte>(Character.Level);
                p.Encode<string>(Character.Name);

                // Guild
                p.Encode<string>("");
                p.Encode<short>(0);
                p.Encode<byte>(0);
                p.Encode<short>(0);
                p.Encode<byte>(0);

                p.Encode<long>(0);
                p.Encode<long>(0);
                p.Encode<byte>(0);
                p.Encode<byte>(0);

                p.Encode<short>(Character.Job);
                Character.EncodeLook(p);

                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);

                p.Encode<short>(X);
                p.Encode<short>(Y);
                p.Encode<byte>(MoveAction);
                p.Encode<short>(Foothold);
                p.Encode<byte>(0);

                p.Encode<byte>(0);

                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);

                p.Encode<byte>(0);

                p.Encode<byte>(0);
                p.Encode<byte>(0);
                p.Encode<byte>(0);
                p.Encode<byte>(0);

                p.Encode<byte>(0);

                p.Encode<byte>(0);
                p.Encode<int>(0);
                return p;
            }
        }

        public override OutPacket GetLeaveFieldPacket()
        {
            using (var p = new OutPacket(GameSendOperations.UserLeaveField))
            {
                p.Encode<int>(ID);
                return p;
            }
        }

        public Task SendPacket(OutPacket packet) => Socket.SendPacket(packet);
    }
}