using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System.Text;
using TransferCommon;

namespace TransferServerLib.Command
{
    public class DoUpLoad : CommandBase<TransferSession, BinaryRequestInfo>
    {
        public override void ExecuteCommand(TransferSession session, BinaryRequestInfo requestInfo)
        {
            UpLoadInfo uploadInfo = SerializeHelp.Deserialize<UpLoadInfo>(requestInfo.Body);
            session.OnDoUpLoad(uploadInfo);
            session.SendData(TransferOP.DoData, new byte[] { 1 }, 0, 1);//上传文件
        }
    }
}
