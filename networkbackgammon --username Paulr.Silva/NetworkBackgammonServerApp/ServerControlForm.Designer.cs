namespace NetworkBackgammonServer
{
    partial class ServerControlForm
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
            this.ServerControlTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.m_activateServer = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_portText = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.m_clientListView = new System.Windows.Forms.ListView();
            this.Client = new System.Windows.Forms.ColumnHeader();
            this.activity = new System.Windows.Forms.ColumnHeader();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.m_serverLogListBox = new System.Windows.Forms.ListBox();
            this.ServerControlTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ServerControlTab
            // 
            this.ServerControlTab.Controls.Add(this.tabPage1);
            this.ServerControlTab.Controls.Add(this.tabPage2);
            this.ServerControlTab.Controls.Add(this.tabPage3);
            this.ServerControlTab.Location = new System.Drawing.Point(12, 12);
            this.ServerControlTab.Name = "ServerControlTab";
            this.ServerControlTab.SelectedIndex = 0;
            this.ServerControlTab.Size = new System.Drawing.Size(518, 337);
            this.ServerControlTab.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.m_serverLogListBox);
            this.tabPage1.Controls.Add(this.m_activateServer);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(510, 311);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Setup";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // m_activateServer
            // 
            this.m_activateServer.Location = new System.Drawing.Point(6, 77);
            this.m_activateServer.Name = "m_activateServer";
            this.m_activateServer.Size = new System.Drawing.Size(75, 23);
            this.m_activateServer.TabIndex = 1;
            this.m_activateServer.Text = "Start Server";
            this.m_activateServer.UseVisualStyleBackColor = true;
            this.m_activateServer.Click += new System.EventHandler(this.m_activateServer_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.m_portText);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(498, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Properties";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port:";
            // 
            // m_portText
            // 
            this.m_portText.Location = new System.Drawing.Point(41, 23);
            this.m_portText.Name = "m_portText";
            this.m_portText.Size = new System.Drawing.Size(62, 20);
            this.m_portText.TabIndex = 0;
            this.m_portText.Text = "8080";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.m_clientListView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(510, 311);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Clients";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // m_clientListView
            // 
            this.m_clientListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Client,
            this.activity});
            this.m_clientListView.Location = new System.Drawing.Point(6, 6);
            this.m_clientListView.Name = "m_clientListView";
            this.m_clientListView.Size = new System.Drawing.Size(498, 273);
            this.m_clientListView.TabIndex = 0;
            this.m_clientListView.UseCompatibleStateImageBehavior = false;
            this.m_clientListView.View = System.Windows.Forms.View.SmallIcon;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.listView1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(510, 311);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Game Room";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(6, 6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(498, 274);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // m_serverLogListBox
            // 
            this.m_serverLogListBox.FormattingEnabled = true;
            this.m_serverLogListBox.Location = new System.Drawing.Point(6, 106);
            this.m_serverLogListBox.Name = "m_serverLogListBox";
            this.m_serverLogListBox.Size = new System.Drawing.Size(498, 199);
            this.m_serverLogListBox.TabIndex = 2;
            // 
            // ServerControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 361);
            this.Controls.Add(this.ServerControlTab);
            this.Name = "ServerControlForm";
            this.Text = "NetworkBackgammon Server Control";
            this.ServerControlTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl ServerControlTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_portText;
        private System.Windows.Forms.Button m_activateServer;
        private System.Windows.Forms.ListView m_clientListView;
        private System.Windows.Forms.ColumnHeader Client;
        private System.Windows.Forms.ColumnHeader activity;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ListBox m_serverLogListBox;
    }
}

