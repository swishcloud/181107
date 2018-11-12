using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test181107.Core;

namespace Test181107.Client
{
    public partial class RemoteForm : Form
    {
        static Dictionary<string, RemoteForm> termimals = new Dictionary<string, RemoteForm>();
        public string To { get; private set; }
        MemoryStream stream = new MemoryStream();
        private RemoteForm(string to)
        {
            InitializeComponent();
            this.To = to;
            this.FormClosing += RemoteForm_FormClosing;
        }

        private void RemoteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            termimals.Remove(To);
        }
        public void Draw(byte[] bytes)
        {
            this.BeginInvoke(new MethodInvoker(() =>
        {
            stream.Dispose();
            stream = new MemoryStream(Helper.Decompress(bytes));
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            var img = Bitmap.FromStream(stream);
            pictureBox1.Image = img;
        }));
        }
        public static RemoteForm Add(string to)
        {
            if (termimals.Keys.Contains(to))
                return null;
            var terminal = new RemoteForm(to);
            RemoteForm.termimals.Add(to, terminal);
            return terminal;
        }
        public static RemoteForm Get(string from)
        {
            RemoteForm terminal;
            RemoteForm.termimals.TryGetValue(from, out terminal);
            return terminal;
        }
    }
}
