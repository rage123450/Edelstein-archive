namespace Edelstein.WvsGame.Conversations.Messages.Requests
{
    public class AskMembershopAvatar : AskAvatar
    {
        public override ScriptMessageType MessageType => ScriptMessageType.AskMembershopAvatar;
        
        public AskMembershopAvatar(byte speakerTypeID, int speakerTemplateID, SpeakerParamType speakerParam, string text, int[] styles) : base(speakerTypeID, speakerTemplateID, speakerParam, text, styles)
        {
        }
    }
}