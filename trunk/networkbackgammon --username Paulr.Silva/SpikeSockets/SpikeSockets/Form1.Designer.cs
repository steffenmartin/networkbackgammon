namespace SpikeSockets
{
    partial class MyIRC
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
            this.textBoxRemoteAddress = new System.Windows.Forms.TextBox();
            this.labelRemoteAddress = new System.Windows.Forms.Label();
            this.textBoxLocalChat = new System.Windows.Forms.TextBox();
            this.textBoxRemoteChat = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxIPAddress = new System.Windows.Forms.TextBox();
            this.labelIPAddress = new System.Windows.Forms.Label();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxRemoteAddress
            // 
            this.textBoxRemoteAddress.Location = new System.Drawing.Point(12, 36);
            this.textBoxRemoteAddress.Name = "textBoxRemoteAddress";
            this.textBoxRemoteAddress.Size = new System.Drawing.Size(281, 20);
            this.textBoxRemoteAddress.TabIndex = 0;
            // 
            // labelRemoteAddress
            // 
            this.labelRemoteAddress.AutoSize = true;
            this.labelRemoteAddress.Location = new System.Drawing.Point(18, 13);
            this.labelRemoteAddress.Name = "labelRemoteAddress";
            this.labelRemoteAddress.Size = new System.Drawing.Size(85, 13);
            this.labelRemoteAddress.TabIndex = 1;
            this.labelRemoteAddress.Text = "Remote Address";
            // 
            // textBoxLocalChat
            // 
            this.textBoxLocalChat.Location = new System.Drawing.Point(21, 101);
            this.textBoxLocalChat.Multiline = true;
            this.textBoxLocalChat.Name = "textBoxLocalChat";
            this.textBoxLocalChat.Size = new System.Drawing.Size(208, 59);
            this.textBoxLocalChat.TabIndex = 2;
            this.textBoxLocalChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxLocalChat_KeyDown);
            // 
            // textBoxRemoteChat
            // 
            this.textBoxRemoteChat.Location = new System.Drawing.Point(248, 101);
            this.textBoxRemoteChat.Multiline = true;
            this.textBoxRemoteChat.Name = "textBoxRemoteChat";
            this.textBoxRemoteChat.Size = new System.Drawing.Size(204, 151);
            this.textBoxRemoteChat.TabIndex = 3;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(34, 68);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(83, 33);
            this.buttonSend.TabIndex = 4;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxIPAddress
            // 
            this.textBoxIPAddress.Location = new System.Drawing.Point(321, 35);
            this.textBoxIPAddress.Name = "textBoxIPAddress";
            this.textBoxIPAddress.Size = new System.Drawing.Size(150, 20);
            this.textBoxIPAddress.TabIndex = 5;
            // 
            // labelIPAddress
            // 
            this.labelIPAddress.AutoSize = true;
            this.labelIPAddress.Location = new System.Drawing.Point(332, 11);
            this.labelIPAddress.Name = "labelIPAddress";
            this.labelIPAddress.Size = new System.Drawing.Size(58, 13);
            this.labelIPAddress.TabIndex = 6;
            this.labelIPAddress.Text = "IP Address";
            // 
            // buttonAccept
            // 
            this.buttonAccept.Location = new System.Drawing.Point(302, 68);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(134, 32);
            this.buttonAccept.TabIndex = 7;
            this.buttonAccept.Text = "Start Server";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // MyIRC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 266);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.labelIPAddress);
            this.Controls.Add(this.textBoxIPAddress);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxRemoteChat);
            this.Controls.Add(this.textBoxLocalChat);
            this.Controls.Add(this.labelRemoteAddress);
            this.Controls.Add(this.textBoxRemoteAddress);
            this.Name = "MyIRC";
            this.Text = "MyIRC";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MyIRC_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxRemoteAddress;
        private System.Windows.Forms.Label labelRemoteAddress;
        private System.Windows.Forms.TextBox textBoxLocalChat;
        private System.Windows.Forms.TextBox textBoxRemoteChat;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxIPAddress;
        private System.Windows.Forms.Label labelIPAddress;
        private System.Windows.Forms.Button buttonAccept;
    }
}

