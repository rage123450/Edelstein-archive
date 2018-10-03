namespace Edelstein.WvsGame.Conversations.Speakers
{
    public abstract class Speaker
    {
        protected readonly ConversationContext Context;

        protected Speaker(ConversationContext context)
        {
            Context = context;
        }
    }
}