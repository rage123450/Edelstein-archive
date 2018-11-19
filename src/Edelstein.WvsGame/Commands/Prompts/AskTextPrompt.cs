using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Conversations.Messages.Requests;

namespace Edelstein.WvsGame.Commands.Prompts
{
    public class AskTextPrompt : AskText
    {
        public AskTextPrompt(string text, string textDefault, short lenMin, short lenMax)
            : base(0, 9010000, SpeakerParamType.NPCReplacedByUser, text, textDefault, lenMin, lenMax)
        {
        }
    }
}