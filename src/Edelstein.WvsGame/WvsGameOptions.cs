using Edelstein.Network;

namespace Edelstein.WvsGame
{
    public class WvsGameOptions
    {
        public string ServerName { get; set; }
        public ClientOptions InteropClientOptions { get; set; }
        public string ConnectionString { get; set; }
    }
}