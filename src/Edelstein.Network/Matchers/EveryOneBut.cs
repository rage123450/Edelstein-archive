using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;

namespace Edelstein.Network.Matchers
{
    public class EveryOneBut : IChannelMatcher
    {
        readonly IChannelId _id;

        public EveryOneBut(IChannelId id)
        {
            _id = id;
        }

        public bool Matches(IChannel channel) => !Equals(channel.Id, _id);
    }
}