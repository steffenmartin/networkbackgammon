namespace NetworkBackgammon
{
    partial class NetworkBackgammonBoard
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
            this.SuspendLayout();
            // 
            // NetworkBackgammonBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 402);
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NetworkBackgammonBoard";
            this.Text = "NetworkBackgammonBoard";
            this.Load += new System.EventHandler(this.NetworkBackgammonBoard_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NetworkBackgammonBoard_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.NetworkBackgammonBoard_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NetworkBackgammonBoard_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NetworkBackgammonBoard_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

    }
}