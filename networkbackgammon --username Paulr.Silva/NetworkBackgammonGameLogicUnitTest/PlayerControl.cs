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
        Type lastGameSessionEventType = null;

        public PlayerControl()
        {
            InitializeComponent();
        }
        ~PlayerControl()
        {
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

                            UpdateConnectedPlayersList();
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
            bool actionPerformed = false;

            try
            {
                if (player != null)
                {
                    if (lastGameSessionEventType == typeof(GameSessionInitialDiceRollEvent))
                    {
                        player.AcknowledgeInitialDiceRoll();

                        actionPerformed = true;
                    }
                    else if (lastGameSessionEventType == typeof(GameSessionMoveExpectedEvent))
                    {
                        if (listBoxCheckers.SelectedItem != null &&
                            listBoxMoves.SelectedItem != null)
                        {
                            player.MakeMove((NetworkBackgammonChecker)listBoxCheckers.SelectedItem, (NetworkBackgammonDice)listBoxMoves.SelectedItem);

                            actionPerformed = true;
                        }
                    }
                    else if (lastGameSessionEventType == typeof(GameSessionNoPossibleMovesEvent))
                    {
                        player.AcknowledgeNoMoves();

                        actionPerformed = true;
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
                if (actionPerformed)
                {
                    buttonAction.Enabled = false;
                    buttonAction.Text = "[No Action]";
                    lastGameSessionEventType = null;
                }
            }
        }

        private void buttonResign_Click(object sender, EventArgs e)
        {
            try
            {
                if (player != null)
                {
                    player.ResignFromGame();
                }
            }
            catch (Exception ex)
            {
                listBoxLog.Items.Add(ex.Message);
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

                #region Sender: Game Room

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

                            string challengingPlayer = challengeEvent.ChallengingPlayer;
                            string challengedPlayer = challengeEvent.ChallengedPlayer;

                            if (challengingPlayer.CompareTo(player.PlayerName) != 0 &&
                                challengedPlayer.CompareTo(player.PlayerName) == 0)
                            {
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
                    }
                    catch (Exception ex)
                    {
                        listBoxLog.Items.Add(ex.Message);
                    }
                }

                #endregion

                #region Sender: Game Session

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
                                // Generate warning if last event has been overwritten
                                // (Currently every event received is supposed to be responded too by means of an
                                //  action (e.g. move, dice roll acknowledge, etc), which resets the last event type to invalid)
                                if (lastGameSessionEventType != null)
                                {
                                    listBoxLog.Items.Add("Warning: Overwriting last event (" + lastGameSessionEventType.ToString() + ") with new event " + e.GetType().ToString() + " (i.e. last event hasn't been responded to by means of an action)");
                                }

                                // Latch the event type
                                lastGameSessionEventType = e.GetType();
                            }

                            #region Actions for initial dice roll

                            if (e is GameSessionInitialDiceRollEvent)
                            {
                                listBoxCheckers.Items.Clear();
                                listBoxMoves.Items.Clear();

                                GameSessionInitialDiceRollEvent gameSessionInitialDiceRollEvent = (GameSessionInitialDiceRollEvent)e;

                                // if (player.InitialDice != null)
                                if (gameSessionInitialDiceRollEvent.GetDiceForPlayer(player.PlayerName) != null)
                                {
                                    listBoxLog.Items.Add("Initial dice rolled: " + gameSessionInitialDiceRollEvent.GetDiceForPlayer(player.PlayerName).CurrentValue);

                                    buttonAction.Enabled = true;

                                    buttonAction.Text = "Confirm initial dice roll";
                                }
                                else
                                {
                                    listBoxLog.Items.Add("Warning: Event received is " + e.GetType().ToString() + " but dice values are missing");
                                }
                            }

                            #endregion

                            #region Actions for checker update

                            else if (e is GameSessionCheckerUpdatedEvent)
                            {
                                listBoxCheckers.Items.Clear();
                                listBoxMoves.Items.Clear();

                                GameSessionCheckerUpdatedEvent gameSessionCheckerUpdateEvent = (GameSessionCheckerUpdatedEvent)e;

                                player = gameSessionCheckerUpdateEvent.GetPlayerByName(player.PlayerName);

                                string strDice = "";

                                foreach (NetworkBackgammonDice d in gameSessionCheckerUpdateEvent.DiceRolled)
                                {
                                    strDice += " " + d.CurrentValue;
                                }

                                listBoxLog.Items.Add("Dice: " + strDice);

                                foreach (NetworkBackgammonChecker checker in player.Checkers)
                                {
                                    listBoxCheckers.Items.Add(checker);
                                }
                            }

                            #endregion

                            #region Actions for move expected

                            else if (e is GameSessionMoveExpectedEvent)
                            {
                                GameSessionMoveExpectedEvent gameSessionMoveExpectedEvent = (GameSessionMoveExpectedEvent)e;

                                if (player.PlayerName == gameSessionMoveExpectedEvent.ActivePlayer)
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
                            }

                            #endregion

                            #region Actions for no (valid) move

                            else if (e is GameSessionNoPossibleMovesEvent)
                            {
                                GameSessionNoPossibleMovesEvent gameSessionNoPossibleMovesEvent = (GameSessionNoPossibleMovesEvent)e;

                                if (player.PlayerName == gameSessionNoPossibleMovesEvent.PlayerName)
                                {
                                    listBoxLog.Items.Add("I'm the active player, but have no (valid) moves ...");

                                    groupBoxGameControls.BackColor = Color.DarkGreen;

                                    buttonAction.Enabled = true;

                                    buttonAction.Text = "Confirm";
                                }
                                else
                                {
                                    groupBoxGameControls.BackColor = Color.DarkRed;
                                }
                            }

                            #endregion

                            #region Actions for player resignation

                            else if (e is GameSessionPlayerResignationEvent)
                            {
                                listBoxCheckers.Items.Clear();
                                listBoxMoves.Items.Clear();

                                GameSessionPlayerResignationEvent gameSessionPlayerResignationEvent = (GameSessionPlayerResignationEvent)e;

                                listBoxLog.Items.Add("Player " + gameSessionPlayerResignationEvent.ResigningPlayer + " has resigned from current game");

                                listBoxCheckers.Items.Clear();

                                groupBoxGameControls.BackColor = SystemColors.Control;

                                groupBoxGameControls.Enabled = false;

                            }

                            #endregion

                            #region Actions for player won game

                            else if (e is GameSessionPlayerWonEvent)
                            {
                                listBoxCheckers.Items.Clear();
                                listBoxMoves.Items.Clear();

                                GameSessionPlayerWonEvent gameSessionPlayerWonEvent = (GameSessionPlayerWonEvent)e;

                                if (gameSessionPlayerWonEvent.WinningPlayer == player.PlayerName)
                                {
                                    listBoxLog.Items.Add("Yeah!!! I won the game!!!");
                                }
                                else
                                {
                                    listBoxLog.Items.Add("Player " + gameSessionPlayerWonEvent.WinningPlayer + " won the game");
                                }

                                listBoxCheckers.Items.Clear();

                                groupBoxGameControls.BackColor = SystemColors.Control;

                                groupBoxGameControls.Enabled = false;
                            }

                            #endregion

                            else if (e is NetworkBackgammonGameSessionEvent)
                            {
                                listBoxCheckers.Items.Clear();
                                listBoxMoves.Items.Clear();

                                NetworkBackgammonGameSessionEvent gameSessionEvent = (NetworkBackgammonGameSessionEvent)e;

                                switch (gameSessionEvent.EventType)
                                {
                                    case NetworkBackgammonGameSessionEvent.GameSessionEventType.GameFinished:
                                        {
                                            listBoxLog.Items.Add("Game finished");
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

                #endregion
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

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBoxLog.Items.Clear();
        }
    }
}
