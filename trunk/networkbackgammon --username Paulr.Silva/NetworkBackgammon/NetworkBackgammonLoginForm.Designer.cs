namespace NetworkBackgammon
{
    partial class NetworkBackgammonLoginForm
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
            this.m_loginGroup = new System.Windows.Forms.GroupBox();
            this.m_loginButton = new System.Windows.Forms.Button();
            this.m_registerRegister = new System.Windows.Forms.Button();
            this.m_passwordTextBox = new System.Windows.Forms.TextBox();
            this.m_usernameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_connectButton = new System.Windows.Forms.Button();
            this.m_portTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_maskedIPAddressTextBox = new System.Windows.Forms.MaskedTextBox();
            this.m_loginGroup.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_loginGroup
            // 
            this.m_loginGroup.Controls.Add(this.m_loginButton);
            this.m_loginGroup.Controls.Add(this.m_registerRegister);
            this.m_loginGroup.Controls.Add(this.m_passwordTextBox);
            this.m_loginGroup.Controls.Add(this.m_usernameTextBox);
            this.m_loginGroup.Controls.Add(this.label2);
            this.m_loginGroup.Controls.Add(this.label1);
            this.m_loginGroup.Location = new System.Drawing.Point(13, 133);
            this.m_loginGroup.Name = "m_loginGroup";
            this.m_loginGroup.Size = new System.Drawing.Size(237, 170);
            this.m_loginGroup.TabIndex = 0;
            this.m_loginGroup.TabStop = false;
            this.m_loginGroup.Text = "Login";
            // 
            // m_loginButton
            // 
            this.m_loginButton.Location = new System.Drawing.Point(9, 130);
            this.m_loginButton.Name = "m_loginButton";
            this.m_loginButton.Size = new System.Drawing.Size(222, 23);
            this.m_loginButton.TabIndex = 5;
            this.m_loginButton.Text = "Login";
            this.m_loginButton.UseVisualStyleBackColor = true;
            this.m_loginButton.Click += new System.EventHandler(this.m_loginButton_Click);
            // 
            // m_registerRegister
            // 
            this.m_registerRegister.Location = new System.Drawing.Point(9, 101);
            this.m_registerRegister.Name = "m_registerRegister";
            this.m_registerRegister.Size = new System.Drawing.Size(222, 23);
            this.m_registerRegister.TabIndex = 4;
            this.m_registerRegister.Text = "Register";
            this.m_registerRegister.UseVisualStyleBackColor = true;
            this.m_registerRegister.Click += new System.EventHandler(this.m_registerRegister_Click);
            // 
            // m_passwordTextBox
            // 
            this.m_passwordTextBox.Location = new System.Drawing.Point(70, 65);
            this.m_passwordTextBox.Name = "m_passwordTextBox";
            this.m_passwordTextBox.Size = new System.Drawing.Size(161, 20);
            this.m_passwordTextBox.TabIndex = 3;
            this.m_passwordTextBox.UseSystemPasswordChar = true;
            // 
            // m_usernameTextBox
            // 
            this.m_usernameTextBox.Location = new System.Drawing.Point(70, 28);
            this.m_usernameTextBox.Name = "m_usernameTextBox";
            this.m_usernameTextBox.Size = new System.Drawing.Size(161, 20);
            this.m_usernameTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.m_connectButton);
            this.groupBox2.Controls.Add(this.m_portTextBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.m_maskedIPAddressTextBox);
            this.groupBox2.Location = new System.Drawing.Point(13, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(237, 115);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server";
            // 
            // m_connectButton
            // 
            this.m_connectButton.Location = new System.Drawing.Point(9, 86);
            this.m_connectButton.Name = "m_connectButton";
            this.m_connectButton.Size = new System.Drawing.Size(222, 23);
            this.m_connectButton.TabIndex = 4;
            this.m_connectButton.Text = "Connect";
            this.m_connectButton.UseVisualStyleBackColor = true;
            this.m_connectButton.Click += new System.EventHandler(this.m_connectButton_Click);
            // 
            // m_portTextBox
            // 
            this.m_portTextBox.Location = new System.Drawing.Point(73, 57);
            this.m_portTextBox.Name = "m_portTextBox";
            this.m_portTextBox.Size = new System.Drawing.Size(90, 20);
            this.m_portTextBox.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Port:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "IP Address:";
            // 
            // m_maskedIPAddressTextBox
            // 
            this.m_maskedIPAddressTextBox.Location = new System.Drawing.Point(73, 19);
            this.m_maskedIPAddressTextBox.Mask = "000.000.000.000";
            this.m_maskedIPAddressTextBox.Name = "m_maskedIPAddressTextBox";
            this.m_maskedIPAddressTextBox.Size = new System.Drawing.Size(90, 20);
            this.m_maskedIPAddressTextBox.TabIndex = 0;
            // 
            // NetworkBackgammonLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(262, 315);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.m_loginGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetworkBackgammonLoginForm";
            this.ShowIcon = false;
            this.Text = "NetworkBackGammon Login";
            this.m_loginGroup.ResumeLayout(false);
            this.m_loginGroup.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_loginGroup;
        private System.Windows.Forms.TextBox m_passwordTextBox;
        private System.Windows.Forms.TextBox m_usernameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button m_loginButton;
        private System.Windows.Forms.Button m_registerRegister;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MaskedTextBox m_maskedIPAddressTextBox;
        private System.Windows.Forms.Button m_connectButton;
        private System.Windows.Forms.TextBox m_portTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}