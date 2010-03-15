namespace NetworkBackgammonGameLogicUnitTest
{
    partial class PlayerControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxPlayerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.groupBoxGameControls = new System.Windows.Forms.GroupBox();
            this.listBoxMoves = new System.Windows.Forms.ListBox();
            this.listBoxCheckers = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonMove = new System.Windows.Forms.Button();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.groupBoxGameRoomControls = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listBoxConnectedPlayers = new System.Windows.Forms.ListBox();
            this.contextMenuStripConnectedPlayers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.challengeToolStripMenuItemChallenge = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxGameControls.SuspendLayout();
            this.groupBoxGameRoomControls.SuspendLayout();
            this.contextMenuStripConnectedPlayers.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxPlayerName
            // 
            this.textBoxPlayerName.Location = new System.Drawing.Point(67, 3);
            this.textBoxPlayerName.Name = "textBoxPlayerName";
            this.textBoxPlayerName.Size = new System.Drawing.Size(172, 20);
            this.textBoxPlayerName.TabIndex = 0;
            this.textBoxPlayerName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPlayerName_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Playername:";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(3, 29);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(242, 23);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // groupBoxGameControls
            // 
            this.groupBoxGameControls.Controls.Add(this.listBoxMoves);
            this.groupBoxGameControls.Controls.Add(this.listBoxCheckers);
            this.groupBoxGameControls.Controls.Add(this.label3);
            this.groupBoxGameControls.Controls.Add(this.label2);
            this.groupBoxGameControls.Controls.Add(this.buttonMove);
            this.groupBoxGameControls.Location = new System.Drawing.Point(3, 170);
            this.groupBoxGameControls.Name = "groupBoxGameControls";
            this.groupBoxGameControls.Size = new System.Drawing.Size(242, 166);
            this.groupBoxGameControls.TabIndex = 3;
            this.groupBoxGameControls.TabStop = false;
            this.groupBoxGameControls.Text = "Game Controls";
            // 
            // listBoxMoves
            // 
            this.listBoxMoves.FormattingEnabled = true;
            this.listBoxMoves.Location = new System.Drawing.Point(123, 32);
            this.listBoxMoves.Name = "listBoxMoves";
            this.listBoxMoves.Size = new System.Drawing.Size(108, 95);
            this.listBoxMoves.TabIndex = 2;
            // 
            // listBoxCheckers
            // 
            this.listBoxCheckers.FormattingEnabled = true;
            this.listBoxCheckers.Location = new System.Drawing.Point(9, 32);
            this.listBoxCheckers.Name = "listBoxCheckers";
            this.listBoxCheckers.Size = new System.Drawing.Size(108, 95);
            this.listBoxCheckers.TabIndex = 2;
            this.listBoxCheckers.SelectedIndexChanged += new System.EventHandler(this.listBoxCheckers_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Moves";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Checkers";
            // 
            // buttonMove
            // 
            this.buttonMove.Location = new System.Drawing.Point(9, 133);
            this.buttonMove.Name = "buttonMove";
            this.buttonMove.Size = new System.Drawing.Size(222, 23);
            this.buttonMove.TabIndex = 0;
            this.buttonMove.Text = "Move";
            this.buttonMove.UseVisualStyleBackColor = true;
            this.buttonMove.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // listBoxLog
            // 
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.HorizontalScrollbar = true;
            this.listBoxLog.Location = new System.Drawing.Point(3, 341);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(242, 82);
            this.listBoxLog.TabIndex = 3;
            // 
            // groupBoxGameRoomControls
            // 
            this.groupBoxGameRoomControls.Controls.Add(this.label4);
            this.groupBoxGameRoomControls.Controls.Add(this.listBoxConnectedPlayers);
            this.groupBoxGameRoomControls.Location = new System.Drawing.Point(6, 58);
            this.groupBoxGameRoomControls.Name = "groupBoxGameRoomControls";
            this.groupBoxGameRoomControls.Size = new System.Drawing.Size(239, 106);
            this.groupBoxGameRoomControls.TabIndex = 4;
            this.groupBoxGameRoomControls.TabStop = false;
            this.groupBoxGameRoomControls.Text = "Game Room Controls";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Connected Players:";
            // 
            // listBoxConnectedPlayers
            // 
            this.listBoxConnectedPlayers.ContextMenuStrip = this.contextMenuStripConnectedPlayers;
            this.listBoxConnectedPlayers.FormattingEnabled = true;
            this.listBoxConnectedPlayers.Location = new System.Drawing.Point(108, 19);
            this.listBoxConnectedPlayers.Name = "listBoxConnectedPlayers";
            this.listBoxConnectedPlayers.Size = new System.Drawing.Size(125, 82);
            this.listBoxConnectedPlayers.TabIndex = 0;
            // 
            // contextMenuStripConnectedPlayers
            // 
            this.contextMenuStripConnectedPlayers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.challengeToolStripMenuItemChallenge});
            this.contextMenuStripConnectedPlayers.Name = "contextMenuStripConnectedPlayers";
            this.contextMenuStripConnectedPlayers.Size = new System.Drawing.Size(128, 26);
            this.contextMenuStripConnectedPlayers.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripConnectedPlayers_Opening);
            // 
            // challengeToolStripMenuItemChallenge
            // 
            this.challengeToolStripMenuItemChallenge.Name = "challengeToolStripMenuItemChallenge";
            this.challengeToolStripMenuItemChallenge.Size = new System.Drawing.Size(127, 22);
            this.challengeToolStripMenuItemChallenge.Text = "Challenge";
            this.challengeToolStripMenuItemChallenge.Click += new System.EventHandler(this.challengeToolStripMenuItemChallenge_Click);
            // 
            // PlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxGameRoomControls);
            this.Controls.Add(this.groupBoxGameControls);
            this.Controls.Add(this.listBoxLog);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPlayerName);
            this.Name = "PlayerControl";
            this.Size = new System.Drawing.Size(251, 426);
            this.Load += new System.EventHandler(this.PlayerControl_Load);
            this.groupBoxGameControls.ResumeLayout(false);
            this.groupBoxGameControls.PerformLayout();
            this.groupBoxGameRoomControls.ResumeLayout(false);
            this.groupBoxGameRoomControls.PerformLayout();
            this.contextMenuStripConnectedPlayers.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPlayerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.GroupBox groupBoxGameControls;
        private System.Windows.Forms.ListBox listBoxCheckers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonMove;
        private System.Windows.Forms.ListBox listBoxMoves;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.GroupBox groupBoxGameRoomControls;
        private System.Windows.Forms.ListBox listBoxConnectedPlayers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripConnectedPlayers;
        private System.Windows.Forms.ToolStripMenuItem challengeToolStripMenuItemChallenge;
    }
}
