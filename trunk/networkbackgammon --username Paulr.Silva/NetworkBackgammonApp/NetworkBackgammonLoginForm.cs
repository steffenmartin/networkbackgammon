using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using NetworkBackgammonLib;
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammon
{
    public partial class NetworkBackgammonLoginForm : Form, INetworkBackgammonListener
    {
        #region Declarations

        /// <summary>
        /// Container for exchanging data relevant for a game challenge between threads
        /// </summary>
        private class GameChallengeDataContainer
        {
            #region Declarations

            /// <summary>
            /// Enumerations of results of a game challenge
            /// </summary>
            public enum ChallengeResultType
            {
                CHALLENGE_ACCEPTED,
                CHALLENGE_REJECTED,
                CHALLENGE_FAILED,
                CHALLENGE_CANCELLED,
                CHALLENGE_UNKNOWN
            }

            #endregion

            #region Members

            /// <summary>
            /// Name of the challenged player
            /// </summary>
            string challengedPlayerName;

            /// <summary>
            /// Results of the game challenge (to be set)
            /// </summary>
            ChallengeResultType challengeResult = ChallengeResultType.CHALLENGE_UNKNOWN;

            #endregion

            #region Methods

            public GameChallengeDataContainer(string _challengedPlayerName)
            {
                challengedPlayerName = _challengedPlayerName;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the name of the challenged player
            /// </summary>
            public string ChallengedPlayerName
            {
                get
                {
                    return challengedPlayerName;
                }
            }

            /// <summary>
            /// Gets or sets the game challenge result
            /// </summary>
            public ChallengeResultType ChallengeResult
            {
                get
                {
                    return challengeResult;
                }
                set
                {
                    challengeResult = value;
                }
            }

            #endregion
        }

        #endregion

        INetworkBackgammonListener defaultListener = new NetworkBackgammonListener();
        NetworkBackgammonWaitDialog waitDlg = null;
        GameChallengeDataContainer gameChallengeThreadExchangeData = null;
        object gameChallengeMutex = new object();
        Thread challengeThread = null;
        delegate void OnNotificationDelegate();
        delegate bool OnChallengeDelegate(NetworkBackgammonPlayer _challengingPlayer, NetworkBackgammonPlayer _challengedPlayer);
        delegate void OnChallengeResponseDelegate();

        // Update list box with players in the game room
        private void UpdateGameRoomList()
        {
            m_gameRoomPlayersListBox.Items.Clear();

            if (NetworkBackgammonClient.Instance.IsConnected)
            {
                // Get all current players in the game room
                for (int i = 0; i < NetworkBackgammonClient.Instance.GameRoom.ConnectedPlayers.Count(); i++)
                {
                    m_gameRoomPlayersListBox.Items.Add(NetworkBackgammonClient.Instance.GameRoom.ConnectedPlayers.ElementAt(i));
                }
            }
            else
            {
                m_gameRoomPlayersListBox.Items.Clear();
                m_gameRoomPlayersListBox.Enabled = false;
                m_playButton.Enabled = false;
            }
        }

        /// <summary>
        /// Handler for a cancellation of a game challenge (e.g. from the wait dialog)
        /// </summary>
        private void OnChallengeCancelled()
        {
            lock (gameChallengeMutex)
            {
                if (gameChallengeThreadExchangeData != null)
                {
                    gameChallengeThreadExchangeData.ChallengeResult = GameChallengeDataContainer.ChallengeResultType.CHALLENGE_CANCELLED;

                    if (InvokeRequired)
                    {
                        BeginInvoke(new OnChallengeResponseDelegate(OnChallengeResponse));
                    }
                    else
                    {
                        OnChallengeResponse();
                    }
                }
            }
        }

        /// <summary>
        /// Handler for a challenge response (incuding cancellation)
        /// </summary>
        private void OnChallengeResponse()
        {
            GameChallengeDataContainer.ChallengeResultType result = GameChallengeDataContainer.ChallengeResultType.CHALLENGE_UNKNOWN;

            bool challengeProceed = false;

            lock (gameChallengeMutex)
            {
                if (gameChallengeThreadExchangeData != null)
                {
                    result = gameChallengeThreadExchangeData.ChallengeResult;

                    gameChallengeThreadExchangeData = null;

                    challengeProceed = true;
                }
            }

            if (waitDlg.Visible) waitDlg.Close();

            if (challengeProceed)
            {
                switch (result)
                {
                    case GameChallengeDataContainer.ChallengeResultType.CHALLENGE_FAILED:
                        MessageBox.Show("Game Challenge Failed!", "Game Challenge", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        break;
                    case GameChallengeDataContainer.ChallengeResultType.CHALLENGE_REJECTED:
                        MessageBox.Show("Game Challenge Rejected!", "Game Challenge", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        break;
                    case GameChallengeDataContainer.ChallengeResultType.CHALLENGE_ACCEPTED:
                        break;
                    case GameChallengeDataContainer.ChallengeResultType.CHALLENGE_CANCELLED:
                        break;
                    default:
                        MessageBox.Show("Game Challenge Error!", "Game Challenge", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        break;
                }

                if (result == GameChallengeDataContainer.ChallengeResultType.CHALLENGE_ACCEPTED)
                {
                    this.DialogResult = DialogResult.OK;
                    Hide();
                }
            }
        }

        // Call the challenge routine on a seperate thread than the GUI thread to prevent blocking
        private void ChallengeThread()
        {
            string tempString = null;

            lock (gameChallengeMutex)
            {
                if (gameChallengeThreadExchangeData != null)
                {
                    tempString = gameChallengeThreadExchangeData.ChallengedPlayerName;
                }
            }

            bool gameChallengeResult = false;

            if (tempString != null)
            {
                gameChallengeResult = NetworkBackgammonClient.Instance.GameRoom.Challenge(NetworkBackgammonClient.Instance.Player.PlayerName, tempString);
            }

            lock (gameChallengeMutex)
            {
                if (gameChallengeThreadExchangeData != null)
                {
                    if (!gameChallengeResult)
                    {
                        gameChallengeThreadExchangeData.ChallengeResult = GameChallengeDataContainer.ChallengeResultType.CHALLENGE_FAILED;

                        if (InvokeRequired)
                        {
                            BeginInvoke(new OnChallengeResponseDelegate(OnChallengeResponse));
                        }
                        else
                        {
                            OnChallengeResponse();
                        }
                    }
                }
            }
        }

        public NetworkBackgammonLoginForm()
        {
            InitializeComponent();

            // Initialize the server property information with default data
            m_ipAddrTextBox.Text = NetworkBackgammonClient.Instance.ServerIpAddress;
            m_portTextBox.Text = NetworkBackgammonClient.Instance.ServerPort;

            // Disable/enable controls for the login group based on whether or not connected to the server
            foreach (Control control in m_loginGroup.Controls)
            {
                control.Enabled = NetworkBackgammonClient.Instance.IsConnected;
            }

            // Fill out the game room list
            UpdateGameRoomList();

            if (NetworkBackgammonClient.Instance.IsConnected)
            {
                // Add self as a listener to game toom events
                NetworkBackgammonClient.Instance.GameRoom.AddListener(this);

                // Enable the player room list based on whether or not we are in a room
                if (NetworkBackgammonClient.Instance.Player != null)
                {
                    // Have this form become a listener of the player's events
                    NetworkBackgammonClient.Instance.Player.AddListener(this);
                }
            }
        }

        private void NetworkBackgammonLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            if (NetworkBackgammonClient.Instance.IsConnected)
            {
                try
                {
                    // Remove self as listener of the player's events
                    if (NetworkBackgammonClient.Instance.Player != null)
                    {
                        NetworkBackgammonClient.Instance.Player.RemoveListener(this);
                    }
                    
                    // Remove self as a listener of game room events
                    if (NetworkBackgammonClient.Instance.GameRoom != null)
                    {
                        NetworkBackgammonClient.Instance.GameRoom.RemoveListener(this);
                    }
                }
                catch
                {
                    // TODO: add exceptiopn handling
                }
            }

            this.Hide();
        }

        #region INetworkBackgammonListener Members

        public bool AddNotifier(INetworkBackgammonNotifier notifier)
        {
            return defaultListener.AddNotifier(notifier);
        }

        public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        {
            return defaultListener.RemoveNotifier(notifier);
        }

        public void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e)
        {
            if (e is NetworkBackgammonGameRoomEvent)
            {
                switch (((NetworkBackgammonGameRoomEvent)e).EventType)
                {
                    case NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerDisconnected:
                    case NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerConnected:
                        {
                            if (InvokeRequired)
                            {
                                // In case the caller has called this routine on a different thread
                                BeginInvoke(new OnNotificationDelegate(UpdateGameRoomList));
                            }
                            else
                            {
                                UpdateGameRoomList();
                            }
                        }
                        break;
                }
            }
           
            //else if ( ((NetworkBackgammonPlayer)sender).PlayerName.CompareTo(NetworkBackgammonClient.Instance.Player.PlayerName) == 0 )
            {
                if (e is NetworkBackgammonChallengeResponseEvent)
                {
                    NetworkBackgammonChallengeResponseEvent challengeRespEvent = ((NetworkBackgammonChallengeResponseEvent)e);

                    bool challengeAccepted = challengeRespEvent.ChallengeAccepted;

                    if (NetworkBackgammonClient.Instance.Player.PlayerName.CompareTo(challengeRespEvent.ChallengingPlayer) == 0)
                    {
                        lock (gameChallengeMutex)
                        {
                            if (gameChallengeThreadExchangeData != null)
                            {
                                gameChallengeThreadExchangeData.ChallengeResult = 
                                    challengeAccepted ?
                                        GameChallengeDataContainer.ChallengeResultType.CHALLENGE_ACCEPTED :
                                        GameChallengeDataContainer.ChallengeResultType.CHALLENGE_REJECTED;
                            }
                        }

                        if (InvokeRequired)
                        {
                            BeginInvoke(new OnChallengeResponseDelegate(OnChallengeResponse));
                        }
                        else
                        {
                            OnChallengeResponse();
                        }
                    }
                }
            }
        }

        #endregion

        // Registration button handler
        private void m_registerRegister_Click(object sender, EventArgs e)
        {
            if (NetworkBackgammonClient.Instance.IsConnected && 
                ( m_usernameTextBox.Text != string.Empty && m_passwordTextBox.Text != string.Empty) )
            {
                if (!NetworkBackgammonClient.Instance.GameRoom.RegisterPlayer(m_usernameTextBox.Text, m_passwordTextBox.Text))
                {
                    MessageBox.Show(NetworkBackgammonClient.Instance.GameRoom.GetPlayerListError());
                }
            }
        }

        // Login Button handler
        private void m_loginButton_Click(object sender, EventArgs e)
        {
            if (NetworkBackgammonClient.Instance.IsConnected)
            {
                // Assign the player object 
                NetworkBackgammonClient.Instance.Player = 
                    NetworkBackgammonClient.Instance.GameRoom.Enter(m_usernameTextBox.Text, 
                                                                    m_passwordTextBox.Text);

                if (NetworkBackgammonClient.Instance.Player == null)
                {
                    MessageBox.Show("Could not login: " + m_usernameTextBox.Text);

                    m_gameRoomPlayersListBox.Enabled = false;
                    m_playButton.Enabled = false;
                }
                else
                {
                    // Register the remotable client object as a listener 
                    NetworkBackgammonClient.Instance.Player.AddListener(this);
                    // Register the parent window...
                    // TODO: Replace with notifier framework event
                    NetworkBackgammonClient.Instance.Player.AddListener(((NetworkBackGammonForm)Parent));

                    m_gameRoomPlayersListBox.Enabled = true;
                    m_playButton.Enabled = true;
                }

                // Fill out the game room list
                UpdateGameRoomList();
            }
        }

        // Connect to the server button handler
        private void m_connectButton_Click(object sender, EventArgs e)
        {
            if (NetworkBackgammonClient.Instance.IsConnected)
            {
                NetworkBackgammonClient.Instance.DisconnectServer();
            }
            else
            {
                NetworkBackgammonClient.Instance.ConnectServer(m_ipAddrTextBox.Text, m_portTextBox.Text);
            }

            bool connected = NetworkBackgammonClient.Instance.IsConnected;

            m_connectButton.Text = (connected ? "Disconnect" : "Connect");
            m_ipAddrTextBox.Enabled = !connected;
            m_portTextBox.Enabled = !connected;

            // Enable all controls in the login group after a successful login
            foreach (Control control in m_loginGroup.Controls)
            {
                control.Enabled = connected;
            }
        }

        private void m_playButton_Click(object sender, EventArgs e)
        {
            int curIndex = m_gameRoomPlayersListBox.SelectedIndex;

            if (curIndex >= 0)
            {
                NetworkBackgammonPlayer challengingPlayer = NetworkBackgammonClient.Instance.Player;
                NetworkBackgammonPlayer challengedPlayer = (NetworkBackgammonPlayer)m_gameRoomPlayersListBox.Items[curIndex];

                string tempChallengingPlayer = String.Copy(challengingPlayer.PlayerName);
                string tempChallengedlayer = String.Copy(challengedPlayer.PlayerName);

                // Setup the wait dialog
                waitDlg = new NetworkBackgammonWaitDialog(new NetworkBackgammonWaitDialog.WaitButtonActionDelegate(OnChallengeCancelled));
                waitDlg.WaitDialogLabel.Text = "Waiting for response from " + challengedPlayer.PlayerName;

                bool challengeProceed = true;

                lock (gameChallengeMutex)
                {
                    // Only execute challenge if none is pending (i.e. if gameChallengeThreadExchangeData object exists)
                    if (gameChallengeThreadExchangeData == null)
                    {
                        // Store challenged player name for thread access
                        gameChallengeThreadExchangeData = new GameChallengeDataContainer(challengedPlayer.PlayerName);
                    }
                    else
                    {
                        challengeProceed = false;
                    }
                }

                if (challengeProceed)
                {
                    // Create the challenge thread
                    challengeThread = new Thread(new ThreadStart(ChallengeThread));
                    // Start the thread
                    challengeThread.Start();

                    // Start up the wait dialog
                    waitDlg.ShowDialog(this);
                }
            }

        }

        private void m_gameRoomPlayersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int curIndex = m_gameRoomPlayersListBox.SelectedIndex;

            if (NetworkBackgammonClient.Instance.Player != null && curIndex >= 0)
            {
                bool samePlayer = (m_gameRoomPlayersListBox.Items[curIndex] == NetworkBackgammonClient.Instance.Player);
                m_playButton.Enabled = (((m_gameRoomPlayersListBox.Items.Count > 0) && !samePlayer) ? true : false);
            }
        }
    }
}
