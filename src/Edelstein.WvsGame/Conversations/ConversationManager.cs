using System.Threading.Tasks;
using Edelstein.WvsGame.Conversations.Speakers;

namespace Edelstein.WvsGame.Conversations
{
    public class ConversationManager<T, S>
        where T : FieldUserSpeaker
        where S : Speaker
    {
        public Task Start(T target, S self, Conversation<T, S> conversation)
        {
            if (target.FieldUser.ConversationContext != null) return Task.CompletedTask;

            target.FieldUser.ConversationContext = conversation.Context;
            return conversation.Start(target, self).ContinueWith(t =>
            {
                target.FieldUser.ConversationContext = null;
                target.FieldUser.ModifyStats(exclRequest: true);
            });
        }
    }
}