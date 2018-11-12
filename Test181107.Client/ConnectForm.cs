using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test181107.Core;

namespace Test181107.Client
{
    public partial class ConnectForm : Form
    {
        TcpClient tcpClient;
        long sentSize;
        string userName;
        bool canReconnect = true;
        List<RemoteForm> remotes = new List<RemoteForm>();
        public ConnectForm()
        {
            InitializeComponent();
            userName = Guid.NewGuid().ToString().Substring(0, 2);
            Text += " username:" + userName;
        }
        public void Connect(string ip, string port)
        {
            this.Show();

            tcpClient = new TcpClient(ip, Convert.ToInt32(port));
            tcpClient.NewSession += (s, e) => { NewSession(e); };
            tcpClient.Connected += (s, e) =>
            {
                e.Send(MessageHelper.CreateLoginMessage(userName, "request to login in"));
            };
            tcpClient.Disconnected += (s, e) =>
            {
                ReConnect();
            };
            tcpClient.ConnectFailed += (s, e) =>
            {
                ReConnect();
            };
        }
        private void NewSession(TcpClientSession session)
        {
            session.SentCompleted += (s, e) =>
            {
                IncreaseSentSize(e.Header.DataLength + Header.HEADER_SIZE);
                if (e.Header.Key == MessageKeys.Literal)
                {
                    var msg = (BytesMessage)e;
                    Print($"{e.Header.From}:{MessageHelper.LiteralEncoding.GetString(msg.Bytes)}");
                }
                else if (e.Header.Key == MessageKeys.File)
                {
                    Print($"{e.Header.From}:{e.Header.State}");
                }
            };
            session.Received += (s, e) =>
            {
                if (e.Header.Key == MessageKeys.Literal)
                {
                    var msg = (BytesMessage)e;
                    Print($"{e.Header.From ?? "system"}:{MessageHelper.LiteralEncoding.GetString(msg.Bytes)}");
                }
                else if (e.Header.Key == MessageKeys.Remote)
                {
                    Remote(session, e.Header.From);
                }
                else if (e.Header.Key == MessageKeys.RemoteStop)
                {
                }
                else if (e.Header.Key == MessageKeys.RemoteImage)
                {
                    var bytesMesaage = e as BytesMessage;
                    var terminal = RemoteForm.Get(e.Header.From);
                    if (terminal == null)
                    {
                        session.Send(MessageHelper.CreateRemoteStopMessage(this.userName, e.Header.From, null));
                        return;
                    }
                    if (terminal.Disposing || terminal.IsDisposed)
                    {
                        return;
                    }
                    if (!terminal.Visible)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            terminal.Show();
                        }));
                    }
                    terminal.Draw(bytesMesaage.Bytes);
                    Task.Run(() =>
                    {
                        Thread.Sleep(1000);
                        SendRemoteImageRequest(e.Header.From);
                    });
                }
            };
        }
        private void Remote(TcpClientSession session, string userName)
        {
            // 新建一个和屏幕大小相同的图片
            Bitmap catchBmp = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
            // 创建一个画板，让我们可以在画板上画图
            // 这个画板也就是和屏幕大小一样大的图片
            // 我们可以通过Graphics这个类在这个空白图片上画图
            Graphics g = Graphics.FromImage(catchBmp);
            // 把屏幕图片拷贝到我们创建的空白图片 CatchBmp中
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height));

            var stream = new MemoryStream();
            catchBmp.Save(stream, ImageFormat.Png);
            var array = stream.ToArray();
            var compressed = Helper.Compress(array);
            session.Send(MessageHelper.CreateRemoteImageMessage(compressed, this.userName, userName, null));
            g.Dispose();
            catchBmp.Dispose();
            stream.Dispose();
        }

        private void send_Click(object sender, EventArgs e)
        {
            tcpClient.Session.Send(MessageHelper.CreateLiteralMessage(txtMsg.Text, this.userName, null, "user message"));
        }
        private void Print(string info)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                var index = listBox1.Items.Add(info);
                listBox1.TopIndex = index;
            }));
        }
        private void ReConnect()
        {
            if (canReconnect)
                Task.Run(() =>
                {
                    Print($"reconnect in 3 seconds");
                    Thread.Sleep(3000);
                    tcpClient.Connect();
                });
        }

        private void txtFile_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                txtFile.Text = dialog.FileName;
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtFile.Text))
            {
                Print("file is not exist");
                return;
            }
            Task.Run(() =>
            {
                try
                {
                    var fileStream = new FileStream(txtFile.Text, FileMode.Open);
                    tcpClient.Session.Send(new FileMessage(fileStream, Regex.Match(txtFile.Text, "[^\\\\]+$").Value, userName));
                }
                catch (Exception ex)
                {
                    Print(ex.Message);
                }
            });
        }
        private void IncreaseSentSize(long size)
        {
            Interlocked.Add(ref this.sentSize, size);
            BeginInvoke(new MethodInvoker(() =>
            {
                label1.Text = $"sent : {sentSize} bytes";
            }));
        }

        private void txtConnectRemote_Click(object sender, EventArgs e)
        {
            if (remotes.Any(s => s.To == txtRemoteTarget.Text))
            {
                Print($"You are remoting {txtRemoteTarget.Text} now");
                return;
            }
            if (string.IsNullOrEmpty(txtRemoteTarget.Text))
            {
                Print("please input the username of the user you want to connect to");
                return;
            }
            remotes.Add(RemoteForm.Add(txtRemoteTarget.Text));
            SendRemoteImageRequest(txtRemoteTarget.Text);
        }
        private void SendRemoteImageRequest(string to)
        {
            var msg = MessageHelper.CreateRemoteMessage(this.userName, txtRemoteTarget.Text, "send RemoteImage request");
            tcpClient.Session.Send(msg);
        }
        private void ConnectForm_Shown(object sender, EventArgs e)
        {
            tcpClient.Connect();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            foreach (var i in remotes)
                i.Close();
            canReconnect = false;
            tcpClient.Session.Send(MessageHelper.CreateCloseSessionMessage(userName, "connection client closed"));
            tcpClient.Session.Disconnect("cancelled by user");
            base.OnClosing(e);
        }
    }
}
