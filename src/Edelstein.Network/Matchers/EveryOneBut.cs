using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;

namespace Edelstein.Network.Matchers
{
    public class EveryOneBut : IChannelMatcher
    {
        readonly IChannelId id;

        public EveryOneBut(IChannelId id)
        {
            this.id = id;
        }

        public bool Matches(IChannel channel) => !Equals(channel.Id, this.id);
    }
}