using Edelstein.Network;

namespace Edelstein.WvsGame
{
    public class WvsGameOptions
    {
        public GameInfo GameInfo { get; set; }

        public ClientOptions InteropClientOptions { get; set; }
        public string ConnectionString { get; set; }
    }

    public class GameInfo
    {
        public byte ID { get; set; }
        public string Name { get; set; }
        public bool AdultChannel { get; set; }
    }
}