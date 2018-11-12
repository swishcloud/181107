using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Test181107.Core
{
    public abstract class MessagePayload
    {
        public Header Header { get; set; }
        protected virtual byte[] ReceiveBatch(NetworkStream networkStream, int batchMaxLength)
        {
            Byte[] bytes = new byte[batchMaxLength];
            var remainSize = bytes.Length;
            while (remainSize != 0)
            {
                remainSize -= networkStream.Read(bytes, bytes.Length - remainSize, batchMaxLength > remainSize ? remainSize : batchMaxLength);
            }
            return bytes;
        }
        public override string ToString()
        {
            return $"type:{GetType().Name} {Header}";
        }
    }
}