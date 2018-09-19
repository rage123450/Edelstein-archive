using Edelstein.Network;

namespace Edelstein.WvsLogin
{
    public class WvsLoginOptions
    {
        public string ServerName { get; set; }
        public ClientOptions InteropClientOptions { get; set; }
        public ServerOptions GameServerOptions { get; set; }
    }
}