using System;
using System.Text;
using TransferCommon;

namespace TransferClientLib
{
    internal class TransferReader : ReaderBase
    {
        public TransferReader(TransferSocket transferSocket)
            : base(transferSocket)
        {
        }
        public override TransferSocketCommandInfo GetCommandInfo(byte[] readBuffer, int offset, int length, out int left)
        {
            left = 0;
            if ((readBuffer.Length - length) < 4)
                return null;
            TransferOP opcode = (TransferOP)((int)readBuffer[offset + 0] * 256 + (int)readBuffer[offset + 1]);
            if (!Enum.IsDefined(typeof(TransferOP), opcode))
                return null;
            left = 4;
            int datalen = ((int)readBuffer[offset + 2] * 256 + (int)readBuffer[offset + 3]);
            var data = new byte[datalen];
            Buffer.BlockCopy(readBuffer, left, data, 0, datalen);
            switch (opcode)
            {
                case TransferOP.CheckFile:
                    return new TransferSocketCommandInfo(TransferOP.CheckFile.ToString(), data);
                case TransferOP.DoUpLoad:
                    return null;
                case TransferOP.DoData:
                    return null;
                case TransferOP.DoEnd:
                    return null;
                case TransferOP.Text:
                    return new TransferSocketCommandInfo(TransferOP.Text.ToString(), Encoding.UTF8.GetString(data));
                default:
                    return null;
            }
        }
    }
}
