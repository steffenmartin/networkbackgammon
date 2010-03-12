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
            NetworkBackgammonLoginForm backgammonLogin = new NetworkBackgammonLoginForm();
            DialogResult res = backgammonLogin.ShowDialog();

            if (res == DialogResult.OK)
            {
                NetworkBackgammonGameRoomForm gameRoomList = new NetworkBackgammonGameRoomForm();
                // Set the Parent Form of the Child window.
                gameRoomList.MdiParent = this;
                gameRoomList.Left = 0;
                gameRoomList.Top = 0;
                gameRoomList.Width = this.Width;
                gameRoomList.Height = this.Height;
                // Display the new form.
                gameRoomList.Show();

                /*
                 
                 NetworkBackgammonBoard backgammonBoard = new NetworkBackgammonBoard();
                // Set the Parent Form of the Child window.
                backgammonBoard.MdiParent = this;
                // Display the new form.
                backgammonBoard.Show();
                 * 
                 * NetworkBackgammonChat backGammonChat = new NetworkBackgammonChat();
                // Set the Parent Form of the Child window.
                backGammonChat.MdiParent = this;
                backGammonChat.Left = backgammonBoard.Left;
                backGammonChat.Top = backgammonBoard.Bottom;
                backGammonChat.Width = this.Width - 10;
                //backGammonChat.Height = backGammonChat.Height ;
                // Display the new form.
                backGammonChat.Show();

                NetworkBackgammonScoreBoard backGammonScoreBoardPlayer1 = new NetworkBackgammonScoreBoard();
                // Set the Parent Form of the Child window.
                backGammonScoreBoardPlayer1.MdiParent = this;
                backGammonScoreBoardPlayer1.Left = backgammonBoard.Right;
                backGammonScoreBoardPlayer1.Top = backgammonBoard.Top;
                backGammonScoreBoardPlayer1.Width = 137;
                backGammonScoreBoardPlayer1.Height = backgammonBoard.Height / 2;
                // Display the new form.
                backGammonScoreBoardPlayer1.Show();

                NetworkBackgammonScoreBoard backGammonScoreBoardPlayer2 = new NetworkBackgammonScoreBoard();
                // Set the Parent Form of the Child window.
                backGammonScoreBoardPlayer2.MdiParent = this;
                backGammonScoreBoardPlayer2.Left = backgammonBoard.Right;
                backGammonScoreBoardPlayer2.Top = backGammonScoreBoardPlayer1.Bottom;
                backGammonScoreBoardPlayer2.Width = 137;
                backGammonScoreBoardPlayer2.Height = backgammonBoard.Height / 2;
                // Display the new form.
                backGammonScoreBoardPlayer2.Show();
                */
            }
        }
    }
}
