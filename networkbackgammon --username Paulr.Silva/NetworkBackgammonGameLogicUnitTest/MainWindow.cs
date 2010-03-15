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
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammonGameLogicUnitTest
{
    public partial class MainWindow : Form
    {
        NetworkBackgammonRemoteGameRoom gameRoom = new NetworkBackgammonRemoteGameRoom();

        public MainWindow()
        {
            InitializeComponent();

            // Simulate the connection to a game room
            playerControl1.ConnectedGameRoom = gameRoom;
            playerControl2.ConnectedGameRoom = gameRoom;
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            gameRoom.Shutdown();
        }
    }
}
