using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Test181107.Core
{
    public interface IDataHandler
    {
        void Transfer(NetworkStream networkStream);
        void Receive(NetworkStream networkStream);
    }
}
