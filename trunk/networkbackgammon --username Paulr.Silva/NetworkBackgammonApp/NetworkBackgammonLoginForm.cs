using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using NetworkBackgammonLib;
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammon
{
    public partial class NetworkBackgammonLoginForm : Form, INetworkBackgammonListener
    {
        INetworkBackgammonListener defaultListener = new NetworkBackgammonListener();
        private delegate void OnNotificationDelegate();

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
            }
        }

        private void NetworkBackgammonLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (NetworkBackgammonClient.Instance.IsConnected)
            {
                // Add self as a listener to game toom events
                NetworkBackgammonClient.Instance.GameRoom.RemoveListener(this);
            }
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
            if (sender is NetworkBackgammonRemoteGameRoom)
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
            }
        }

        #endregion

        // Registration button handler
        private void m_registerRegister_Click(object sender, EventArgs e)
        {
            if (NetworkBackgammonClient.Instance.IsConnected)
            {
                if (!NetworkBackgammonClient.Instance.GameRoom.RegisterPlayer(m_usernameTextBox.Text, m_passwordTextBox.Text))
                {
                    MessageBox.Show("Could not register: " + m_usernameTextBox.Text);
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

                // Register the remotable client object as a listner 
                NetworkBackgammonClient.Instance.GameRoom.AddListener(NetworkBackgammonClient.Instance.Player);

                if (NetworkBackgammonClient.Instance.Player == null)
                {
                    MessageBox.Show("Could not login: " + m_usernameTextBox.Text);
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

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

        }
    }
}
