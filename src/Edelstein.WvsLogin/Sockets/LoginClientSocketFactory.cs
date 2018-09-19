using System;
using DotNetty.Transport.Channels;
using Edelstein.Network;

namespace Edelstein.WvsLogin.Sockets
{
    public class LoginClientSocketFactory : ISocketFactory<LoginClientSocket>
    {
        private readonly WvsLogin _wvsLogin;

        public LoginClientSocketFactory(WvsLogin wvsLogin)
        {
            this._wvsLogin = wvsLogin;
        }

        public LoginClientSocket Build(IChannel channel, uint seqSend, uint seqRecv)
        {
            return new LoginClientSocket(this._wvsLogin, channel, seqSend, seqRecv);
        }
    }
}