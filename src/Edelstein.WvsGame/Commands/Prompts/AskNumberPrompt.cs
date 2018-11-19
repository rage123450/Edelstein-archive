using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Conversations.Messages.Requests;

namespace Edelstein.WvsGame.Commands.Prompts
{
    public class AskNumberPrompt : AskNumber
    {
        public AskNumberPrompt(string text, int def, int min, int max)
            : base(0, 9010000, SpeakerParamType.NPCReplacedByUser, text, def, min, max)
        {
        }
    }
}