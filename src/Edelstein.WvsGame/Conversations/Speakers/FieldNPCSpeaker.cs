using System;
using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Conversations.Messages.Requests;
using Edelstein.WvsGame.Fields.Objects;

namespace Edelstein.WvsGame.Conversations.Speakers
{
    public class FieldNPCSpeaker : Speaker
    {
        private readonly FieldNPC _fieldNPC;

        public FieldNPCSpeaker(
            ConversationContext context,
            FieldNPC fieldNPC
        ) : base(context)
        {
            _fieldNPC = fieldNPC;
        }

        public void Say(string message)
        {
            var res = Context.Send(new Say(
                0,
                _fieldNPC.Template.TemplateID,
                SpeakerParamType.NoESC,
                message
            ));
        }
    }
}