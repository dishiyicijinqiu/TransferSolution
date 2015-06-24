using SuperSocket.ClientEngine.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransferClientLib
{

    public abstract class ReaderBase : IClientCommandReader<TransferSocketCommandInfo>
    {
        protected TransferSocket TransferSocket { get; private set; }
        public ReaderBase(TransferSocket transferSocket)
        {
            TransferSocket = transferSocket;
            m_BufferSegments = new ArraySegmentList();
        }
        private readonly ArraySegmentList m_BufferSegments;
        protected ArraySegmentList BufferSegments
        {
            get { return m_BufferSegments; }
        }
        public ReaderBase(ReaderBase previousCommandReader)
        {
            m_BufferSegments = previousCommandReader.BufferSegments;
        }
        public abstract TransferSocketCommandInfo GetCommandInfo(byte[] readBuffer, int offset, int length, out int left);

        public IClientCommandReader<TransferSocketCommandInfo> NextCommandReader { get; internal set; }
        protected void AddArraySegment(byte[] buffer, int offset, int length)
        {
            BufferSegments.AddSegment(buffer, offset, length, true);
        }
        protected void ClearBufferSegments()
        {
            BufferSegments.ClearSegements();
        }
    }
}
