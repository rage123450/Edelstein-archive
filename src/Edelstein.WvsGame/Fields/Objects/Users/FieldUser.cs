using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using CSharpx;
using Edelstein.Common.Packets;
using Edelstein.Common.Packets.Messages;
using Edelstein.Common.Packets.Stats;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Types;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Conversations;
using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Conversations.Speakers;
using Edelstein.WvsGame.Fields.Objects.Users.Effects;
using Edelstein.WvsGame.Fields.Objects.Users.Stats;
using Edelstein.WvsGame.Interactions;
using Edelstein.WvsGame.Packets;
using Edelstein.WvsGame.Sockets;
using Edelstein.WvsGame.Utils;

namespace Edelstein.WvsGame.Fields.Objects.Users
{
    public partial class FieldUser : FieldLife, IUpdateable
    {
        public override FieldObjType Type => FieldObjType.User;
        
        public GameClientSocket Socket { get; set; }
        public Character Character { get; set; }

        public ConversationContext ConversationContext { get; set; }

        public ForcedStat ForcedStat { get; }
        public BasicStat BasicStat { get; }
        public SecondaryStat SecondaryStat { get; }
        public TemporaryStat TemporaryStat { get; }

        public int? PortableChairID { get; set; }
        public int? CompletedSetItemID { get; set; }

        private string _adBoard;

        public string ADBoard
        {
            get => _adBoard;
            set
            {
                _adBoard = value;

                using (var p = new OutPacket(GameSendOperations.UserADBoard))
                {
                    var result = _adBoard != null;

                    p.Encode<int>(ID);
                    p.Encode<bool>(result);
                    if (result) p.Encode<string>(_adBoard);
                    Field?.BroadcastPacket(p);
                }
            }
        }

        private Dialogue _dialogue;

        public Dialogue Dialogue
        {
            get => _dialogue;
            set
            {
                if (value != null && _dialogue != null)
                    return;

                _dialogue = value;
                if (_dialogue != null)
                    SendPacket(_dialogue.GetCreatePacket());
            }
        }

        public FieldUser(GameClientSocket socket, Character character)
        {
            Socket = socket;
            Character = character;

            ForcedStat = new ForcedStat();
            BasicStat = new BasicStat(this);
            SecondaryStat = new SecondaryStat(this);
            TemporaryStat = new TemporaryStat();
            ValidateStat();
        }

        public async Task Update(DateTime now)
        {
            if (!Socket.IsInstantiated) return;

            var expiredStats = TemporaryStat.Entries.Values
                .Where(s => !s.Permanent)
                .Where(i => (now - i.DateExpire).Milliseconds >= 0)
                .ToList();
            var expiredItems = Character.Inventories
                .SelectMany(i => i.Items)
                .Where(i => i.DateExpire.HasValue)
                .Where(i => (now - i.DateExpire.Value).Milliseconds >= 0)
                .ToList();
            var expiredSkills = Character.SkillRecords
                .Where(s => s.DateExpire.HasValue)
                .Where(s => (now - s.DateExpire.Value).Milliseconds >= 0)
                .ToList();

            if (expiredStats.Any())
                await ModifyTemporaryStat(s => expiredStats.ForEach(e => s.Reset(e.Type)));
            if (expiredItems.Any())
                await ModifyInventory(i => expiredItems.ForEach(e => i.Remove(e)));
            if (expiredSkills.Any())
                await ModifySkill(s => expiredSkills.ForEach(s.Remove));

            var itemTemplates = Socket.WvsGame.ItemTemplates;
            var expiredTemplates = expiredItems
                .Select(i => itemTemplates.Get(i.TemplateID))
                .ToList();
            var expiredCashTemplates = expiredTemplates.Where(t => t.Cash).ToList();
            var expiredGeneralTemplates = expiredTemplates.Except(expiredCashTemplates).ToList();

            if (expiredCashTemplates.Any())
                await Task.WhenAll(
                    expiredCashTemplates
                        .Select(t => Message(new CashItemExpireMessage(t.TemplateID)))
                );
            if (expiredGeneralTemplates.Any())
                await Message(new GeneralItemExpireMessage(expiredGeneralTemplates
                    .Select(t => t.TemplateID)
                    .ToList())
                );
            if (expiredSkills.Any())
                await Message(new SkillExpireMessage(expiredSkills.Select(s => s.Skill).ToList()));
        }

        public void ValidateStat()
        {
            BasicStat.Calculate();
            SecondaryStat.Calculate();

            if (Character.HP > BasicStat.MaxHP) ModifyStats(s => s.HP = BasicStat.MaxHP);
            if (Character.MP > BasicStat.MaxMP) ModifyStats(s => s.MP = BasicStat.MaxMP);
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

        public Task Effect(UserEffectType type, bool local = true, bool remote = true)
        {
            return Effect(new UserEffect(type), local);
        }

        public Task Effect(UserEffect effect, bool local = true, bool remote = true)
        {
            if (local)
            {
                using (var p = new OutPacket(GameSendOperations.UserEffectLocal))
                {
                    effect.Encode(p);
                    SendPacket(p);
                }
            }

            if (remote)
            {
                using (var p = new OutPacket(GameSendOperations.UserEffectRemote))
                {
                    p.Encode<int>(ID);
                    effect.Encode(p);
                    Field.BroadcastPacket(this, p);
                }
            }

            return Task.CompletedTask;
        }

        public async Task<bool> Prompt(Action<FieldUserSpeaker> action = null)
            => await Prompt((s1, s2) => action?.Invoke(s1));

        public async Task<bool> Prompt(Action<FieldUserSpeaker, NPCSpeaker> action = null)
        {
            var context = new ConversationContext(Socket);
            var conversation = new ActionConversation(context, action);

            return await ConversationManager<FieldUserSpeaker, NPCSpeaker>.Start(
                new FieldUserSpeaker(context, this),
                new NPCSpeaker(context, 901000, SpeakerParamType.NPCReplacedByNPC),
                conversation
            );
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

                p.Encode<Job>(Character.Job);
                Character.EncodeLook(p);

                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(CompletedSetItemID ?? 0);
                p.Encode<int>(PortableChairID ?? 0);

                p.Encode<Point>(Position);
                p.Encode<byte>(MoveAction);
                p.Encode<short>(Foothold);
                p.Encode<byte>(0);

                p.Encode<byte>(0);

                p.Encode<int>(0);
                p.Encode<int>(0);
                p.Encode<int>(0);

                p.Encode<byte>(0);

                if (ADBoard != null)
                {
                    p.Encode<bool>(true);
                    p.Encode<string>(ADBoard);
                }
                else p.Encode<bool>(false);

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