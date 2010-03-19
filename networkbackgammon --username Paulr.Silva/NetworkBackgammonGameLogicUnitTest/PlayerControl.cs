using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetworkBackgammonGameLogic;
using NetworkBackgammonLib;
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammonGameLogicUnitTest
{
    public partial class PlayerControl : UserControl, INetworkBackgammonListener
    {
        INetworkBackgammonListener defaultListener = new NetworkBackgammonListener();

        NetworkBackgammonRemoteGameRoom gameRoom = null;
        NetworkBackgammonPlayer player = null;
        NetworkBackgammonGameSessionEvent.GameSessionEventType lastGameSessionEventType = NetworkBackgammonGameSessionEvent.GameSessionEventType.Invalid;

        public PlayerControl()
        {
            InitializeComponent();
        }
        ~PlayerControl()
        {
            if (gameRoom != null)
            {
                gameRoom.RemoveListener(this);
            }

            gameRoom = null;

            if (player != null)
            {
                player.RemoveListener(this);
            }

            player = null;
        }

        public NetworkBackgammonRemoteGameRoom ConnectedGameRoom
        {
            set
            {
                gameRoom = value;
            }
            get
            {
                return gameRoom;
            }
        }

        private void PlayerControl_Load(object sender, EventArgs e)
        {
            groupBoxGameRoomControls.Enabled = false;
            groupBoxGameControls.Enabled = false;

            if (gameRoom != null)
            {
                gameRoom.AddListener(this);
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (player == null)
            {
                if (textBoxPlayerName.Text.Trim() != "")
                {
                    if (gameRoom != null)
                    {
                        string playerName = textBoxPlayerName.Text.Trim();

                        // Register (if not already)
                        gameRoom.RegisterPlayer(playerName, "password");
                        
                        // Enter the game room
                        player = gameRoom.Enter(playerName, "password");

                        if (player != null)
                        {
                            buttonConnect.Text = "Disconnect";
                            textBoxPlayerName.Enabled = false;
                            groupBoxGameRoomControls.Enabled = true;

                            player.AddListener(this);
                        }
                        else
                        {
                            MessageBox.Show("Login failed!", "Login Error (username)!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Login failed (no game room connection)!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }
                else
                {
                    MessageBox.Show("Login failed (no username specified)!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                if (gameRoom != null)
                {
                    gameRoom.Leave(player);

                    gameRoom.RemoveListener(this);
                    player.RemoveListener(this);

                    player = null;
                }

                buttonConnect.Text = "Connect";
                textBoxPlayerName.Enabled = true;
                groupBoxGameRoomControls.Enabled = false;
            }
        }

        private void buttonAction_Click(object sender, EventArgs e)
        {
            try
            {
                if (player != null)
                {
                    switch (lastGameSessionEventType)
                    {
                        case NetworkBackgammonGameSessionEvent.GameSessionEventType.CheckerUpdated:
                            {
                                if (listBoxCheckers.SelectedItem != null &&
                                    listBoxMoves.SelectedItem != null)
                                {
                                    player.Broadcast(new GameSessionMoveSelectedEvent((NetworkBackgammonChecker)listBoxCheckers.SelectedItem, (NetworkBackgammonDice)listBoxMoves.SelectedItem));
                                }
                            }
                            break;
                        case NetworkBackgammonGameSessionEvent.GameSessionEventType.InitialDiceRolled:
                            {
                                player.Broadcast(new NetworkBackgammonGameSessionEvent(NetworkBackgammonGameSessionEvent.GameSessionEventType.InitialDiceRolledAcknowledged));
                            }
                            break;
                    }
                }

                listBoxMoves.Items.Clear();
            }
            catch (Exception ex)
            {
                listBoxLog.Items.Add(ex.Message);
            }
            finally
            {
                buttonAction.Enabled = false;
                buttonAction.Text = "[No Action]";
                lastGameSessionEventType = NetworkBackgammonGameSessionEvent.GameSessionEventType.Invalid;
            }
        }

        private void listBoxCheckers_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxMoves.Items.Clear();

            try
            {
                NetworkBackgammonChecker selectedChecker = (NetworkBackgammonChecker)listBoxCheckers.SelectedItem;

                foreach (NetworkBackgammonDice diceValue in selectedChecker.PossibleMoves)
                {
                    listBoxMoves.Items.Add(diceValue);
                }
            }
            catch (Exception ex)
            {
                listBoxLog.Items.Add(ex.Message);
            }
        }

        private void textBoxPlayerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                buttonConnect_Click(sender, e);
            }
        }

        private void UpdateConnectedPlayersList()
        {
            listBoxConnectedPlayers.Items.Clear();

            foreach (NetworkBackgammonPlayer p in gameRoom.ConnectedPlayers)
            {
                // Don't add our own player to the list
                if (p.PlayerName != textBoxPlayerName.Text.Trim())
                {
                    listBoxConnectedPlayers.Items.Add(p);
                }
            }
        }

        public delegate void NotifyDelegate(INetworkBackgammonNotifier _notifier, INetworkBackgammonEvent _event);

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
            if (InvokeRequired)
            {
                Invoke(new NotifyDelegate(OnEventNotification), new object[] { sender, e });
            }
            else
            {
                buttonAction.Enabled = false;
                buttonAction.Text = "[No Action]";

                if (sender is NetworkBackgammonRemoteGameRoom)
                {
                    try
                    {
                        if (e is NetworkBackgammonGameRoomEvent)
                        {
                            NetworkBackgammonGameRoomEvent gameRoomEvent = (NetworkBackgammonGameRoomEvent)e;

                            switch (gameRoomEvent.EventType)
                            {
                                case NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerConnected:
                                    UpdateConnectedPlayersList();
                                    break;
                                case NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerDisconnected:
                                    UpdateConnectedPlayersList();
                                    break;
                            }
                        }
                        else if (e is NetworkBackgammonChallengeEvent)
                        {
                            NetworkBackgammonChallengeEvent challengeEvent = (NetworkBackgammonChallengeEvent)e;

                            bool challengeResponse = MessageBox.Show(
                                "Accept game challenge from " + challengeEvent.ChallengingPlayer + "?",
                                "Game Challenge",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button1) == DialogResult.Yes;

                            player.RespondToChallenge(challengeResponse, challengeEvent.ChallengingPlayer);

                            groupBoxGameControls.Enabled = challengeResponse;
                        }
                    }
                    catch (Exception ex)
                    {
                        listBoxLog.Items.Add(ex.Message);
                    }
                }
                else if (sender is NetworkBackgammonGameSession)
                {
                    // Filter out broadcasts from our own player
                    if (sender != player)
                    {
                        try
                        {
                            NetworkBackgammonGameSession gameSession = (NetworkBackgammonGameSession)sender;

                            if (e is NetworkBackgammonGameSessionEvent)
                            {
                                NetworkBackgammonGameSessionEvent gameSessionEvent = (NetworkBackgammonGameSessionEvent)e;

                                // Generate warning if last event has been overwritten
                                // (Currently every event received is supposed to be responded too by means of an
                                //  action (e.g. move, dice roll acknowledge, etc), which resets the last event type to invalid)
                                if (lastGameSessionEventType != NetworkBackgammonGameSessionEvent.GameSessionEventType.Invalid)
                                {
                                    listBoxLog.Items.Add("Warning: Overwriting last event (" + lastGameSessionEventType + ") with new event " + gameSessionEvent.EventType + " (i.e. last event hasn't been responded to by means of an action)");
                                }

                                // Latch the event type
                                lastGameSessionEventType = gameSessionEvent.EventType;

                                switch (gameSessionEvent.EventType)
                                {
                                    case NetworkBackgammonGameSessionEvent.GameSessionEventType.InitialDiceRolled:
                                        {
                                            if (player.InitialDice != null)
                                            {
                                                listBoxLog.Items.Add("Initial dice rolled: " + player.InitialDice.CurrentValue);

                                                buttonAction.Enabled = true;

                                                buttonAction.Text = "Confirm initial dice roll";
                                            }
                                            else
                                            {
                                                listBoxLog.Items.Add("Warning: Event type received is " + gameSessionEvent.EventType + " but dice values are missing");
                                            }
                                        }
                                        break;

                                    case NetworkBackgammonGameSessionEvent.GameSessionEventType.CheckerUpdated:
                                        {
                                            listBoxCheckers.Items.Clear();

                                            if (player.Active)
                                            {
                                                listBoxLog.Items.Add("I'm the active player, expected to make the next move ...");

                                                groupBoxGameControls.BackColor = Color.DarkGreen;

                                                buttonAction.Enabled = true;

                                                buttonAction.Text = "Make Move";
                                            }
                                            else
                                            {
                                                groupBoxGameControls.BackColor = Color.DarkRed;
                                            }

                                            string strDice = "";

                                            foreach (NetworkBackgammonDice d in gameSession.CurrentDice)
                                            {
                                                strDice += " " + d.CurrentValue;
                                            }

                                            listBoxLog.Items.Add("Dice: " + strDice);

                                            foreach (NetworkBackgammonChecker checker in player.Checkers)
                                            {
                                                listBoxCheckers.Items.Add(checker);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            listBoxLog.Items.Add(ex.Message);
                        }
                    }
                }
            }
        }

        #endregion

        private void contextMenuStripConnectedPlayers_Opening(object sender, CancelEventArgs e)
        {
            contextMenuStripConnectedPlayers.Enabled = listBoxConnectedPlayers.SelectedItem != null;
        }

        private void challengeToolStripMenuItemChallenge_Click(object sender, EventArgs e)
        {
            if (gameRoom != null &&
                player != null &&
                listBoxConnectedPlayers.SelectedItem != null)
            {
                NetworkBackgammonPlayer challengedPlayer = (NetworkBackgammonPlayer)listBoxConnectedPlayers.SelectedItem;

                if (gameRoom.Challenge(player.PlayerName, challengedPlayer.PlayerName))
                {
                    groupBoxGameControls.Enabled = true;
                }
                else
                {
                    listBoxLog.Items.Add("Challenge failed (selected opponent rejected or timeout waiting for challenge response)!");
                }
            }
        }
    }
}
