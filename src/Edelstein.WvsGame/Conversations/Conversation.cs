using System.Threading.Tasks;
using Edelstein.WvsGame.Conversations.Speakers;

namespace Edelstein.WvsGame.Conversations
{
    public abstract class Conversation<T, S>
        where T : Speaker
        where S : Speaker
    {
        public abstract Task Start(ConversationContext context, T target, S self);
    }
}