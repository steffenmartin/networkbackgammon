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
            this.playerControl1 = new NetworkBackgammonGameLogicUnitTest.PlayerControl();
            this.playerControl2 = new NetworkBackgammonGameLogicUnitTest.PlayerControl();
            this.listBoxConnectedPlayers = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // playerControl1
            // 
            this.playerControl1.Location = new System.Drawing.Point(12, 12);
            this.playerControl1.Name = "playerControl1";
            this.playerControl1.Size = new System.Drawing.Size(352, 306);
            this.playerControl1.TabIndex = 0;
            // 
            // playerControl2
            // 
            this.playerControl2.Location = new System.Drawing.Point(378, 12);
            this.playerControl2.Name = "playerControl2";
            this.playerControl2.Size = new System.Drawing.Size(352, 306);
            this.playerControl2.TabIndex = 1;
            // 
            // listBoxConnectedPlayers
            // 
            this.listBoxConnectedPlayers.FormattingEnabled = true;
            this.listBoxConnectedPlayers.Location = new System.Drawing.Point(12, 337);
            this.listBoxConnectedPlayers.Name = "listBoxConnectedPlayers";
            this.listBoxConnectedPlayers.Size = new System.Drawing.Size(718, 95);
            this.listBoxConnectedPlayers.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Connected Players";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 526);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxConnectedPlayers);
            this.Controls.Add(this.playerControl2);
            this.Controls.Add(this.playerControl1);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PlayerControl playerControl1;
        private PlayerControl playerControl2;
        private System.Windows.Forms.ListBox listBoxConnectedPlayers;
        private System.Windows.Forms.Label label1;
    }
}

