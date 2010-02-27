namespace RemotingBiDirectionalSpike
{
    partial class MainWindow
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
            this.textBoxServerIPAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxServerPort = new System.Windows.Forms.TextBox();
            this.textBoxServerPortConnect = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonStartServer = new System.Windows.Forms.Button();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.tabControlType = new System.Windows.Forms.TabControl();
            this.tabPageServer = new System.Windows.Forms.TabPage();
            this.tabPageClient = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControlType.SuspendLayout();
            this.tabPageServer.SuspendLayout();
            this.tabPageClient.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxServerIPAddress
            // 
            this.textBoxServerIPAddress.Location = new System.Drawing.Point(73, 6);
            this.textBoxServerIPAddress.Name = "textBoxServerIPAddress";
            this.textBoxServerIPAddress.Size = new System.Drawing.Size(230, 20);
            this.textBoxServerIPAddress.TabIndex = 0;
            this.textBoxServerIPAddress.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP Address:";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(9, 58);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(294, 23);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // textBoxServerPort
            // 
            this.textBoxServerPort.Location = new System.Drawing.Point(41, 6);
            this.textBoxServerPort.Name = "textBoxServerPort";
            this.textBoxServerPort.Size = new System.Drawing.Size(46, 20);
            this.textBoxServerPort.TabIndex = 0;
            this.textBoxServerPort.Text = "8080";
            // 
            // textBoxServerPortConnect
            // 
            this.textBoxServerPortConnect.Location = new System.Drawing.Point(73, 32);
            this.textBoxServerPortConnect.Name = "textBoxServerPortConnect";
            this.textBoxServerPortConnect.Size = new System.Drawing.Size(230, 20);
            this.textBoxServerPortConnect.TabIndex = 0;
            this.textBoxServerPortConnect.Text = "8080";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Server Port:";
            // 
            // buttonStartServer
            // 
            this.buttonStartServer.Location = new System.Drawing.Point(9, 32);
            this.buttonStartServer.Name = "buttonStartServer";
            this.buttonStartServer.Size = new System.Drawing.Size(78, 23);
            this.buttonStartServer.TabIndex = 2;
            this.buttonStartServer.Text = "Start Server";
            this.buttonStartServer.UseVisualStyleBackColor = true;
            this.buttonStartServer.Click += new System.EventHandler(this.buttonStartServer_Click);
            // 
            // listBoxLog
            // 
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.Location = new System.Drawing.Point(12, 205);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(317, 95);
            this.listBoxLog.TabIndex = 3;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(75, 99);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(228, 20);
            this.textBoxMessage.TabIndex = 0;
            this.textBoxMessage.Text = "Hello Server!";
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(9, 125);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(294, 23);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // tabControlType
            // 
            this.tabControlType.Controls.Add(this.tabPageServer);
            this.tabControlType.Controls.Add(this.tabPageClient);
            this.tabControlType.Location = new System.Drawing.Point(12, 12);
            this.tabControlType.Name = "tabControlType";
            this.tabControlType.SelectedIndex = 0;
            this.tabControlType.Size = new System.Drawing.Size(317, 187);
            this.tabControlType.TabIndex = 4;
            // 
            // tabPageServer
            // 
            this.tabPageServer.Controls.Add(this.label2);
            this.tabPageServer.Controls.Add(this.textBoxServerPort);
            this.tabPageServer.Controls.Add(this.buttonStartServer);
            this.tabPageServer.Location = new System.Drawing.Point(4, 22);
            this.tabPageServer.Name = "tabPageServer";
            this.tabPageServer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageServer.Size = new System.Drawing.Size(309, 161);
            this.tabPageServer.TabIndex = 0;
            this.tabPageServer.Text = "Server";
            this.tabPageServer.UseVisualStyleBackColor = true;
            // 
            // tabPageClient
            // 
            this.tabPageClient.Controls.Add(this.label1);
            this.tabPageClient.Controls.Add(this.textBoxServerIPAddress);
            this.tabPageClient.Controls.Add(this.buttonSend);
            this.tabPageClient.Controls.Add(this.label4);
            this.tabPageClient.Controls.Add(this.textBoxMessage);
            this.tabPageClient.Controls.Add(this.label3);
            this.tabPageClient.Controls.Add(this.buttonConnect);
            this.tabPageClient.Controls.Add(this.textBoxServerPortConnect);
            this.tabPageClient.Location = new System.Drawing.Point(4, 22);
            this.tabPageClient.Name = "tabPageClient";
            this.tabPageClient.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageClient.Size = new System.Drawing.Size(309, 161);
            this.tabPageClient.TabIndex = 1;
            this.tabPageClient.Text = "Client";
            this.tabPageClient.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Message:";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 314);
            this.Controls.Add(this.tabControlType);
            this.Controls.Add(this.listBoxLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Bi-Directional Remoting Spike";
            this.tabControlType.ResumeLayout(false);
            this.tabPageServer.ResumeLayout(false);
            this.tabPageServer.PerformLayout();
            this.tabPageClient.ResumeLayout(false);
            this.tabPageClient.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxServerIPAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxServerPort;
        private System.Windows.Forms.TextBox textBoxServerPortConnect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonStartServer;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TabControl tabControlType;
        private System.Windows.Forms.TabPage tabPageServer;
        private System.Windows.Forms.TabPage tabPageClient;
        private System.Windows.Forms.Label label4;
    }
}

