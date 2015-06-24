using TransferClientLib;
using TransferCommon;
namespace TransferClientLib.Command
{
    public class DoData : UpLoadSocketCommandBase//文件上传时不接受命令，也可设置为接受命令
    {
        public override void ExecuteCommand(TransferSocket session, TransferSocketCommandInfo commandInfo)
        {
            session.OnDoData();
        }

        public override string Name
        {
            get { return TransferOP.DoData.ToString(); }
        }
    }
}
