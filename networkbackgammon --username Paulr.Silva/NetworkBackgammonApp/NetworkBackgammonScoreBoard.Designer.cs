namespace NetworkBackgammon
{
    partial class NetworkBackgammonScoreBoard
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
            this.labelPlayer1Pips = new System.Windows.Forms.Label();
            this.labelPlayer1Score = new System.Windows.Forms.Label();
            this.player1Pips = new System.Windows.Forms.Label();
            this.player1Score = new System.Windows.Forms.Label();
            this.groupPlayerScoreBoard = new System.Windows.Forms.GroupBox();
            this.groupPlayerScoreBoard.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPlayer1Pips
            // 
            this.labelPlayer1Pips.AutoSize = true;
            this.labelPlayer1Pips.Location = new System.Drawing.Point(10, 25);
            this.labelPlayer1Pips.Name = "labelPlayer1Pips";
            this.labelPlayer1Pips.Size = new System.Drawing.Size(30, 13);
            this.labelPlayer1Pips.TabIndex = 0;
            this.labelPlayer1Pips.Text = "Pips:";
            // 
            // labelPlayer1Score
            // 
            this.labelPlayer1Score.AutoSize = true;
            this.labelPlayer1Score.Location = new System.Drawing.Point(10, 48);
            this.labelPlayer1Score.Name = "labelPlayer1Score";
            this.labelPlayer1Score.Size = new System.Drawing.Size(38, 13);
            this.labelPlayer1Score.TabIndex = 1;
            this.labelPlayer1Score.Text = "Score:";
            // 
            // player1Pips
            // 
            this.player1Pips.Location = new System.Drawing.Point(54, 25);
            this.player1Pips.Name = "player1Pips";
            this.player1Pips.Size = new System.Drawing.Size(42, 13);
            this.player1Pips.TabIndex = 2;
            // 
            // player1Score
            // 
            this.player1Score.Location = new System.Drawing.Point(54, 48);
            this.player1Score.Name = "player1Score";
            this.player1Score.Size = new System.Drawing.Size(35, 13);
            this.player1Score.TabIndex = 3;
            // 
            // groupPlayerScoreBoard
            // 
            this.groupPlayerScoreBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPlayerScoreBoard.Controls.Add(this.player1Pips);
            this.groupPlayerScoreBoard.Controls.Add(this.player1Score);
            this.groupPlayerScoreBoard.Controls.Add(this.labelPlayer1Pips);
            this.groupPlayerScoreBoard.Controls.Add(this.labelPlayer1Score);
            this.groupPlayerScoreBoard.Location = new System.Drawing.Point(2, 0);
            this.groupPlayerScoreBoard.Name = "groupPlayerScoreBoard";
            this.groupPlayerScoreBoard.Size = new System.Drawing.Size(116, 109);
            this.groupPlayerScoreBoard.TabIndex = 4;
            this.groupPlayerScoreBoard.TabStop = false;
            this.groupPlayerScoreBoard.Text = "Player Name";
            // 
            // NetworkBackgammonScoreBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(121, 112);
            this.Controls.Add(this.groupPlayerScoreBoard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetworkBackgammonScoreBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "NetworkBackgammonScoreBoard";
            this.Load += new System.EventHandler(this.NetworkBackgammonScoreBoard_Load);
            this.groupPlayerScoreBoard.ResumeLayout(false);
            this.groupPlayerScoreBoard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelPlayer1Pips;
        private System.Windows.Forms.Label labelPlayer1Score;
        private System.Windows.Forms.Label player1Pips;
        private System.Windows.Forms.Label player1Score;
        private System.Windows.Forms.GroupBox groupPlayerScoreBoard;

    }
}