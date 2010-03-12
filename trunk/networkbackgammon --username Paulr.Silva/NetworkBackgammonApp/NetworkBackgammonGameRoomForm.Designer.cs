namespace NetworkBackgammon
{
    partial class NetworkBackgammonGameRoomForm
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
            this.m_gameRoomPlayerList = new System.Windows.Forms.ListBox();
            this.m_challengeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_gameRoomPlayerList
            // 
            this.m_gameRoomPlayerList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_gameRoomPlayerList.FormattingEnabled = true;
            this.m_gameRoomPlayerList.Location = new System.Drawing.Point(12, 12);
            this.m_gameRoomPlayerList.Name = "m_gameRoomPlayerList";
            this.m_gameRoomPlayerList.Size = new System.Drawing.Size(268, 212);
            this.m_gameRoomPlayerList.TabIndex = 0;
            // 
            // m_challengeButton
            // 
            this.m_challengeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_challengeButton.Location = new System.Drawing.Point(12, 231);
            this.m_challengeButton.Name = "m_challengeButton";
            this.m_challengeButton.Size = new System.Drawing.Size(75, 23);
            this.m_challengeButton.TabIndex = 1;
            this.m_challengeButton.Text = "Challenge";
            this.m_challengeButton.UseVisualStyleBackColor = true;
            this.m_challengeButton.Click += new System.EventHandler(this.m_challengeButton_Click);
            // 
            // NetworkBackgammonGameRoomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.m_challengeButton);
            this.Controls.Add(this.m_gameRoomPlayerList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetworkBackgammonGameRoomForm";
            this.Text = "NetworkBackgammonGameRoomForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox m_gameRoomPlayerList;
        private System.Windows.Forms.Button m_challengeButton;
    }
}