using SuperSocket.ClientEngine.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransferClientLib.Command
{
    public abstract class UpLoadSocketCommandBase : ICommand<TransferSocket, TransferSocketCommandInfo>
    {
        public abstract void ExecuteCommand(TransferSocket session, TransferSocketCommandInfo commandInfo);

        public abstract string Name { get; }
    }
}
