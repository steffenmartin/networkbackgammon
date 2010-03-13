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

namespace NetworkBackgammonGameLogicUnitTest
{
    public partial class PlayerControl : UserControl, INetworkBackgammonListener
    {
        INetworkBackgammonListener defaultListener = new NetworkBackgammonListener();

        NetworkBackgammonGameRoom gameRoom = null;
        NetworkBackgammonPlayer player = null;

        public PlayerControl()
        {
            InitializeComponent();
        }
        ~PlayerControl()
        {
            gameRoom = null;
            player = null;
        }

        public NetworkBackgammonGameRoom ConnectedGameRoom
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
                        player = gameRoom.Enter(textBoxPlayerName.Text.Trim());

                        if (player != null)
                        {
                            buttonConnect.Text = "Disconnect";
                            textBoxPlayerName.Enabled = false;
                            groupBoxGameControls.Enabled = true;

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

                    player.RemoveListener(this);

                    player = null;
                }

                buttonConnect.Text = "Connect";
                textBoxPlayerName.Enabled = true;
                groupBoxGameControls.Enabled = false;
            }
        }

        private void buttonMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (player != null)
                {
                    if (listBoxCheckers.SelectedItem != null &&
                        listBoxMoves.SelectedItem != null)
                    {
                        player.Broadcast(new GameSessionMoveSelectedEvent((NetworkBackgammonChecker)listBoxCheckers.SelectedItem, (NetworkBackgammonDice)listBoxMoves.SelectedItem));
                    }
                }

                listBoxMoves.Items.Clear();
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
                // Filter out broadcasts from our own player
                if (sender != player)
                {
                    listBoxCheckers.Items.Clear();

                    try
                    {
                        NetworkBackgammonGameSession gameSession = (NetworkBackgammonGameSession)sender;

                        if (player.Active)
                        {
                            listBoxLog.Items.Add("I'm the active player, expected to make the next move ...");
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
                    catch (Exception ex)
                    {
                        listBoxLog.Items.Add(ex.Message);
                    }
                }
            }
        }

        #endregion
    }
}
