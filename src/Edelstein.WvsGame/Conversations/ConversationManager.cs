namespace Edelstein.WvsGame.Conversations
{
    public abstract class ConversationManager<T, S>
    {
        public abstract bool Start(T target, S self);
    }
}