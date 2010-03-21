namespace SpikeDatabase
{
    partial class Client
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
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.pwLabel = new System.Windows.Forms.Label();
            this.playerListBox = new System.Windows.Forms.ListBox();
            this.playerListLabel = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.registerButton = new System.Windows.Forms.Button();
            this.logoutButton = new System.Windows.Forms.Button();
            this.listButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(127, 12);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(197, 20);
            this.userNameTextBox.TabIndex = 0;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(126, 48);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(197, 20);
            this.passwordTextBox.TabIndex = 1;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(20, 14);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(58, 13);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "User name";
            // 
            // pwLabel
            // 
            this.pwLabel.AutoSize = true;
            this.pwLabel.Location = new System.Drawing.Point(20, 45);
            this.pwLabel.Name = "pwLabel";
            this.pwLabel.Size = new System.Drawing.Size(53, 13);
            this.pwLabel.TabIndex = 3;
            this.pwLabel.Text = "Password";
            // 
            // playerListBox
            // 
            this.playerListBox.FormattingEnabled = true;
            this.playerListBox.Location = new System.Drawing.Point(28, 139);
            this.playerListBox.Name = "playerListBox";
            this.playerListBox.Size = new System.Drawing.Size(206, 160);
            this.playerListBox.TabIndex = 4;
            // 
            // playerListLabel
            // 
            this.playerListLabel.AutoSize = true;
            this.playerListLabel.Location = new System.Drawing.Point(26, 111);
            this.playerListLabel.Name = "playerListLabel";
            this.playerListLabel.Size = new System.Drawing.Size(55, 13);
            this.playerListLabel.TabIndex = 5;
            this.playerListLabel.Text = "Player List";
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(126, 74);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(68, 25);
            this.loginButton.TabIndex = 6;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(23, 75);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(79, 24);
            this.registerButton.TabIndex = 7;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // logoutButton
            // 
            this.logoutButton.Location = new System.Drawing.Point(223, 75);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(82, 25);
            this.logoutButton.TabIndex = 8;
            this.logoutButton.Text = "Logout";
            this.logoutButton.UseVisualStyleBackColor = true;
            // 
            // listButton
            // 
            this.listButton.Location = new System.Drawing.Point(247, 144);
            this.listButton.Name = "listButton";
            this.listButton.Size = new System.Drawing.Size(102, 40);
            this.listButton.TabIndex = 9;
            this.listButton.Text = "Get players";
            this.listButton.UseVisualStyleBackColor = true;
            this.listButton.Click += new System.EventHandler(this.listButton_Click);
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 317);
            this.Controls.Add(this.listButton);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.registerButton);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.playerListLabel);
            this.Controls.Add(this.playerListBox);
            this.Controls.Add(this.pwLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.userNameTextBox);
            this.Name = "Client";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label pwLabel;
        private System.Windows.Forms.ListBox playerListBox;
        private System.Windows.Forms.Label playerListLabel;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.Button listButton;
    }
}

