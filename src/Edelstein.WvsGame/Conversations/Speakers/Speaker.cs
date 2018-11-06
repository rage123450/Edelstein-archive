using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Conversations.Messages.Requests;

namespace Edelstein.WvsGame.Conversations.Speakers
{
    public abstract class Speaker
    {
        protected readonly ConversationContext Context;
        public abstract byte SpeakerTypeID { get; }
        public abstract int SpeakerTemplateID { get; }
        public abstract SpeakerParamType SpeakerParam { get; }

        protected Speaker(ConversationContext context)
        {
            Context = context;
        }

        public Speaker AsSpeaker(int templateID, SpeakerParamType param = SpeakerParamType.NPCReplacedByNPC)
            => new NPCSpeaker(Context, templateID, param);

        public int Say(string text, bool prev = false, bool next = false)
            => Context.Send(new Say(SpeakerTypeID, SpeakerTemplateID, SpeakerParam, text, prev, next));

        public int AskYesNo(string text, bool quest = false)
            => Context.Send(new AskYesNo(SpeakerTypeID, SpeakerTemplateID, SpeakerParam, text, quest));

        public string AskText(string text, string textDefault = "",
            short lenMin = short.MinValue, short lenMax = short.MaxValue)
            => Context.Send(new AskText(SpeakerTypeID, SpeakerTemplateID, SpeakerParam, text, textDefault,
                lenMin, lenMax));

        public string AskBoxText(string text = "", string textDefault = "", short col = 24, short line = 4)
            => Context.Send(new AskBoxText(SpeakerTypeID, SpeakerTemplateID, SpeakerParam, text, textDefault,
                col, line));

        public int AskNumber(string text = "", int def = 0, int min = int.MinValue, int max = int.MaxValue)
            => Context.Send(new AskNumber(SpeakerTypeID, SpeakerTemplateID, SpeakerParam, text, def, min, max));
    }
}