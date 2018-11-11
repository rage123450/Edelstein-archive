using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Edelstein.Common.Packets;
using Edelstein.Common.Packets.Inventory;
using Edelstein.Common.Packets.Skills;
using Edelstein.Common.Packets.Stats;
using Edelstein.Database.Entities.Inventory;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Fields.Objects.Users.Stats;
using Edelstein.WvsGame.Packets;
using MoreLinq.Extensions;

namespace Edelstein.WvsGame.Fields.Objects.Users
{
    public partial class FieldUser
    {
        public Task ModifyStats(Action<ModifyStatContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyStatContext(Character);

            action?.Invoke(context);
            ValidateStat();

            if (!Socket.IsInstantiated) return Task.CompletedTask;
            using (var p = new OutPacket(GameSendOperations.StatChanged))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(false);
                p.Encode<bool>(false);
                return SendPacket(p);
            }
        }

        public Task ModifyForcedStats(Action<ModifyForcedStatContext> action = null)
        {
            var context = new ModifyForcedStatContext(ForcedStat);

            action?.Invoke(context);
            ValidateStat();

            if (!Socket.IsInstantiated) return Task.CompletedTask;
            using (var p = new OutPacket(GameSendOperations.ForcedStatSet))
            {
                context.Encode(p);
                return SendPacket(p);
            }
        }

        public Task ResetForcedStats()
        {
            ForcedStat.Clear();
            ValidateStat();

            return SendPacket(new OutPacket(GameSendOperations.ForcedStatReset));
        }

        public Task ModifyInventory(Action<ModifyInventoryContext> action = null, bool exclRequest = false)
        {
            var context = new ModifyInventoryContext(Character);
            var equipped = Character.GetInventory(ItemInventoryType.Equip).Items
                .Where(i => i.Position < 0)
                .Select(i => i.TemplateID)
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
                .Where(i => i.Position < 0)
                .Select(i => i.TemplateID)
                .ToList();

            if (equipped.Except(newEquipped).Any() ||
                newEquipped.Except(equipped).Any())
            {
                ValidateStat();
                using (var p = new OutPacket(GameSendOperations.UserAvatarModified))
                {
                    p.Encode<int>(ID);
                    p.Encode<byte>(0x1); // Flag
                    Character.EncodeLook(p);
                    p.Encode<bool>(false); // bCouple
                    p.Encode<bool>(false); // bFriendship
                    p.Encode<bool>(false); // Marriage
                    p.Encode<int>(CompletedSetItemID ?? 0);

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
                ValidateStat();
            return Task.CompletedTask;
        }

        public Task ModifySkill(Action<ModifySkillContext> action = null, bool exclRequest = false)
        {
            var context = new ModifySkillContext(Character);

            action?.Invoke(context);
            ValidateStat();
            using (var p = new OutPacket(GameSendOperations.ChangeSkillRecordResult))
            {
                p.Encode<bool>(exclRequest);
                context.Encode(p);
                p.Encode<bool>(true);
                return SendPacket(p);
            }
        }
    }
}