namespace NetworkBackgammonGameLogicUnitTest
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
            this.playerControl2 = new NetworkBackgammonGameLogicUnitTest.PlayerControl();
            this.playerControl1 = new NetworkBackgammonGameLogicUnitTest.PlayerControl();
            this.SuspendLayout();
            // 
            // playerControl2
            // 
            this.playerControl2.ConnectedGameRoom = null;
            this.playerControl2.Location = new System.Drawing.Point(268, 12);
            this.playerControl2.Name = "playerControl2";
            this.playerControl2.Size = new System.Drawing.Size(250, 431);
            this.playerControl2.TabIndex = 1;
            // 
            // playerControl1
            // 
            this.playerControl1.ConnectedGameRoom = null;
            this.playerControl1.Location = new System.Drawing.Point(12, 12);
            this.playerControl1.Name = "playerControl1";
            this.playerControl1.Size = new System.Drawing.Size(250, 431);
            this.playerControl1.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 442);
            this.Controls.Add(this.playerControl2);
            this.Controls.Add(this.playerControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private PlayerControl playerControl1;
        private PlayerControl playerControl2;
    }
}

