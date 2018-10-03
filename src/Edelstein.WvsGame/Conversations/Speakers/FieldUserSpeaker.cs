using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Conversations.Speakers
{
    public class FieldUserSpeaker : Speaker
    {
        private readonly FieldUser _fieldUser;

        public FieldUserSpeaker(
            ConversationContext context,
            FieldUser fieldUser
        ) : base(context)
        {
            _fieldUser = fieldUser;
        }
    }
}