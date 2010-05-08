using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Net;
using System.Net.Sockets;

using NetworkBackgammonLib;
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammonServer
{
    public partial class ServerControlForm : Form, INetworkBackgammonListener
    {
        // Channel the server is connected to
        private HttpChannel m_channel = null;
        // Local server object
        private NetworkBackgammonRemoteGameRoom m_server = null;
        // Local delegated listener object
        INetworkBackgammonListener m_localListener = new NetworkBackgammonListener();
        // Log Message Delegate
        private delegate void OnLogMessageDelegate(string _message);
        // Update Player List Delegate
        private delegate void UpdatePlayerListDelegate();

        DataTable playerListTable;

        public ServerControlForm()
        {
            InitializeComponent();

            playerListTable = new DataTable();
            InitializePlayerTable();
        }

        private void InitializePlayerTable()
        {
            // Create table columns
            DataColumn myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ReadOnly = false;
            myColumn.Caption = "Player Name";
            myColumn.ColumnName = "PlayerName";
            playerListTable.Columns.Add(myColumn);

            myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ReadOnly = false;
            myColumn.Caption = "Player Password";
            myColumn.ColumnName = "Password";
            playerListTable.Columns.Add(myColumn);

            myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ReadOnly = false;
            myColumn.Caption = "Player Login";
            myColumn.ColumnName = "Login";
            playerListTable.Columns.Add(myColumn);

            myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ReadOnly = false;
            myColumn.Caption = "Opponent";
            myColumn.ColumnName = "Opponent";
            playerListTable.Columns.Add(myColumn);

            playerListGridView.DataSource = playerListTable;

        }

        /**
        *   Activate/Deactivate server
        */
        private bool ActivateServer(bool activate)
        {
            bool retval = false;

            try
            {
                if (activate)
                {
                    if (m_channel == null)
                    {
                        // We need to use binary formatters, which allow the serialization of generic collections
                        BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
                        serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                        BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

                        IDictionary props = new Hashtable();
                        props["port"] = Convert.ToInt32(m_portText.Text);

                        m_channel = new HttpChannel(props, clientProv, serverProv);
                    }

                    ChannelServices.RegisterChannel(m_channel, false);
                    RemotingConfiguration.RegisterWellKnownServiceType( typeof(NetworkBackgammonRemoteGameRoom), "GameRoom", WellKnownObjectMode.Singleton);

                    // Assign the instantiated remote server to the local server
                    MarshalByRefObject obj = (MarshalByRefObject)RemotingServices.Connect(typeof(NetworkBackgammonRemoteGameRoom), "http://127.0.0.1:" + m_portText.Text + "/GameRoom");
                    m_server = obj as NetworkBackgammonRemoteGameRoom;

                    // Register delegated listener as a listener of the server object
                    retval = m_server.AddListener(this);
            
                    m_activateServer.Text = "Stop Server";
                    m_portText.Enabled = false;
                  
                    // Log status message 
                    Log("Server Started...");

                    UpdateList();
                }
                else
                {
                    // Stop listening to the server
                    m_server.RemoveListener(this);
                    m_server.Shutdown();
                    m_server = null;

                    if (m_channel != null)
                    {
                        ChannelServices.UnregisterChannel(m_channel);

                        m_channel = null;
                    }

                    m_activateServer.Text = "Start Server";
                    m_portText.Enabled = true;

                    // Log status message 
                    Log("Server Stopped...");

                    retval = true;
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

            return retval;
        }

        private void m_activateServer_Click(object sender, EventArgs e)
        {
            ActivateServer( (m_activateServer.Text == "Start Server" ? true : false) );
        }

        // Log a local message
        private void Log(string message)
        {
            m_serverLogListBox.Items.Add(message);
        }

        /// <summary>
        /// Call GameRoom to get list of logged on players
        /// </summary>
        private void UpdateList()
        {
            // Build DataView of player list
            playerListTable.Rows.Clear();
            ArrayList registeredPlayers = m_server.GetRegisteredPlayers();
            List<NetworkBackgammonPlayer> activePlayers;
            activePlayers = m_server.ConnectedPlayers;
            foreach (string regPlayer in registeredPlayers)
            {
                DataRow playerRow = playerListTable.NewRow();
                playerRow["PlayerName"] = regPlayer;
                foreach (NetworkBackgammonPlayer player in activePlayers)
                {
                    if (regPlayer.ToLower().Equals(player.PlayerName.ToLower()))
                    {
                        playerRow["Login"] = true;
                        if (m_server.GetOpposingPlayer(player) != null)
                        {
                            string otherPlayer = (m_server.GetOpposingPlayer(player)).PlayerName;
                            playerRow["Opponent"] = otherPlayer;
                        }
                    }
                }
                playerListTable.Rows.Add(playerRow);
            }
            playerListGridView.Update();
        }

        #region INetworkBackgammonListener Members

        public bool AddNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_localListener.AddNotifier(notifier);
        }

        public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_localListener.RemoveNotifier(notifier);
        }

        public void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e)
        {
            if (InvokeRequired)
            {
                // In case the caller has called this routine on a different thread
                BeginInvoke(new OnLogMessageDelegate(Log), sender.ToString() + e.ToString());

                BeginInvoke(new OnLogMessageDelegate(Log), ("***** Event ***** "));
                BeginInvoke(new OnLogMessageDelegate(Log), "Sender: " + sender.ToString());
                BeginInvoke(new OnLogMessageDelegate(Log), "Event: " + e.ToString());
                if (e is NetworkBackgammonGameRoomEvent)
                {
                    NetworkBackgammonGameRoomEvent eGameRoom = (NetworkBackgammonGameRoomEvent)e;
                    if (eGameRoom.EventType == 
                                NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerConnected
                            || eGameRoom.EventType == 
                                NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerDisconnected
                            || eGameRoom.EventType == 
                                NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerPlaying
                            || eGameRoom.EventType == 
                                NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerFinished)
                        BeginInvoke(new UpdatePlayerListDelegate(UpdateList));
                }
            }
            else
            {
                Log("***** Event ***** ");
                Log("Sender: " + sender.ToString());
                Log("Event: " + e.ToString());

                if (e is NetworkBackgammonGameRoomEvent)
                {
                    NetworkBackgammonGameRoomEvent eGameRoom = (NetworkBackgammonGameRoomEvent)e;
                    if (eGameRoom.EventType ==
                            NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerConnected
                        || eGameRoom.EventType ==
                            NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerDisconnected
                        || eGameRoom.EventType == 
                            NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerPlaying
                        || eGameRoom.EventType == 
                            NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerFinished)
                    {
                        UpdateList();
                    }
                }
            }
        }

        #endregion

        private void ServerControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ActivateServer(false);
        }

        /// <summary>
        /// Terminate all game sessions, but keep Game room open for future logins and players
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void killGamesButton_Click(object sender, EventArgs e)
        {
            m_server.Shutdown();
            UpdateList();
        }

    }
}
