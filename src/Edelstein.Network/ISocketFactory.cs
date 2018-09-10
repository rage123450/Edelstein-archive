using DotNetty.Transport.Channels;

namespace Edelstein.Network
{
    public interface ISocketFactory<T>
        where T : Socket
    {
        T createNew(IChannel channel, uint seqSend, uint seqRecv);
    }
}