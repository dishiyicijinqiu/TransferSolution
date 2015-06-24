using TransferClientLib;
using TransferCommon;
namespace TransferClientLib.Command
{
    public class CheckFile : UpLoadSocketCommandBase
    {
        public override void ExecuteCommand(TransferSocket session, TransferSocketCommandInfo commandInfo)
        {
            if (session.TransferStatus < 100)//表示上传
            {
                if (commandInfo.Text == "1")//上传时服务器存在相同的文件
                {
                    if (session.OnFileExist())//返回真则覆盖处理
                    {
                        session.OnCheckFile();
                    }
                }
                else
                {
                    session.OnCheckFile();
                }
            }
            else//下载
            {

            }
        }

        public override string Name
        {
            get { return TransferOP.CheckFile.ToString(); }
        }
    }
}
