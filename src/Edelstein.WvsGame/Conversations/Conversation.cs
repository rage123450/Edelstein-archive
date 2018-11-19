using System.Threading.Tasks;
using Edelstein.WvsGame.Conversations.Speakers;

namespace Edelstein.WvsGame.Conversations
{
    public abstract class Conversation<T, S>
        where T : Speaker
        where S : Speaker
    {
        public readonly ConversationContext Context;

        public Conversation(ConversationContext context)
        {
            Context = context;
        }

        public abstract Task<bool> Start(T target, S self);
    }
}