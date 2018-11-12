namespace Test181107.Client
{
    partial class ConnectForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.send = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnectRemote = new System.Windows.Forms.Button();
            this.txtRemoteTarget = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(-1, 7);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(798, 303);
            this.listBox1.TabIndex = 2;
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(399, 316);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(75, 23);
            this.send.TabIndex = 4;
            this.send.Text = "send";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(13, 320);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(360, 20);
            this.txtMsg.TabIndex = 3;
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(12, 346);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(361, 20);
            this.txtFile.TabIndex = 5;
            this.txtFile.Click += new System.EventHandler(this.txtFile_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.Location = new System.Drawing.Point(399, 344);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(75, 23);
            this.btnSendFile.TabIndex = 6;
            this.btnSendFile.Text = "send file";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 416);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "sent : 0 kb";
            // 
            // txtConnectRemote
            // 
            this.txtConnectRemote.Location = new System.Drawing.Point(93, 381);
            this.txtConnectRemote.Name = "txtConnectRemote";
            this.txtConnectRemote.Size = new System.Drawing.Size(92, 23);
            this.txtConnectRemote.TabIndex = 8;
            this.txtConnectRemote.Text = "connect remote";
            this.txtConnectRemote.UseVisualStyleBackColor = true;
            this.txtConnectRemote.Click += new System.EventHandler(this.txtConnectRemote_Click);
            // 
            // txtRemoteTarget
            // 
            this.txtRemoteTarget.Location = new System.Drawing.Point(12, 383);
            this.txtRemoteTarget.Name = "txtRemoteTarget";
            this.txtRemoteTarget.Size = new System.Drawing.Size(75, 20);
            this.txtRemoteTarget.TabIndex = 9;
            // 
            // ConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtRemoteTarget);
            this.Controls.Add(this.txtConnectRemote);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.send);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.listBox1);
            this.Name = "ConnectForm";
            this.Text = "Connect";
            this.Shown += new System.EventHandler(this.ConnectForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button send;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button txtConnectRemote;
        private System.Windows.Forms.TextBox txtRemoteTarget;
    }
}