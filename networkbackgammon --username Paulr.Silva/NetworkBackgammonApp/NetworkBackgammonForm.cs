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
        NetworkBackgammonBoard m_backgammonBoard = new NetworkBackgammonBoard();
        NetworkBackgammonChat m_backGammonChat = new NetworkBackgammonChat();
        NetworkBackgammonScoreBoard m_backGammonScoreBoardPlayer1 = new NetworkBackgammonScoreBoard();
        NetworkBackgammonScoreBoard m_backGammonScoreBoardPlayer2 = new NetworkBackgammonScoreBoard();
        
        public NetworkBackGammonForm()
        {
            InitializeComponent();
        }

        private void NetworkBackGammonForm_Load(object sender, EventArgs e)
        { 
    
        }

        private void LoadGameBoard()
        { 
            // Set the Parent Form of the Child window.
            m_backgammonBoard.MdiParent = this;
            // Display the new form.
            m_backgammonBoard.Show();
             
            // Set the Parent Form of the Child window.
            m_backGammonChat.MdiParent = this;
            m_backGammonChat.Left = m_backgammonBoard.Left;
            m_backGammonChat.Top = m_backgammonBoard.Bottom;
            m_backGammonChat.Width = this.Width - 10;
            // Display the new form.
            m_backGammonChat.Show();

            // Set the Parent Form of the Child window.
            m_backGammonScoreBoardPlayer1.MdiParent = this;
            m_backGammonScoreBoardPlayer1.Left = m_backgammonBoard.Right;
            m_backGammonScoreBoardPlayer1.Top = m_backgammonBoard.Top;
            m_backGammonScoreBoardPlayer1.Width = 137;
            m_backGammonScoreBoardPlayer1.Height = m_backgammonBoard.Height / 2;
            // Display the new form.
            m_backGammonScoreBoardPlayer1.Show();

            // Set the Parent Form of the Child window.
            m_backGammonScoreBoardPlayer2.MdiParent = this;
            m_backGammonScoreBoardPlayer2.Left = m_backgammonBoard.Right;
            m_backGammonScoreBoardPlayer2.Top = m_backGammonScoreBoardPlayer1.Bottom;
            m_backGammonScoreBoardPlayer2.Width = 137;
            m_backGammonScoreBoardPlayer2.Height = m_backgammonBoard.Height / 2;
            // Display the new form.
            m_backGammonScoreBoardPlayer2.Show();
        }

        private void UnloadGameBoard()
        {
            m_backgammonBoard.Hide();
            m_backGammonChat.Hide();
            m_backGammonScoreBoardPlayer1.Hide();
            m_backGammonScoreBoardPlayer2.Hide();
        }

        private void serverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkBackgammonLoginForm backgammonLogin = new NetworkBackgammonLoginForm();
            DialogResult res = backgammonLogin.ShowDialog();
        }

        private void gameRoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NetworkBackgammonGameRoomForm backgammonGameRoom = new NetworkBackgammonGameRoomForm();
            DialogResult res = backgammonGameRoom.ShowDialog();

            // Load the game on successful selection of a opponent
            LoadGameBoard();
        }

        private void NetworkBackGammonForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Disconnect from the server upon exit
            NetworkBackgammonClient.Instance.DisconnectServer();
        }
    }
}
