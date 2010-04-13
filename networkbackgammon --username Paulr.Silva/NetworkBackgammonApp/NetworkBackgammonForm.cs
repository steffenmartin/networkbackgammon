using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using NetworkBackgammonLib;
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammon
{
    public partial class NetworkBackGammonForm : Form, INetworkBackgammonListener
    {
        INetworkBackgammonListener m_defaultListener = new NetworkBackgammonListener();
        NetworkBackgammonBoard m_backgammonBoard = new NetworkBackgammonBoard();
        NetworkBackgammonChat m_backGammonChat = new NetworkBackgammonChat();
        NetworkBackgammonScoreBoard m_backGammonScoreBoardPlayer1 = new NetworkBackgammonScoreBoard();
        NetworkBackgammonScoreBoard m_backGammonScoreBoardPlayer2 = new NetworkBackgammonScoreBoard();
        NetworkBackgammonLoginForm m_backgammonLogin = new NetworkBackgammonLoginForm();
        delegate void OnQueryChallengeDelegate(string cPlayer);
        delegate void OnChallengeResponseDelegate(bool challengeResponse);
        delegate void OnShowBoardDelegate(bool show);
        delegate void OnGameRoomDelegate(bool show);

        public NetworkBackGammonForm()
        {
            InitializeComponent();
        }

        private void NetworkBackGammonForm_Load(object sender, EventArgs e)
        {
            // Show the Login screen on load
            ShowGameRoomScreen(true);
        }

        // Show the game board display
        private void ShowBoard(bool show)
        {
            if (show)
            {
                m_backgammonBoard.TopLevel = false;
                // Set the Parent Form of the Child window.
                m_backgammonBoard.Parent = this;
                m_backgammonBoard.Top = SystemInformation.CaptionHeight;
                // Display the new form.
                m_backgammonBoard.Show();

                m_backGammonChat.TopLevel = false;
                // Set the Parent Form of the Child window.
                m_backGammonChat.Parent = this;
                m_backGammonChat.Left = m_backgammonBoard.Left;
                m_backGammonChat.Top = m_backgammonBoard.Bottom;
                m_backGammonChat.Width = this.Width - 10;
                // Display the new form.
                m_backGammonChat.Show();

                m_backGammonScoreBoardPlayer1.TopLevel = false;
                // Set the Parent Form of the Child window.
                m_backGammonScoreBoardPlayer1.Parent = this;
                m_backGammonScoreBoardPlayer1.Left = m_backgammonBoard.Right;
                m_backGammonScoreBoardPlayer1.Top = m_backgammonBoard.Top;
                m_backGammonScoreBoardPlayer1.Width = 137;
                m_backGammonScoreBoardPlayer1.Height = m_backgammonBoard.Height / 2;
                // Set the title for the scoreboard
                m_backGammonScoreBoardPlayer1.Title = (NetworkBackgammonClient.Instance.Player != null ? NetworkBackgammonClient.Instance.Player.PlayerName : "?");
                // Display the new form.
                m_backGammonScoreBoardPlayer1.Show();

                m_backGammonScoreBoardPlayer2.TopLevel = false;
                // Set the Parent Form of the Child window.
                m_backGammonScoreBoardPlayer2.Parent = this;
                m_backGammonScoreBoardPlayer2.Left = m_backgammonBoard.Right;
                m_backGammonScoreBoardPlayer2.Top = m_backGammonScoreBoardPlayer1.Bottom;
                m_backGammonScoreBoardPlayer2.Width = 137;
                m_backGammonScoreBoardPlayer2.Height = m_backgammonBoard.Height / 2;
                
                // Get the game session this player is associated with
                NetworkBackgammonPlayer oppPlayer = NetworkBackgammonClient.Instance.GameRoom.GetOpposingPlayer(NetworkBackgammonClient.Instance.Player);

                // Set the title for the scoreboard
                m_backGammonScoreBoardPlayer2.Title = (oppPlayer != null ? oppPlayer.PlayerName : "?");
                // Display the new form.
                m_backGammonScoreBoardPlayer2.Show();
            }
            else
            {
                m_backgammonBoard.Hide();
                m_backGammonChat.Hide();
                m_backGammonScoreBoardPlayer1.Hide();
                m_backGammonScoreBoardPlayer2.Hide();
            }
        }

        // Display the game room display
        private void ShowGameRoomScreen(bool show)
        {
            if (show)
            {
                m_backgammonLogin.TopLevel = false;
                // Set the Parent Form of the Child window.
                m_backgammonLogin.Parent = this;
                m_backgammonLogin.Top = SystemInformation.MenuHeight;
                m_backgammonLogin.Width = this.Width;
                m_backgammonLogin.Height = this.Height - SystemInformation.CaptionHeight - SystemInformation.MenuHeight;
                // Show the login screen 
                m_backgammonLogin.Show();
            }
            else
            {
               m_backgammonLogin.Hide();
            }
        }

        private void gameRoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowGameRoomScreen(!m_backgammonLogin.Visible);
        }

        private void NetworkBackGammonForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Disconnect from the server upon exit
            NetworkBackgammonClient.Instance.DisconnectServer();
        }

        #region INetworkBackgammonListener Members

        public bool AddNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_defaultListener.AddNotifier(notifier);
        }

        public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_defaultListener.RemoveNotifier(notifier);
        }

        public void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e)
        {
            // Challegne event
            /*if (e is NetworkBackgammonChallengeResponseEvent)
            {
                NetworkBackgammonChallengeResponseEvent challengeRespEvent = ((NetworkBackgammonChallengeResponseEvent)e);

                bool challengeAccepted = challengeRespEvent.ChallengeAccepted;

                if ( (NetworkBackgammonClient.Instance.Player.PlayerName.CompareTo(challengeRespEvent.ChallengingPlayer) == 0) ||
                     (NetworkBackgammonClient.Instance.Player.PlayerName.CompareTo(challengeRespEvent.ChallengedPlayer) == 0)  )
                {
                    if (InvokeRequired)
                    {
                        BeginInvoke(new OnChallengeResponseDelegate(ChallengeResponse), challengeAccepted);
                    }
                    else
                    {
                        ChallengeResponse(challengeAccepted);
                    }
                }
            }*/
            if (e is NetworkBackgammonGameRoomEvent)
            {
                switch (((NetworkBackgammonGameRoomEvent)e).EventType)
                {
                    case NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerDisconnected:
                    {
                        // TODO: Check if player that disconnected was the opposing player

                        if (InvokeRequired)
                        {
                            BeginInvoke( new OnGameRoomDelegate(ShowGameRoomScreen), true);
                            BeginInvoke(new OnShowBoardDelegate(ShowBoard), false);
                        }
                        else
                        {
                            ShowGameRoomScreen(true);
                            ShowBoard(false);
                        }
                    }
                    break;
                }
            }
            else if (e is NetworkBackgammonChallengeEvent)
            {
                string challengingPlayer = ((NetworkBackgammonChallengeEvent)e).ChallengingPlayer;
                string challengedPlayer = ((NetworkBackgammonChallengeEvent)e).ChallengedPlayer;

                if (challengingPlayer.CompareTo(NetworkBackgammonClient.Instance.Player.PlayerName) != 0 &&
                    challengedPlayer.CompareTo(NetworkBackgammonClient.Instance.Player.PlayerName) == 0) 
                {
                    if (InvokeRequired)
                    {
                        // In case the caller has called this routine on a different thread
                        BeginInvoke(new OnQueryChallengeDelegate(QueryChallenge), challengingPlayer);
                    }
                    else
                    {
                        QueryChallenge(challengingPlayer);
                    }
                }
            }
            else if (e is GameSessionInitialDiceRollEvent)
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new OnGameRoomDelegate(ShowGameRoomScreen), false);
                    BeginInvoke(new OnShowBoardDelegate(ShowBoard), true);
                }
                else
                {
                    ShowGameRoomScreen(false);
                    ShowBoard(true);
                }
            }
        }

        #endregion

        // Ask the use if they would like to accept a challenge
        private void QueryChallenge(string challengingPlayerName)
        {
            if (MessageBox.Show(this, "Challenge from " + challengingPlayerName + ". Do you accept?", 
                "Challenge", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                NetworkBackgammonClient.Instance.Player.RespondToChallenge(true, challengingPlayerName);

                // TODO: What happens here when the other player cancels his decision to challenge?
            }
            else
            {
                NetworkBackgammonClient.Instance.Player.RespondToChallenge(false, challengingPlayerName);
            }
        }

        // Handle the challenge response from the challenged player
        private void ChallengeResponse(bool challengeResponse)
        {
            /*if (challengeResponse)
            {
                // Close the login window 
                m_backgammonLogin.Hide();
                // Show the board
                ShowBoard(true);
            }
            */
        }
    }
}
