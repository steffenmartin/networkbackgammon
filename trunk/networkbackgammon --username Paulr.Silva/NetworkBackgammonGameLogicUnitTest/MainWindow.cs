using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetworkBackgammonGameLogic;

namespace NetworkBackgammonGameLogicUnitTest
{
    public partial class MainWindow : Form, IGameRoomListener
    {
        GameRoom gameRoom = new GameRoom();

        public MainWindow()
        {
            InitializeComponent();

            // Simulate the connection to a game room
            playerControl1.ConnectedGameRoom = gameRoom;
            playerControl2.ConnectedGameRoom = gameRoom;

            gameRoom.AddListener(this);
        }

        public void UpdateGUI()
        {
            listBoxConnectedPlayers.Items.Clear();

            foreach (Player player in gameRoom.ConnectedPlayers)
            {
                listBoxConnectedPlayers.Items.Add(player);
            }
        }

        #region GameRoomListener Members

        public void Notify(GameRoomEvent _event)
        {
            switch (_event.EventType)
            {
                case GameRoomEvent.GameRoomEventType.PlayerConnected:
                    UpdateGUI();
                    break;
                case GameRoomEvent.GameRoomEventType.PlayerDisconnected:
                    UpdateGUI();
                    break;
            }
        }

        #endregion

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            gameRoom.Shutdown();
        }
    }
}
