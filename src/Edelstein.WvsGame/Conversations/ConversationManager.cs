using Edelstein.WvsGame.Conversations.Speakers;

namespace Edelstein.WvsGame.Conversations
{
    public abstract class ConversationManager<T, S>
        where T : FieldUserSpeaker
        where S : Speaker
    {
        public bool Start(T target, S self, Conversation<T, S> conversation)
        {
            if (target.FieldUser.ConversationContext != null) return false;

            target.FieldUser.ConversationContext = conversation.Context;
            conversation.Start(target, self).ContinueWith(t =>
            {
                target.FieldUser.ConversationContext = null;
                target.FieldUser.ModifyStats(exclRequest: true);
            });
            return true;
        }
    }
}