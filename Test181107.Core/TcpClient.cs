using System;
using System.Net;
using System.Threading.Tasks;

namespace Test181107.Core
{
    public class TcpClient
    {
        public System.Net.Sockets.TcpClient Client = new System.Net.Sockets.TcpClient();
        public TcpClientSession Session { get; private set; }
        public event EventHandler<TcpClientSession> Connected;
        public event EventHandler<TcpClientSession> Disconnected;
        public event EventHandler<TcpClientSession> NewSession;
        public event EventHandler<Exception> ConnectFailed;
        public string Ip { get; private set; }
        public int Port { get; private set; }
        public TcpClient(string Ip, int port)
        {
            this.Ip = Ip;
            this.Port = port;
        }
        public async void Connect()
        {
            try
            {
                Client = new System.Net.Sockets.TcpClient(new IPEndPoint(IPAddress.Parse(IpAddressHelper.GetHostIp()), new Random().Next(80, 6000)));
                Env.Print($"connecting to {this.Ip}:{this.Port}");
                await Client.ConnectAsync(IPAddress.Parse(this.Ip), this.Port);
                Env.Print($"connected to {Client.Client.RemoteEndPoint} from {Client.Client.LocalEndPoint}");
                Session = new TcpClientSession(Client);
                NewSession(this, Session);
                Session.Disconnected += Disconnected;

                Connected?.Invoke(this, Session);
                Session.Receive();
            }
            catch (Exception ex)
            {
                Env.Print($"connect failed.cause:{ex.Message}");
                ConnectFailed(this, ex);
            }
        }

    }
}
