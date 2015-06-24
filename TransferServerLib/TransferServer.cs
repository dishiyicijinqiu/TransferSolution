using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace TransferServerLib
{
    public class TransferServer : AppServer<TransferSession, BinaryRequestInfo>
    {
        public TransferServer()
            : base(new DefaultReceiveFilterFactory<TransferFilter, BinaryRequestInfo>())
        {

        }
    }
}
