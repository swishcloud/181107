using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Test181107.Core
{
    public class BytesMessage : MessagePayload, IDataHandler
    {
        private const int DATA_PER_READ_MAX_SIZE = 1024;
        public byte[] Bytes { get; private set; }
        public BytesMessage(Header header)
        {
            Header = header;
        }
        public BytesMessage(byte[] bytes, string key, string from, string to, string mark)
        {
            this.Bytes = bytes;
            Header = new Header() { Key = key, Mark = mark, From = from, To = to, DataLength = this.Bytes.LongLength };
        }
        public void Receive(NetworkStream networkStream)
        {
            Bytes = ReceiveBatch(networkStream, (int)Header.DataLength);
        }
        public void Transfer(NetworkStream networkStream)
        {
            networkStream.Write(Bytes, 0, Bytes.Length);
        }
    }
}
