using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System.Text;
using TransferCommon;

namespace TransferServerLib.Command
{
    public class CheckFile : CommandBase<TransferSession, BinaryRequestInfo>
    {
        public override void ExecuteCommand(TransferSession session, BinaryRequestInfo requestInfo)
        {
            string fileName = System.IO.Path.GetFullPath(Encoding.UTF8.GetString(requestInfo.Body));
            if (System.IO.File.Exists(fileName))
                session.SendData(TransferOP.CheckFile, new byte[] { 1 }, 0, 1);
            else
                session.SendData(TransferOP.CheckFile, new byte[] { 0 }, 0, 1);
        }
    }
}
