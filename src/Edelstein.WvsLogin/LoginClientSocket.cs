using DotNetty.Transport.Channels;
using Edelstein.Network;

namespace Edelstein.WvsLogin
{
    public class LoginClientSocket : Socket
    {
        public LoginClientSocket(IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
        }
    }
}