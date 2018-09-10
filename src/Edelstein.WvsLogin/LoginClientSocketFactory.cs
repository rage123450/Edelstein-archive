using DotNetty.Transport.Channels;
using Edelstein.Network;

namespace Edelstein.WvsLogin
{
    public class LoginClientSocketFactory : ISocketFactory<LoginClientSocket>
    {
        public LoginClientSocket createNew(IChannel channel, uint seqSend, uint seqRecv)
        {
            return new LoginClientSocket(channel, seqSend, seqRecv);
        }
    }
}