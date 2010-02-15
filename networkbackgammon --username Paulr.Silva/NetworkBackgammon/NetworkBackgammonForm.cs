using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetworkBackgammon
{
    public partial class NetworkBackGammonForm : Form
    {
        public NetworkBackGammonForm()
        {
            InitializeComponent();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void NetworkBackGammonForm_ResizeBegin(object sender, EventArgs e)
        {

        }

        private void NetworkBackGammonForm_ResizeEnd(object sender, EventArgs e)
        {

        }

        private void NetworkBackGammonForm_Load(object sender, EventArgs e)
        {
            NetworkBackgammonBoard newMDIChild = new NetworkBackgammonBoard();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;
            // Display the new form.
            newMDIChild.Show();

            NetworkBackgammonChat backGammonChat = new NetworkBackgammonChat();
            // Set the Parent Form of the Child window.
            backGammonChat.MdiParent = this;
            backGammonChat.Left = newMDIChild.Left;
            backGammonChat.Top = newMDIChild.Bottom;
            backGammonChat.Width = this.Width - 2;
            backGammonChat.Height = this.Height - newMDIChild.Height - 2;
            // Display the new form.
            backGammonChat.Show();
        }
    }
}
