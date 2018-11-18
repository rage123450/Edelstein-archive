using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Conversations.Messages.Requests;
using MoreLinq;

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

        public int Say(string text = "", bool prev = false, bool next = false)
            => Context.Send(new Say(SpeakerTypeID, SpeakerTemplateID, SpeakerParam, text, prev, next));

        public int AskYesNo(string text = "", bool quest = false)
            => Context.Send(new AskYesNo(SpeakerTypeID, SpeakerTemplateID, SpeakerParam, text, quest));

        public string AskText(string text = "", string textDefault = "",
            short lenMin = short.MinValue, short lenMax = short.MaxValue)
            => Context.Send(new AskText(SpeakerTypeID, SpeakerTemplateID, SpeakerParam, text, textDefault,
                lenMin, lenMax));

        public string AskBoxText(string text = "", string textDefault = "", short col = 24, short line = 4)
            => Context.Send(new AskBoxText(SpeakerTypeID, SpeakerTemplateID, SpeakerParam, text, textDefault,
                col, line));

        public int AskNumber(string text = "", int def = 0, int min = int.MinValue, int max = int.MaxValue)
            => Context.Send(new AskNumber(SpeakerTypeID, SpeakerTemplateID, SpeakerParam, text, def, min, max));

        public int AskMenu(string text = "")
            => Context.Send(new AskMenu(SpeakerTypeID, SpeakerTemplateID, SpeakerParam, text));

        public int AskMenu(string text, IEnumerable<string> options)
        {
            text += "\r\n";
            text = options
                .Select((value, i) => new {i, value})
                .Aggregate(text, (current, item) => current + "#L" + item.i + "#" + item.value + "#l\r\n");
            return AskMenu(text);
        }

        public byte AskAvatar(string text, int[] styles)
            => Context.Send(new AskAvatar(SpeakerTypeID, SpeakerTemplateID, SpeakerParam, text, styles));
    }
}