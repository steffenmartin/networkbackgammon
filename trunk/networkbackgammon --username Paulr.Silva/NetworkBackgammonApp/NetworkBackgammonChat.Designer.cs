namespace NetworkBackgammon
{
    partial class NetworkBackgammonChat
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
            this.textMsgSendBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.msgTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // textMsgSendBox
            // 
            this.textMsgSendBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textMsgSendBox.Location = new System.Drawing.Point(2, 123);
            this.textMsgSendBox.Multiline = true;
            this.textMsgSendBox.Name = "textMsgSendBox";
            this.textMsgSendBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textMsgSendBox.Size = new System.Drawing.Size(277, 53);
            this.textMsgSendBox.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button1.Location = new System.Drawing.Point(281, 123);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 53);
            this.button1.TabIndex = 2;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // msgTextBox
            // 
            this.msgTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.msgTextBox.Location = new System.Drawing.Point(2, 1);
            this.msgTextBox.Name = "msgTextBox";
            this.msgTextBox.ReadOnly = true;
            this.msgTextBox.Size = new System.Drawing.Size(363, 116);
            this.msgTextBox.TabIndex = 3;
            this.msgTextBox.Text = "";
            // 
            // NetworkBackgammonChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(368, 178);
            this.Controls.Add(this.msgTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textMsgSendBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NetworkBackgammonChat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "NetworkBackgammonChat";
            this.VisibleChanged += new System.EventHandler(this.NetworkBackgammonChat_VisibleChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NetworkBackgammonChat_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textMsgSendBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox msgTextBox;

    }
}