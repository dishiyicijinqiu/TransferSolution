using TransferClientLib;
using TransferCommon;
namespace TransferClientLib.Command
{
    public class Text : UpLoadSocketCommandBase
    {
        public override void ExecuteCommand(TransferSocket session, TransferSocketCommandInfo commandInfo)
        {
            System.Console.WriteLine(commandInfo.Text);
        }

        public override string Name
        {
            get { return TransferOP.Text.ToString(); }
        }
    }
}

