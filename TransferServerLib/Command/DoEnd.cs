using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace TransferServerLib.Command
{
    public class DoEnd : CommandBase<TransferSession, BinaryRequestInfo>
    {
        public override void ExecuteCommand(TransferSession session, BinaryRequestInfo requestInfo)
        {
        }
    }
}
