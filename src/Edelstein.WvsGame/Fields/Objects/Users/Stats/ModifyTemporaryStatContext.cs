using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Network.Packets;
using MoreLinq.Extensions;

namespace Edelstein.WvsGame.Fields.Objects.Users.Stats
{
    public class ModifyTemporaryStatContext
    {
        private readonly FieldUser _user;
        public readonly List<TemporaryStat> ResetStats;
        public readonly List<TemporaryStat> SetStats;

        public ModifyTemporaryStatContext(FieldUser user)
        {
            _user = user;
            ResetStats = new List<TemporaryStat>();
            SetStats = new List<TemporaryStat>();
        }

        public void Set(TemporaryStatType type, int templateID, short option, DateTime dateExpire)
        {
            Reset(type);
            SetStats.Add(new TemporaryStat
            {
                Type = type,
                TemplateID = templateID,
                Option = option,
                DateExpire = dateExpire
            });
        }

        public void Reset(TemporaryStatType type)
        {
            ResetStats.AddRange(
                _user.TemporaryStat
                    .Where(s => s.Key == type)
                    .Select(s => s.Value)
                    .ToList()
            );
        }

        public static void EncodeSetForLocal(OutPacket packet, ICollection<TemporaryStat> stats)
        {
            var mask = new int[4];

            stats.ForEach(s =>
            {
                var flag = 1 << ((int) s.Type % 0x20);
                var index = (int) s.Type / 32;
                mask[index] |= flag;
            });

            for (var i = 4; i > 0; i--) packet.Encode<int>(mask[i - 1]);

            var dictionary = stats.ToDictionary(s => s.Type, s => s);

            dictionary.GetValueOrDefault(TemporaryStatType.PAD)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.PDD)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MAD)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MDD)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.ACC)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.EVA)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Craft)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Speed)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Jump)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.EMHP)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.EMMP)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.EMHP)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.EPAD)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.EPDD)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.EMDD)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MagicGuard)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.DarkSight)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Booster)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.PowerGuard)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Guard)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SafetyDamage)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SafetyAbsorb)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MaxHP)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MaxMP)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Invincible)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SoulArrow)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Stun)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Poison)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Seal)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Darkness)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.ComboCounter)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.WeaponCharge)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.DragonBlood)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.HolySymbol)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MesoUp)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.ShadowPartner)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.PickPocket)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MesoGuard)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Thaw)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Weakness)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Curse)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Slow)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Morph)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Ghost)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Regen)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.BasicStatUp)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Stance)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SharpEyes)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.ManaReflection)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Attract)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SpiritJavelin)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Infinity)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Holyshield)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.HamString)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Blind)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Concentration)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.BanMap)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MaxLevelBuff)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Barrier)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.DojangShield)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.ReverseInput)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MesoUpByItem)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.ItemUpByItem)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.RespectPImmune)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.RespectMImmune)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.DefenseAtt)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.DefenseState)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.DojangBerserk)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.DojangInvincible)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Spark)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SoulMasterFinal)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.WindBreakerFinal)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.ElementalReset)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.WindWalk)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.EventRate)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.ComboAbilityBuff)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.ComboDrain)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.ComboBarrier)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.BodyPressure)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SmartKnockback)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.RepeatEffect)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.ExpBuffRate)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.IncEffectHPPotion)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.IncEffectMPPotion)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.StopPortion)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.StopMotion)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Fear)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.EvanSlow)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MagicShield)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MagicResistance)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SoulStone)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Flying)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Frozen)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.AssistCharge)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Enrage)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SuddenDeath)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.NotDamaged)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.FinalCut)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.ThornsEffect)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SwallowAttackDamage)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MorewildDamageUp)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Mine)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Cyclone)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SwallowCritical)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SwallowMaxMP)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SwallowDefence)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SwallowEvasion)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Conversion)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Revive)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Sneak)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Mechanic)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Aura)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.DarkAura)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.BlueAura)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.YellowAura)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SuperBody)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.MorewildMaxHP)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Dice)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.BlessingArmor)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.DamR)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.TeleportMasteryOn)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.CombatOrders)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.Beholder)?.Encode(packet);
            dictionary.GetValueOrDefault(TemporaryStatType.SummonBomb)?.Encode(packet);

            packet.Encode<byte>(0); // nDefenseAtt
            packet.Encode<byte>(0); // nDefenseState

            if (dictionary.GetValueOrDefault(TemporaryStatType.SwallowBuff) != null)
                packet.Encode<byte>(0);

            if (dictionary.GetValueOrDefault(TemporaryStatType.Dice) != null)
                for (var i = 0; i < 22; i++)
                    packet.Encode<int>(0);

            if (dictionary.GetValueOrDefault(TemporaryStatType.BlessingArmor) != null)
                packet.Encode<int>(0);
        }

        public static void EncodeSetForRemote(ICollection<TemporaryStat> stats)
        {
        }

        public static void EncodeResetForLocal(OutPacket packet, ICollection<TemporaryStat> stats)
        {
            var mask = new int[4];

            stats.ForEach(s =>
            {
                var flag = 1 << ((int) s.Type % 0x20);
                var index = (int) s.Type / 32;
                mask[index] |= flag;
            });

            for (var i = 4; i > 0; i--) packet.Encode<int>(mask[i - 1]);
        }
    }
}