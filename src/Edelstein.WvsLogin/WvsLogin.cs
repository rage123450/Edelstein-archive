using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.WvsLogin
{
    public class WvsLogin
    {
        private Server<LoginClientSocket> _server;

        public async Task Run()
        {
            this._server = new Server<LoginClientSocket>(
                new LoginClientSocketFactory(),
                new Dictionary<short, IPacketHandler<LoginClientSocket>>()
            );
            
            await this._server.Run();
            await this._server.Channel.CloseCompletion;
        }
    }
}