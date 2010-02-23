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
            this.textBoxPlayerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.groupBoxGameControls = new System.Windows.Forms.GroupBox();
            this.listBoxMoves = new System.Windows.Forms.ListBox();
            this.listBoxCheckers = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonRollDice = new System.Windows.Forms.Button();
            this.groupBoxGameControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxPlayerName
            // 
            this.textBoxPlayerName.Location = new System.Drawing.Point(67, 3);
            this.textBoxPlayerName.Name = "textBoxPlayerName";
            this.textBoxPlayerName.Size = new System.Drawing.Size(282, 20);
            this.textBoxPlayerName.TabIndex = 0;
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
            this.buttonConnect.Size = new System.Drawing.Size(346, 23);
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
            this.groupBoxGameControls.Controls.Add(this.buttonRollDice);
            this.groupBoxGameControls.Location = new System.Drawing.Point(3, 58);
            this.groupBoxGameControls.Name = "groupBoxGameControls";
            this.groupBoxGameControls.Size = new System.Drawing.Size(346, 245);
            this.groupBoxGameControls.TabIndex = 3;
            this.groupBoxGameControls.TabStop = false;
            this.groupBoxGameControls.Text = "Game Controls";
            // 
            // listBoxMoves
            // 
            this.listBoxMoves.FormattingEnabled = true;
            this.listBoxMoves.Location = new System.Drawing.Point(180, 32);
            this.listBoxMoves.Name = "listBoxMoves";
            this.listBoxMoves.Size = new System.Drawing.Size(157, 173);
            this.listBoxMoves.TabIndex = 2;
            // 
            // listBoxCheckers
            // 
            this.listBoxCheckers.FormattingEnabled = true;
            this.listBoxCheckers.Location = new System.Drawing.Point(9, 32);
            this.listBoxCheckers.Name = "listBoxCheckers";
            this.listBoxCheckers.Size = new System.Drawing.Size(157, 173);
            this.listBoxCheckers.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(177, 16);
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
            // buttonRollDice
            // 
            this.buttonRollDice.Location = new System.Drawing.Point(9, 216);
            this.buttonRollDice.Name = "buttonRollDice";
            this.buttonRollDice.Size = new System.Drawing.Size(328, 23);
            this.buttonRollDice.TabIndex = 0;
            this.buttonRollDice.Text = "Roll Dice / Move";
            this.buttonRollDice.UseVisualStyleBackColor = true;
            this.buttonRollDice.Click += new System.EventHandler(this.buttonRollDice_Click);
            // 
            // PlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxGameControls);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPlayerName);
            this.Name = "PlayerControl";
            this.Size = new System.Drawing.Size(352, 306);
            this.Load += new System.EventHandler(this.PlayerControl_Load);
            this.groupBoxGameControls.ResumeLayout(false);
            this.groupBoxGameControls.PerformLayout();
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
        private System.Windows.Forms.Button buttonRollDice;
        private System.Windows.Forms.ListBox listBoxMoves;
        private System.Windows.Forms.Label label3;
    }
}
