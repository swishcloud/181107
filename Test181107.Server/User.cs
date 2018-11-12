using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test181107.Core;
namespace Test181107.Server
{
    public class User
    {
        public string UserName { get; set; }
        public TcpClientSession Session { get; set; }
    }
}
