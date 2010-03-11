using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetworkBackgammonGameLogic;
using NetworkBackgammonLib;

namespace NetworkBackgammonGameLogicUnitTest
{
    public partial class MainWindow : Form, INetworkBackgammonListener
    {
        INetworkBackgammonListener defaultListener = new NetworkBackgammonListener();

        GameRoom gameRoom = new GameRoom();

        public MainWindow()
        {
            InitializeComponent();

            // Simulate the connection to a game room
            playerControl1.ConnectedGameRoom = gameRoom;
            playerControl2.ConnectedGameRoom = gameRoom;

            ((INetworkBackgammonNotifier)gameRoom).AddListener(this);
        }

        public void UpdateGUI()
        {
            listBoxConnectedPlayers.Items.Clear();

            foreach (Player player in gameRoom.ConnectedPlayers)
            {
                listBoxConnectedPlayers.Items.Add(player);
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
            try
            {
                GameRoomEvent _event = (GameRoomEvent)e;

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
            catch (Exception)
            {
            }
        }

        #endregion

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            gameRoom.Shutdown();
        }
    }
}
