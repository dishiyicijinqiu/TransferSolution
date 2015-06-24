using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.IO;
using TransferCommon;

namespace TransferServerLib
{
    public class TransferSession : AppSession<TransferSession, BinaryRequestInfo>, IDisposable
    {
        FileStream m_fileStream;
        private string _fileName;
        private string _saveName;
        protected override void OnSessionStarted()
        {
            //SendMessage("Connected");
        }

        protected override void HandleException(Exception e)
        {
            //SendMessage("Application error: {0}", e.Message);
        }

        //public void SendMessage(string message)
        //{
        //    var data = Encoding.UTF8.GetBytes(message);
        //    SendData(TransferOP.Text, data, 0, data.Length);
        //}

        //public void SendMessage(string message, params object[] paramValues)
        //{
        //    message = string.Format(message, paramValues);
        //    SendMessage(message);
        //}

        public void SendData(TransferOP opCode, byte[] data, int offset, int length)
        {
            byte[] senddata = new byte[length + 4];//要发出的数据：组成为命令，数据长度，数据
            senddata[0] = (byte)((int)opCode / 256);
            senddata[1] = (byte)((int)opCode % 256);
            senddata[2] = (byte)(data.Length / 256);
            senddata[3] = (byte)(data.Length % 256);
            Buffer.BlockCopy(data, offset, senddata, 4, length);
            this.Send(senddata, 0, senddata.Length);
        }
        public void OnDoUpLoad(UpLoadInfo uploadInfo)
        {
            _saveName = uploadInfo.SaveName;
            string fileFullPath = Path.GetFullPath(_saveName);
            m_fileStream = new FileStream(fileFullPath, FileMode.OpenOrCreate, FileAccess.Write);
        }
        public void OnDoData(byte[] data, int offset, int count)
        {
            m_fileStream.Write(data, offset, count);
            //Console.WriteLine(string.Format("当前位置：{0},本次上传{1},当前大小{2}",
            //    m_fileStream.Position, count, m_fileStream.Length));
            if (m_fileStream.Length <= m_fileStream.Position)
            {
                DoEnd();
            }
        }
        public void DoEnd()
        {
            this.SendData(TransferOP.DoEnd, new byte[] { 1 }, 0, 1);
            this.Dispose();
        }

        public void Dispose()
        {
            _fileName = string.Empty;
            if (m_fileStream != null)
            {
                m_fileStream.Close();
                m_fileStream = null;
            }
        }
    }
}
