using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetworkBackgammonGameLogic;

namespace NetworkBackgammonGameLogicUnitTest
{
    public partial class PlayerControl : UserControl, IGameSessionListener, IPlayerEventInfo
    {
        GameRoom gameRoom = null;
        Player player = null;
        Checker checkerSelected = null;
        Dice moveSelected = null;

        public PlayerControl()
        {
            InitializeComponent();
        }
        ~PlayerControl()
        {
            gameRoom = null;
            player = null;
        }

        public GameRoom ConnectedGameRoom
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
                        player = gameRoom.Login(textBoxPlayerName.Text.Trim());

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
                    gameRoom.Logout(player);

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
                        checkerSelected = (Checker) listBoxCheckers.SelectedItem;
                        moveSelected = (Dice) listBoxMoves.SelectedItem;

                        player.Broadcast(new GameSessionEvent(GameSessionEvent.GameSessionEventType.MoveSelected), player, this);
                    }
                }
            }
            catch (Exception ex)
            {
                listBoxLog.Items.Add(ex.Message);
            }
        }

        public delegate void NotifyDelegate(GameSessionEvent _event, GameSessionSubject _subject, IPlayerEventInfo _playerInfo);

        #region IGameSessionListener Members

        public void Notify(GameSessionEvent _event, GameSessionSubject _subject, IPlayerEventInfo _playerInfo)
        {
            if (InvokeRequired)
            {
                Invoke(new NotifyDelegate(Notify), new object[]{_event, _subject, _playerInfo});
            }
            else
            {
                // Filter out broadcasts from our own player
                if (_subject != player)
                {
                    listBoxCheckers.Items.Clear();

                    try
                    {
                        GameSession gameSession = (GameSession)_subject;

                        foreach (Checker checker in player.Checkers)
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

        private void listBoxCheckers_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxMoves.Items.Clear();

            try
            {
                Checker selectedChecker = (Checker)listBoxCheckers.SelectedItem;

                foreach (Dice diceValue in selectedChecker.PossibleMoves)
                {
                    listBoxMoves.Items.Add(diceValue);
                }
            }
            catch (Exception ex)
            {
                listBoxLog.Items.Add(ex.Message);
            }
        }

        #region IPlayerEventInfo Members

        public Checker GetSelectedChecker()
        {
            return checkerSelected;
        }

        public Dice GetSelectedMove()
        {
            return moveSelected;
        }

        #endregion
    }
}
