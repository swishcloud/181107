using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test181107.Core;

namespace Test181107.Client
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();

            //initialzie ui
            StartPosition = FormStartPosition.CenterScreen;

            //initialize data
            txtIp.Text = IpAddressHelper.GetHostIp();
            txtPort.Text = "5000";
            Env.Initialize(Print);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var connect = new ConnectForm();
            connect.Connect(txtIp.Text, txtPort.Text);
        }
        private void Print(string info)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                var index = listBox1.Items.Add(info);
                listBox1.TopIndex = index;
            }));
        }
    }
}
