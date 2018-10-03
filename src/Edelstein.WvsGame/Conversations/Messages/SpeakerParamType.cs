using System;

namespace Edelstein.WvsGame.Conversations.Messages
{
    [Flags]
    public enum SpeakerParamType
    {
        NoESC = 0x1,
        NPCReplacedByUser = 0x2,
        NPCReplacedByNPC = 0x4,
        FlipImage = 0x8
    }
}