using System;
using System.Threading.Tasks;
using Edelstein.WvsGame.Conversations.Speakers;

namespace Edelstein.WvsGame.Conversations
{
    public class ActionConversation : Conversation<FieldUserSpeaker, NPCSpeaker>
    {
        private readonly Action<FieldUserSpeaker, NPCSpeaker> _action;

        public ActionConversation(ConversationContext context, Action<FieldUserSpeaker, NPCSpeaker> action) :
            base(context)
        {
            _action = action;
        }

        public override Task Start(FieldUserSpeaker target, NPCSpeaker self)
        {
            return Task.Run(() =>
            {
                try
                {

                    _action.Invoke(target, self);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }, Context.TokenSource.Token);
        }
    }
}