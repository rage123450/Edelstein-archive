using System.Collections.Concurrent;
using System.Threading;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Conversations.Messages;
using Edelstein.WvsGame.Packets;

namespace Edelstein.WvsGame.Conversations
{
    public class ConversationContext
    {
        public readonly BlockingCollection<ConversationAnswer> Answers;
        public readonly CancellationTokenSource TokenSource;
        protected readonly Socket Socket;

        public ConversationContext(CancellationTokenSource tokenSource, Socket socket)
        {
            Answers = new BlockingCollection<ConversationAnswer>();
            TokenSource = tokenSource;
            Socket = socket;
        }

        public ConversationAnswer Send(ConversationQuestion request)
        {
            using (var p = new OutPacket(GameSendOperations.ScriptMessage))
            {
                request.Encode(p);
                Socket.SendPacket(p);
            }

            return Answers.Take(TokenSource.Token);
        }
    }
}