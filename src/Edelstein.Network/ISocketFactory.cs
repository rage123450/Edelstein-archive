using DotNetty.Transport.Channels;

namespace Edelstein.Network
{
    public interface ISocketFactory<T>
        where T : Socket
    {
        T Build(IChannel channel, uint seqSend, uint seqRecv);
    }
}