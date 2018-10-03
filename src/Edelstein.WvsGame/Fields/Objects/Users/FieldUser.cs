using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Edelstein.Common.Packets;
using Edelstein.Common.Packets.Inventory;
using Edelstein.Common.Packets.Messages;
using Edelstein.Common.Packets.Stats;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Inventory;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Conversations;
using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Fields.Movements;
using Edelstein.WvsGame.Fields.Objects.Drops;
using Edelstein.WvsGame.Fields.Objects.Users.Stats;
using Edelstein.WvsGame.Packets;
using Edelstein.WvsGame.Sockets;
using MoreLinq;

namespace Edelstein.WvsGame.Fields.Objects.Users
{
    public class FieldUser : FieldObject
    {
        public GameClientSocket Socket { get; set; }
        public Character Character { get; set; }

        public ConversationContext ConversationContext { get; set; }

        public BasicStat BasicStat { get; }
        public SecondaryStat SecondaryStat { get; }
        public TemporaryStat TemporaryStat { get; }

        public IDictionary<TemporaryStatType, Timer> TemporaryStatTimers;

        public FieldUser(GameClientSocket socket, Character character)
        {
            Socket = socket;
            Character = character;

            BasicStat = new BasicStat(this);
            SecondaryStat = new SecondaryStat(this);
            TemporaryStat = new TemporaryStat();
            CalculateStat();

            TemporaryStatTimers = new Dictionary<TemporaryStatType, Timer>();
        }

        public void CalculateStat()
        {
            BasicStat.Calculate();
            SecondaryStat.Calculate();
        }

        public Task ModifyStats(Action<ModifyStatContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyStatContext(Character);

            action?.Invoke(context);
            CalculateStat();
            using (var p = new OutPacket(GameSendOperations.StatChanged))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(false);
                p.Encode<bool>(false);
                return SendPacket(p);
            }
        }

        public Task ModifyInventory(Action<ModifyInventoryContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyInventoryContext(Character);
            var equipped = Character.GetInventory(ItemInventoryType.Equip).Items
                .Where(i => i.Slot < 0)
                .Select(i => i.Slot)
                .ToList();

            action?.Invoke(context);
            using (var p = new OutPacket(GameSendOperations.InventoryOperation))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(false);
                SendPacket(p);
            }

            var newEquipped = Character.GetInventory(ItemInventoryType.Equip).Items
                .Where(i => i.Slot < 0)
                .Select(i => i.Slot)
                .ToList();

            if (equipped.Except(newEquipped).Any() ||
                newEquipped.Except(equipped).Any())
            {
                CalculateStat();
                using (var p = new OutPacket(GameSendOperations.UserAvatarModified))
                {
                    p.Encode<int>(ID);
                    p.Encode<byte>(0x1); // Flag
                    Character.EncodeLook(p);
                    p.Encode<bool>(false); // bCouple
                    p.Encode<bool>(false); // bFriendship
                    p.Encode<bool>(false); // Marriage
                    p.Encode<int>(0); // nCompletedSetItemID

                    Field.BroadcastPacket(this, p);
                }
            }

            return Task.CompletedTask;
        }

        public Task ModifyTemporaryStat(Action<ModifyTemporaryStatContext> action = null)
        {
            var context = new ModifyTemporaryStatContext(this);

            action?.Invoke(context);

            if (context.ResetOperations.Count > 0)
            {
                context.ResetOperations.ForEach(s =>
                {
                    if (!TemporaryStatTimers.ContainsKey(s.Type)) return;
                    TemporaryStatTimers[s.Type].Stop();
                    TemporaryStatTimers.Remove(s.Type);
                });

                using (var p = new OutPacket(GameSendOperations.TemporaryStatReset))
                {
                    TemporaryStat.EncodeMask(p, context.ResetOperations);
                    p.Encode<byte>(0); // IsMovementAffectingStat
                    SendPacket(p);
                }

                using (var p = new OutPacket(GameSendOperations.UserTemporaryStatReset))
                {
                    p.Encode<int>(ID);
                    TemporaryStat.EncodeMask(p, context.ResetOperations);
                    Field.BroadcastPacket(this, p);
                }
            }

            if (context.SetOperations.Count > 0)
            {
                context.SetOperations
                    .Where(s => !s.Permanent)
                    .GroupBy(s => s.DateExpire.Millisecond)
                    .ForEach(g =>
                    {
                        var expire = g.First().DateExpire;
                        var timer = new Timer((expire - DateTime.Now).TotalMilliseconds)
                        {
                            AutoReset = false
                        };

                        timer.Elapsed += (sender, args) =>
                        {
                            ModifyTemporaryStat(ts => { g.ForEach(s => { ts.Reset(s.Type); }); });
                        };
                        timer.Start();

                        g.ForEach(s => { TemporaryStatTimers[s.Type] = timer; });
                    });

                using (var p = new OutPacket(GameSendOperations.TemporaryStatSet))
                {
                    TemporaryStat.EncodeForLocal(p, context.SetOperations);
                    p.Encode<short>(0); // tDelay
                    p.Encode<byte>(0); // IsMovementAffectingStat
                    SendPacket(p);
                }

                using (var p = new OutPacket(GameSendOperations.UserTemporaryStatSet))
                {
                    p.Encode<int>(ID);
                    TemporaryStat.EncodeForRemote(p, context.SetOperations);
                    p.Encode<short>(0); // tDelay
                    Field.BroadcastPacket(this, p);
                }
            }

            if (context.ResetOperations.Count > 0 ||
                context.SetOperations.Count > 0)
                CalculateStat();
            return Task.CompletedTask;
        }

        public Task Message(string text)
        {
            return Message(new SystemMessage(text));
        }

        public Task Message(Message message)
        {
            using (var p = new OutPacket(GameSendOperations.Message))
            {
                message.Encode(p);
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
                case GameRecvOperations.UserSelectNpc:
                    OnUserSelectNPC(packet);
                    break;
                case GameRecvOperations.UserScriptMessageAnswer:
                    OnUserScriptMessageAnswer(packet);
                    break;
                case GameRecvOperations.UserGatherItemRequest:
                    OnUserGatherItemRequest(packet);
                    break;
                case GameRecvOperations.UserSortItemRequest:
                    OnUserSortItemRequest(packet);
                    break;
                case GameRecvOperations.UserChangeSlotPositionRequest:
                    OnUserChangeSlotPositionRequest(packet);
                    break;
                case GameRecvOperations.UserAbilityUpRequest:
                    OnUserAbilityUpRequest(packet);
                    break;
                case GameRecvOperations.UserAbilityMassUpRequest:
                    OnUserAbilityMassUpRequest(packet);
                    break;
                case GameRecvOperations.UserDropMoneyRequest:
                    OnUserDropMoneyRequest(packet);
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
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Message("An error has occured while executing that command.");
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

        private void OnUserSelectNPC(InPacket packet)
        {
            var objectID = packet.Decode<int>();

            if (Field.GetObject(objectID) is FieldNPC npc)
                Socket.WvsGame.NPCConversationManager.Start(this, npc);
        }

        private void OnUserScriptMessageAnswer(InPacket packet)
        {
            ConversationContext?.Answers.Add(new ConversationAnswer());
        }

        private void OnUserGatherItemRequest(InPacket packet)
        {
            packet.Decode<int>();

            var inventoryType = (ItemInventoryType) packet.Decode<byte>();
            var inventoryCopy = Character.GetInventory(inventoryType).Items
                .Where(i => i.Slot > 0)
                .OrderBy(i => i.Slot)
                .ToList();
            short slot = 1;

            ModifyInventory(i =>
            {
                inventoryCopy.ForEach(i.Remove);
                inventoryCopy.ForEach(item => item.Slot = slot++);
                inventoryCopy.ForEach(i.Set);
            }, true);

            using (var p = new OutPacket(GameSendOperations.GatherItemResult))
            {
                p.Encode<bool>(false);
                p.Encode<byte>((byte) inventoryType);
                SendPacket(p);
            }
        }

        private void OnUserSortItemRequest(InPacket packet)
        {
            packet.Decode<int>();

            var inventoryType = (ItemInventoryType) packet.Decode<byte>();
            var inventoryCopy = Character.GetInventory(inventoryType).Items
                .Where(i => i.Slot > 0)
                .OrderBy(i => i.Slot)
                .ToList();

            ModifyInventory(i =>
            {
                inventoryCopy.ForEach(i.Remove);
                inventoryCopy = inventoryCopy.OrderBy(item => item.TemplateID).ToList();
                inventoryCopy.ForEach(i.Add);
            }, true);

            using (var p = new OutPacket(GameSendOperations.SortItemResult))
            {
                p.Encode<bool>(false);
                p.Encode<byte>((byte) inventoryType);
                SendPacket(p);
            }
        }

        private void OnUserChangeSlotPositionRequest(InPacket packet)
        {
            packet.Decode<int>();

            var inventoryType = (ItemInventoryType) packet.Decode<byte>();
            var fromSlot = packet.Decode<short>();
            var toSlot = packet.Decode<short>();

            packet.Decode<short>();

            if (toSlot == 0)
            {
                ModifyInventory(i =>
                {
                    var item = Character.GetInventory(inventoryType).Items
                        .Single(ii => ii.Slot == fromSlot);
                    var drop = new FieldDropItem(item) {X = X, Y = Y};

                    i.Remove(item);
                    Field.Enter(drop, () => drop.GetEnterFieldPacket(0x1, this));
                }, true);
                return;
            }

            ModifyInventory(i => i.Move(inventoryType, fromSlot, toSlot), true);
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

        private void OnUserDropMoneyRequest(InPacket packet)
        {
            packet.Decode<int>();
            var money = packet.Decode<int>();

            ModifyStats(s =>
            {
                if (s.Money < money) return;
                var drop = new FieldDropMoney(money) {X = X, Y = Y};

                s.Money -= money;
                Field.Enter(drop, () => drop.GetEnterFieldPacket(0x1, this));
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

                p.Encode<int>(user.ID);
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

                var chairs = c.Inventories
                    .SelectMany(i => i.Items)
                    .Select(i => i.TemplateID)
                    .Where(i => i / 10000 == 301)
                    .ToList();
                p.Encode<int>(chairs.Count);
                chairs.ForEach(i => p.Encode<int>(i));
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

                TemporaryStat.EncodeForRemote(p, TemporaryStat.Entries.Values);

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