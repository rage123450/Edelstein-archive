using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Edelstein.Common.Packets;
using Edelstein.Common.Packets.Messages;
using Edelstein.Common.Packets.Stats;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Types;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Conversations;
using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Fields.Objects.Users.Effects;
using Edelstein.WvsGame.Fields.Objects.Users.Stats;
using Edelstein.WvsGame.Interactions.Dialogue;
using Edelstein.WvsGame.Packets;
using Edelstein.WvsGame.Sockets;

namespace Edelstein.WvsGame.Fields.Objects.Users
{
    public partial class FieldUser : FieldObject
    {
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

        public IDictionary<TemporaryStatType, Timer> TemporaryStatTimers;

        public FieldUser(GameClientSocket socket, Character character)
        {
            Socket = socket;
            Character = character;

            ForcedStat = new ForcedStat();
            BasicStat = new BasicStat(this);
            SecondaryStat = new SecondaryStat(this);
            TemporaryStat = new TemporaryStat();
            ValidateStat();

            TemporaryStatTimers = new Dictionary<TemporaryStatType, Timer>();
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

        public async Task<T> Prompt<T>(ConversationQuestion<T> request)
        {
            var context = new ConversationContext(Socket);

            if (ConversationContext != null) 
                throw new InvalidOperationException("Tried to prompt when already in conversation.");

            ConversationContext = context;
            var result = context.Send(request);
            ConversationContext = null;
            await ModifyStats(exclRequest: true);
            return result;
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