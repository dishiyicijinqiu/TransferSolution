using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using System;
using TransferCommon;

namespace TransferServerLib
{
    class TransferFilter : FixedHeaderReceiveFilter<BinaryRequestInfo>
    {
        public TransferFilter()
            : base(4)
        {
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return (int)header[offset + 2] * 256 + (int)header[offset + 3];
        }

        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            TransferOP opcode = (TransferOP)((int)header.Array[0] * 256 + (int)header.Array[1]);
            return new BinaryRequestInfo(opcode.ToString(), bodyBuffer.CloneRange(offset, length));
        }
    }
}
