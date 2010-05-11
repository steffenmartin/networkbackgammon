namespace MultipleChannelsSpike
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
            this.buttonChannel1 = new System.Windows.Forms.Button();
            this.buttonChannel2 = new System.Windows.Forms.Button();
            this.richTextBoxLogging = new System.Windows.Forms.RichTextBox();
            this.textBoxChannel1Name = new System.Windows.Forms.TextBox();
            this.textBoxChannel2Name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonChannel1
            // 
            this.buttonChannel1.Location = new System.Drawing.Point(207, 12);
            this.buttonChannel1.Name = "buttonChannel1";
            this.buttonChannel1.Size = new System.Drawing.Size(75, 23);
            this.buttonChannel1.TabIndex = 0;
            this.buttonChannel1.Text = "Register";
            this.buttonChannel1.UseVisualStyleBackColor = true;
            this.buttonChannel1.Click += new System.EventHandler(this.buttonChannel1_Click);
            // 
            // buttonChannel2
            // 
            this.buttonChannel2.Location = new System.Drawing.Point(205, 41);
            this.buttonChannel2.Name = "buttonChannel2";
            this.buttonChannel2.Size = new System.Drawing.Size(75, 23);
            this.buttonChannel2.TabIndex = 1;
            this.buttonChannel2.Text = "Register";
            this.buttonChannel2.UseVisualStyleBackColor = true;
            this.buttonChannel2.Click += new System.EventHandler(this.buttonChannel2_Click);
            // 
            // richTextBoxLogging
            // 
            this.richTextBoxLogging.Location = new System.Drawing.Point(12, 70);
            this.richTextBoxLogging.Name = "richTextBoxLogging";
            this.richTextBoxLogging.ReadOnly = true;
            this.richTextBoxLogging.Size = new System.Drawing.Size(268, 191);
            this.richTextBoxLogging.TabIndex = 2;
            this.richTextBoxLogging.Text = "";
            // 
            // textBoxChannel1Name
            // 
            this.textBoxChannel1Name.Location = new System.Drawing.Point(58, 14);
            this.textBoxChannel1Name.Name = "textBoxChannel1Name";
            this.textBoxChannel1Name.Size = new System.Drawing.Size(143, 20);
            this.textBoxChannel1Name.TabIndex = 3;
            this.textBoxChannel1Name.Text = "Channel1";
            // 
            // textBoxChannel2Name
            // 
            this.textBoxChannel2Name.Location = new System.Drawing.Point(58, 43);
            this.textBoxChannel2Name.Name = "textBoxChannel2Name";
            this.textBoxChannel2Name.Size = new System.Drawing.Size(143, 20);
            this.textBoxChannel2Name.TabIndex = 3;
            this.textBoxChannel2Name.Text = "Channel2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name:";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxChannel2Name);
            this.Controls.Add(this.textBoxChannel1Name);
            this.Controls.Add(this.richTextBoxLogging);
            this.Controls.Add(this.buttonChannel2);
            this.Controls.Add(this.buttonChannel1);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Multiple Channels Spike";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonChannel1;
        private System.Windows.Forms.Button buttonChannel2;
        private System.Windows.Forms.RichTextBox richTextBoxLogging;
        private System.Windows.Forms.TextBox textBoxChannel1Name;
        private System.Windows.Forms.TextBox textBoxChannel2Name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

