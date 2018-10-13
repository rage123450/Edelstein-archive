using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Common.Packets;
using Edelstein.Common.Packets.Stats;
using Edelstein.Database.Entities.Inventory;
using Edelstein.Network.Packets;
using Edelstein.Provider.Items.Consume;
using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Fields.Movements;
using Edelstein.WvsGame.Fields.Objects.Drops;
using Edelstein.WvsGame.Fields.Objects.Users.Stats;
using Edelstein.WvsGame.Packets;
using MoreLinq.Extensions;

namespace Edelstein.WvsGame.Fields.Objects.Users
{
    public partial class FieldUser
    {
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
                case GameRecvOperations.UserSitRequest:
                    OnUserSitRequest(packet);
                    break;
                case GameRecvOperations.UserPortableChairSitRequest:
                    OnUserPortableChairSitRequest(packet);
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
                case GameRecvOperations.UserShopRequest:
                case GameRecvOperations.UserTrunkRequest:
                case GameRecvOperations.UserEntrustedShopRequest:
                case GameRecvOperations.UserStoreBankRequest:
                case GameRecvOperations.UserParcelRequest:
                    return Dialogue?.OnPacket(this, operation, packet) ?? true;
                case GameRecvOperations.UserGatherItemRequest:
                    OnUserGatherItemRequest(packet);
                    break;
                case GameRecvOperations.UserSortItemRequest:
                    OnUserSortItemRequest(packet);
                    break;
                case GameRecvOperations.UserChangeSlotPositionRequest:
                    OnUserChangeSlotPositionRequest(packet);
                    break;
                case GameRecvOperations.UserStatChangeItemUseRequest:
                    OnUserStatChangeItemUseRequest(packet);
                    break;
                case GameRecvOperations.UserStatChangeItemCancelRequest:
                    OnUserStatChangeItemCancelRequest(packet);
                    break;
                case GameRecvOperations.UserAbilityUpRequest:
                    OnUserAbilityUpRequest(packet);
                    break;
                case GameRecvOperations.UserAbilityMassUpRequest:
                    OnUserAbilityMassUpRequest(packet);
                    break;
                case GameRecvOperations.UserSkillUpRequest:
                    OnUserSkillUpRequest(packet);
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

        private void OnUserSitRequest(InPacket packet)
        {
            var id = packet.Decode<short>();

            using (var p = new OutPacket(GameSendOperations.UserSitResult))
            {
                if (id < 0)
                {
                    PortableChairID = null;
                    p.Encode<byte>(0);
                }
                else
                {
                    p.Encode<byte>(1);
                    p.Encode<short>(id); // TODO: proper checks for this
                }

                SendPacket(p);
            }

            if (PortableChairID != null) return;

            using (var p = new OutPacket(GameSendOperations.UserSetActivePortableChair))
            {
                p.Encode<int>(ID);
                p.Encode<int>(0);
                Field.BroadcastPacket(this, p);
            }
        }

        private void OnUserPortableChairSitRequest(InPacket packet)
        {
            var templateID = packet.Decode<int>();

            if (Character.Inventories
                .SelectMany(i => i.Items)
                .Select(i => i.TemplateID)
                .All(i => i != templateID)) return;

            PortableChairID = templateID;
            ModifyStats(exclRequest: true);

            using (var p = new OutPacket(GameSendOperations.UserSetActivePortableChair))
            {
                p.Encode<int>(ID);
                p.Encode<int>(templateID);
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

            if (!(Field.GetObject(objectID) is FieldNPC npc)) return;

            var template = npc.Template;

            if (Socket.WvsGame.NPCShops.ContainsKey(template.TemplateID))
            {
                Dialogue = Socket.WvsGame.NPCShops[template.TemplateID];
                return;
            }

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
                .Where(i => i.Position > 0)
                .OrderBy(i => i.Position)
                .ToList();
            short pos = 1;

            ModifyInventory(i =>
            {
                inventoryCopy.ForEach(i.Remove);
                inventoryCopy.ForEach(item => item.Position = pos++);
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
                .Where(i => i.Position > 0)
                .OrderBy(i => i.Position)
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
                        .Single(ii => ii.Position == fromSlot);
                    var drop = new FieldDropItem(item) {X = X, Y = Y};

                    i.Remove(item);
                    Field.Enter(drop, () => drop.GetEnterFieldPacket(0x1, this));
                }, true);
                return;
            }

            ModifyInventory(i => i.Move(inventoryType, fromSlot, toSlot), true);
        }

        private void OnUserStatChangeItemUseRequest(InPacket packet)
        {
            packet.Decode<int>();

            var position = packet.Decode<short>();
            var templateID = packet.Decode<int>();
            var template = Socket.WvsGame.ItemTemplates.Get(templateID);

            if (!(template is StatChangeItemTemplate scTemplate)) return;

            var temporaryStats = new Dictionary<TemporaryStatType, short>();

            if (scTemplate.PAD > 0) temporaryStats.Add(TemporaryStatType.PAD, scTemplate.PAD);
            if (scTemplate.PDD > 0) temporaryStats.Add(TemporaryStatType.PDD, scTemplate.PDD);
            if (scTemplate.MAD > 0) temporaryStats.Add(TemporaryStatType.MAD, scTemplate.MAD);
            if (scTemplate.MDD > 0) temporaryStats.Add(TemporaryStatType.MDD, scTemplate.MDD);
            if (scTemplate.ACC > 0) temporaryStats.Add(TemporaryStatType.ACC, scTemplate.ACC);
            if (scTemplate.EVA > 0) temporaryStats.Add(TemporaryStatType.EVA, scTemplate.EVA);
            if (scTemplate.Craft > 0) temporaryStats.Add(TemporaryStatType.Craft, scTemplate.Craft);
            if (scTemplate.Speed > 0) temporaryStats.Add(TemporaryStatType.Speed, scTemplate.Speed);
            if (scTemplate.Jump > 0) temporaryStats.Add(TemporaryStatType.Jump, scTemplate.Jump);
            if (scTemplate.Morph > 0) temporaryStats.Add(TemporaryStatType.Morph, scTemplate.Morph);

            if (temporaryStats.Count > 0)
                ModifyTemporaryStat(ts => temporaryStats.ForEach(t =>
                    ts.Set(
                        t.Key,
                        -templateID,
                        t.Value,
                        DateTime.Now.AddMilliseconds(scTemplate.Time)
                    )
                ));

            if (!temporaryStats.ContainsKey(TemporaryStatType.Morph))
            {
                var incHP = 0;
                var incMP = 0;

                incHP += scTemplate.HP;
                incMP += scTemplate.MP;
                incHP += BasicStat.MaxHP * (scTemplate.HPr / 100);
                incMP += BasicStat.MaxMP * (scTemplate.MPr / 100);

                if (incHP > 0 || incMP > 0)
                {
                    ModifyStats(s =>
                    {
                        s.HP = Math.Min(BasicStat.MaxHP, s.HP + incHP);
                        s.MP = Math.Min(BasicStat.MaxMP, s.MP + incMP);
                    });
                }
            }

            ModifyStats(exclRequest: true);
        }

        private void OnUserStatChangeItemCancelRequest(InPacket packet)
        {
            var templateID = packet.Decode<int>();
            var template = Socket.WvsGame.ItemTemplates.Get(-templateID);

            if (template is StatChangeItemTemplate scTemplate &&
                scTemplate.NoCancelMouse) return;

            ModifyTemporaryStat(ts => ts.Reset(templateID));
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

        private void OnUserSkillUpRequest(InPacket packet)
        {
            packet.Decode<int>();
            var templateID = packet.Decode<int>();

            ModifyStats(s => s.SP--);
            ModifySkill(s => s.Add(Socket.WvsGame.SkillTemplates.Get(templateID)), true);
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
    }
}