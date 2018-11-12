using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Test181107.Core
{
    public class TcpListener
    {
        public System.Net.Sockets.TcpListener server { get; private set; }
        public event EventHandler<System.Net.Sockets.TcpListener> ListeningStarted;
        public event EventHandler<System.Net.Sockets.TcpListener> ListeningStoped;
        public event EventHandler<TcpClientSession> TcpClientAccepted;
        public event EventHandler<TcpClientSession> NewSession;
        public bool Listening { get; private set; }
        public async void ListenAsync(string ip, int port)
        {
            if (Listening)
            {
                throw new Exception("Listening");
            }
            Listening = true;

            server = new System.Net.Sockets.TcpListener(IPAddress.Parse(ip), port);
            server.Start();
            ListeningStarted(this, server);
            Env.Print($"listening: {server.LocalEndpoint.ToString()}");
            try
            {
                while (true)
                {
                    Env.Print("waiting for a connection");
                    var client = await server.AcceptTcpClientAsync();
                    Env.Print("one incoming tcp connection");

                    var session = new TcpClientSession(client);
                    NewSession.Invoke(this, session);
                    TcpClientAccepted?.Invoke(this, session);
                    session.Receive();
                }
            }
            catch (Exception ex)
            {
                Env.Print(ex.Message);
            }
        }
        public void Stop()
        {
            server.Stop();
            Listening = false;
            Env.Print("listening stoped");
            ListeningStoped?.Invoke(this, server);
        }
    }
}
