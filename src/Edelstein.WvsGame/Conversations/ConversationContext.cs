using System;
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
        public ScriptMessageType AwaitingType;
        public readonly BlockingCollection<object> Answers;
        public readonly CancellationTokenSource TokenSource;
        protected readonly Socket Socket;

        public ConversationContext(CancellationTokenSource tokenSource, Socket socket)
        {
            Answers = new BlockingCollection<object>();
            TokenSource = tokenSource;
            Socket = socket;
        }

        public T Send<T>(ConversationQuestion<T> request)
        {
            AwaitingType = request.MessageType;
            using (var p = new OutPacket(GameSendOperations.ScriptMessage))
            {
                request.Encode(p);
                Socket.SendPacket(p);
            }

            return (T) Answers.Take(TokenSource.Token);
        }
    }
}