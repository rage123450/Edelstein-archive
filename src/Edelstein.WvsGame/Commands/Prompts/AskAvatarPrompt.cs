using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Conversations.Messages.Requests;

namespace Edelstein.WvsGame.Commands.Prompts
{
    public class AskAvatarPrompt : AskAvatar
    {
        public AskAvatarPrompt(string text, int[] styles)
            : base(0, 9010000, SpeakerParamType.NPCReplacedByUser, text, styles)
        {
        }
    }
}