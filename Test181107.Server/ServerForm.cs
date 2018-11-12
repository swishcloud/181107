using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test181107.Core;

namespace Test181107.Server
{
    public partial class ServerForm : Form
    {
        TcpListener tcpListener = null;
        private List<User> users = new List<User>();
        public ServerForm()
        {
            InitializeComponent();
            listBox1.Items.Add("welcome to test this project,it's a network communication project based on TCP protocol,just for studying use only,may be updated at any time.");
            Env.Initialize(Print);
            tcpListener = new TcpListener();
            tcpListener.ListeningStarted += (s, e) =>
            {
                btnListener.Text = "listening...";
            };
            tcpListener.ListeningStoped += (s, e) =>
            {
                btnListener.Text = "listen";
            };
            tcpListener.TcpClientAccepted += (s, e) =>
            {
                TcpClientAccepted(e);
            };
            tcpListener.NewSession += (s, e) =>
            {
                NewSession(e);
            };
        }

        private void NewSession(TcpClientSession session)
        {
            session.Disconnected += (s, e) =>
            {
                users.Remove(users.First(u => u.Session == e));
            };
        }
        private void TcpClientAccepted(TcpClientSession session)
        {
            session.Received += (s, e) =>
            {
                if (e.Header.Key == MessageKeys.Login)
                {
                    var userName = e.Header.From;
                    session.Send(MessageHelper.CreateLiteralMessage($"Hello,{userName}", null, userName, "system greeting"));
                    users.Add(new User() { Session = session, UserName = userName });
                    Print($"{userName} is logged in.");
                }
                else if (e.Header.To != null)
                {
                    var user = users.FirstOrDefault(u => u.UserName == e.Header.To);
                    if (user == null)
                    {
                        session.Send(MessageHelper.CreateLiteralMessage($"user {e.Header.To} not found.", null, e.Header.From, "system reply"));
                    }
                    else
                    {
                        user.Session.Send(e);
                    }

                }
                else if (e.Header.Key == MessageKeys.Literal)
                {
                    var msg = (BytesMessage)e;
                    Print($"{e.Header.From}:{MessageHelper.LiteralEncoding.GetString(msg.Bytes)}");
                }
                else if (e.Header.Key == MessageKeys.CloseSession)
                {
                    session.Disconnect("client disconnected");
                }
            };
        }
        private void btnListener_Click(object sender, EventArgs e)
        {
            Listen();
        }

        private void Print(string info)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                var index = listBox1.Items.Add(info);
                listBox1.TopIndex = index;
            }));
        }

        private void Listen()
        {
            if (tcpListener.Listening)
                tcpListener.Stop();
            else
                tcpListener.ListenAsync(IpAddressHelper.GetHostIp(), 5000);
        }

        private void ServerForm_Shown(object sender, EventArgs e)
        {
            //optional
            Listen();
        }

        private void openFileFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var directoryPath = Directory.CreateDirectory("TempFile").FullName;
            Process.Start(directoryPath);
        }
    }
}
