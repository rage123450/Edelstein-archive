using System.Threading.Tasks;
using Edelstein.WvsGame.Conversations.Speakers;

namespace Edelstein.WvsGame.Conversations
{
    public static class ConversationManager<T, S>
        where T : FieldUserSpeaker
        where S : Speaker
    {
        public static Task<bool> Start(T target, S self, Conversation<T, S> conversation)
        {
            if (target.FieldUser.ConversationContext != null) return Task.FromResult(false);

            target.FieldUser.ConversationContext = conversation.Context;
            var task = conversation.Start(target, self);
            task.ContinueWith(t =>
            {
                target.FieldUser.ConversationContext = null;
                target.FieldUser.ModifyStats(exclRequest: true);
            });
            return task;
        }
    }
}