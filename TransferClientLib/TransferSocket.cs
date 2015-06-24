using SuperSocket.ClientEngine;
using SuperSocket.ClientEngine.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using TransferClientLib.Command;
using TransferCommon;
namespace TransferClientLib
{
    public class TransferSocket : IDisposable
    {
        private string _fileName;
        private string _saveName;
        private FileStream m_fileStream;
        private TcpClientSession Client;
        public delegate void TransferErrorEventHandler(TransferErrorEventArgs e);
        public event TransferErrorEventHandler TransferError;
        public delegate bool FileExistEventHandler(EventArgs e);
        public event FileExistEventHandler FileExist;
        internal int TransferStatus = 0;//0-99上传状态，100以上为下载状态
        int PacketSize = 1024 * 30;
        byte[] readBuffer;
        byte[] writeBuffer;
        AutoResetEvent m_OpenedEvent = new AutoResetEvent(false);
        protected IClientCommandReader<TransferSocketCommandInfo> CommandReader { get; private set; }
        private Dictionary<string, ICommand<TransferSocket, TransferSocketCommandInfo>> m_CommandDict
            = new Dictionary<string, ICommand<TransferSocket, TransferSocketCommandInfo>>
                (StringComparer.OrdinalIgnoreCase);
        public TransferSocket(EndPoint endpoint, string fileName, string saveName)
        {
            readBuffer = new byte[PacketSize];
            _fileName = fileName;
            _saveName = saveName;
            var cmds = new UpLoadSocketCommandBase[] { 
                new CheckFile(),
                new DoData(),
                new Text(),
            };
            foreach (var item in cmds)
                m_CommandDict.Add(item.Name, item);
            TcpClientSession client = new AsyncTcpSession(endpoint);
            client.Error += client_Error;
            client.DataReceived += client_DataReceived;
            client.Connected += client_Connected;
            client.Closed += client_Closed;
            Client = client;
            CommandReader = new TransferReader(this);
        }

        public void StartUpLoad()
        {
            try
            {
                if (TransferStatus != 0)
                    throw new Exception("状态错误");
                Client.Connect();
                if (!m_OpenedEvent.WaitOne(5000))
                    throw new Exception("连接失败");
                SendMessage(TransferOP.CheckFile, _saveName);
            }
            catch (Exception ex)
            {
                OnTransferError(new TransferErrorEventArgs(ex));
            }
        }
        internal void OnCheckFile()
        {
            m_fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
            UpLoadInfo uploadInfo = new UpLoadInfo(_fileName, m_fileStream.Length, 0, m_fileStream.Length, _saveName);
            byte[] data = SerializeHelp.Serialize<UpLoadInfo>(uploadInfo);
            SendData(TransferOP.DoUpLoad, data, 0, data.Length);
        }
        internal void OnDoData()
        {
            while (m_fileStream.Position < m_fileStream.Length)
            {
                int count = m_fileStream.Read(readBuffer, 0, PacketSize);
                SendData(TransferOP.DoData, readBuffer, 0, count);//上传文件
            }
        }
        internal void DoEnd()
        {

        }
        #region 事件
        void client_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
        }

        void client_DataReceived(object sender, DataEventArgs e)
        {
            m_OnDataReceived(e.Data, e.Offset, e.Length);
        }

        void client_Connected(object sender, EventArgs e)
        {
            m_OpenedEvent.Set();
        }

        void client_Closed(object sender, EventArgs e)
        {
        }
        #endregion
        #region 交互事件
        private void m_OnDataReceived(byte[] data, int offset, int length)
        {
            while (true)
            {
                int left;
                var commandInfo = CommandReader.GetCommandInfo(data, offset, length, out left);
                if (CommandReader.NextCommandReader != null)
                    CommandReader = CommandReader.NextCommandReader;
                if (commandInfo != null)
                    ExecuteCommand(commandInfo);
                if (left <= 0)
                    break;
                offset = offset + length - left;
                length = left;
            }
        }
        #endregion
        #region 处理事件
        internal void OnTransferError(TransferErrorEventArgs e)
        {
            if (TransferError != null)
                this.TransferError(e);
            Dispose();
        }
        internal bool OnFileExist()
        {
            if (FileExist != null)
                return this.FileExist(new EventArgs());
            OnTransferError(new TransferErrorEventArgs(new Exception("已存在相同文件")));
            return false;
        }
        #endregion
        #region 方法
        private void ExecuteCommand(TransferSocketCommandInfo commandInfo)
        {
            ICommand<TransferSocket, TransferSocketCommandInfo> command;
            if (m_CommandDict.TryGetValue(commandInfo.Key, out command))
            {
                command.ExecuteCommand(this, commandInfo);
            }
        }
        void SendMessage(TransferOP opCode, string msg)
        {
            var data = Encoding.UTF8.GetBytes(msg);
            SendData(opCode, data, 0, data.Length);
        }
        void SendData(TransferOP opCode, byte[] data, int offset, int length)
        {
            byte[] senddata = new byte[length + 4];//要发出的数据：组成为命令，数据长度，数据
            senddata[0] = (byte)((int)opCode / 256);
            senddata[1] = (byte)((int)opCode % 256);
            senddata[2] = (byte)(length / 256);
            senddata[3] = (byte)(length % 256);
            Buffer.BlockCopy(data, offset, senddata, 4, length);
            Client.Send(senddata, 0, senddata.Length);
        }
        public void Dispose()
        {
            _fileName = string.Empty;
            if (m_fileStream != null)
            {
                m_fileStream.Close();
                m_fileStream = null;
            }
            var client = Client;
            if (client != null)
            {
                client.Error -= client_Error;
                client.DataReceived -= client_DataReceived;
                client.Connected -= client_Connected;
                client.Closed -= client_Closed;
                if (client.IsConnected)
                    client.Close();
                Client = null;
            }
        }
        #endregion
    }
    public class TransferErrorEventArgs
    {
        public TransferErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }
        public Exception Exception { get; private set; }
    }
}
