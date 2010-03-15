namespace NetworkBackgammon
{
    partial class NetworkBackgammonWaitDialog
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
            this.m_waitlabel = new System.Windows.Forms.Label();
            this.m_cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_waitlabel
            // 
            this.m_waitlabel.AutoSize = true;
            this.m_waitlabel.Location = new System.Drawing.Point(13, 23);
            this.m_waitlabel.Name = "m_waitlabel";
            this.m_waitlabel.Size = new System.Drawing.Size(58, 13);
            this.m_waitlabel.TabIndex = 0;
            this.m_waitlabel.Text = "wait text....";
            // 
            // m_cancelButton
            // 
            this.m_cancelButton.Location = new System.Drawing.Point(103, 65);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new System.Drawing.Size(75, 23);
            this.m_cancelButton.TabIndex = 1;
            this.m_cancelButton.Text = "Cancel";
            this.m_cancelButton.UseVisualStyleBackColor = true;
            this.m_cancelButton.Click += new System.EventHandler(this.m_cancelButton_Click);
            // 
            // NetworkBackgammonWaitDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 100);
            this.ControlBox = false;
            this.Controls.Add(this.m_cancelButton);
            this.Controls.Add(this.m_waitlabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "NetworkBackgammonWaitDialog";
            this.Text = "NetworkBackgammonWaitDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_waitlabel;
        private System.Windows.Forms.Button m_cancelButton;
    }
}