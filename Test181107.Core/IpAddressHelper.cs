using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Test181107.Core
{
    public static class IpAddressHelper
    {
        public static string GetHostIp()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.ToList().First(s => s.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        }
    }
}
