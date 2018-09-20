using System.Collections.Generic;
using Edelstein.Network;

namespace Edelstein.WvsLogin
{
    public class WvsLoginOptions
    {
        public LoginInfo LoginInfo { get; set; }

        public ICollection<ClientOptions> InteropClientOptions { get; set; }
        public ServerOptions GameServerOptions { get; set; }
        public string ConnectionString { get; set; }
    }

    public class LoginInfo
    {
        public string Name { get; set; }
    }
}