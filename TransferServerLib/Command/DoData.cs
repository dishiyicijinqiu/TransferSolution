using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace TransferServerLib.Command
{
    public class DoData : CommandBase<TransferSession, BinaryRequestInfo>
    {
        public override void ExecuteCommand(TransferSession session, BinaryRequestInfo requestInfo)
        {
            session.OnDoData(requestInfo.Body, 0, requestInfo.Body.Length);
        }
    }
}
