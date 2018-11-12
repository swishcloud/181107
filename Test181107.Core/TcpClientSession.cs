using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Test181107.Core
{
    public class TcpClientSession
    {
        public System.Net.Sockets.TcpClient TcpClient { get; private  set; }
        public Guid Id => Guid.NewGuid();
        public event EventHandler<TcpClientSession> Disconnected;
        public event EventHandler<MessagePayload> Received;
        public event EventHandler<MessagePayload> SentCompleted;
        public EndPoint RemoteEndPoint;
        public EndPoint LocalEndPoint;
        public bool ReceiveCalled { get; private set; }
        private bool disconnected;
        public TcpClientSession(System.Net.Sockets.TcpClient tcpClient)
        {
            this.TcpClient = tcpClient;
            this.RemoteEndPoint = tcpClient.Client.RemoteEndPoint;
            this.LocalEndPoint = tcpClient.Client.LocalEndPoint;
        }
        public void Receive()
        {
            if (ReceiveCalled)
                throw new Exception("this method can not be called mutiple times");
            ReceiveCalled = true;

            Task.Run(() =>
            {
                try
                {
                    //get a stream object for reading and writing
                    NetworkStream stream = this.TcpClient.GetStream();
                    while (true)
                    {
                        ReadHeader(stream);
                    }
                }
                catch (Exception ex)
                {
                    OnDisconnected(ex);
                }
            });
        }

        public bool Send(MessagePayload msg)
        {
            lock (this)
            {
                if (!this.TcpClient.Connected)
                {
                    Env.Print("unconnected,message not sent:" + msg);
                    return false;
                }

                var headerBytes = msg.Header.GetBytes();
                TcpClient.GetStream().Write(headerBytes, 0, headerBytes.Length);

                try
                {
                    var dataHandler = msg as IDataHandler;
                    dataHandler?.Transfer(TcpClient.GetStream());
                }
                catch (Exception ex)
                {
                    OnDisconnected(ex);
                    return false;
                }
                OnSentCompleted(msg);
                return true;
            }
        }

        public void Disconnect(string error)
        {
            OnDisconnected(new Exception(error));
        }

        #region About Events
        private void OnReceived(MessagePayload messagePayload)
        {
            Env.Print($"read:{messagePayload.ToString()}");
            Received?.Invoke(this, messagePayload);
        }
        private void OnSentCompleted(MessagePayload messagePayload)
        {
            Env.Print("sent " + messagePayload.ToString());
            SentCompleted?.Invoke(this, messagePayload);
        }
        private void OnDisconnected(Exception ex)
        {
            if (disconnected)
            {
                Env.Print("warn:connection hase been disconnected before.");
                return;
            }
            disconnected = true;
            Env.Print($"disconnected from {this.RemoteEndPoint}.cause:{ex.Message}");
            this.TcpClient.Close();
            this.TcpClient.Dispose();
            Disconnected?.Invoke(this, this);
        }
        #endregion

        #region Private Functions
        private MessagePayload GetMessagePayload(string key, Header header)
        {
            switch (key)
            {
                case MessageKeys.File:
                    return new FileMessage(header);
                default:
                    return new BytesMessage(header);
            }
        }
        private void ReadHeader(NetworkStream networkStream)
        {
            Byte[] bytes = new byte[Header.HEADER_SIZE];
            var readSize = 0;
            while (readSize != bytes.Length)
            {
                readSize += networkStream.Read(bytes, 0, Header.HEADER_SIZE - readSize);
            }
            var header = Header.GetHeader(bytes);
            Env.Print($"read header {header}");

            var messagepayload = GetMessagePayload(header.Key, header);

            var dataHandler = messagepayload as IDataHandler;
            dataHandler?.Receive(networkStream);

            OnReceived(messagepayload);
        }
        public override string ToString()
        {
            return $"remote({RemoteEndPoint}) local({LocalEndPoint})";
        }
        #endregion
    }
}
