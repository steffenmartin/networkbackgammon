using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkBackgammonRemotingLib;
using NetworkBackgammonLib;

namespace NetworkBackgammonGameLogicUnitTester
{
    /// <summary>
    /// Various tests to verify functionality and validity of the game room
    /// </summary>
    [TestClass]
    public class NetworkBackgammonGameRoomUnitTest
    {
        #region Members

        /// <summary>
        /// Test context
        /// </summary>
        private TestContext testContextInstance;

        /// <summary>
        /// The game room (under test)
        /// </summary>
        NetworkBackgammonRemoteGameRoom gameRoom = null;

        /// <summary>
        /// Controller for (simulating) player 1
        /// </summary>
        NetworkBackgammonPlayerController player1Controller = null;

        /// <summary>
        /// Controller for (simulating) player 2
        /// </summary>
        NetworkBackgammonPlayerController player2Controller = null;

        #endregion

        #region Methods

        #region Test Methods

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void Initialize()
        {
            // Create game room
            gameRoom = new NetworkBackgammonRemoteGameRoom();

            Assert.IsNotNull(gameRoom, "Game room hasn't been created");

            // Create player controllers
            player1Controller = new NetworkBackgammonPlayerController();
            player2Controller = new NetworkBackgammonPlayerController();

            Assert.IsNotNull(player1Controller, "Player 1 controller hasn't been created");
            Assert.IsNotNull(player2Controller, "Player 2 controller hasn't been created");

            // Connect player controllers to game room
            player1Controller.ConnectedGameRoom = gameRoom;
            player2Controller.ConnectedGameRoom = gameRoom;
        }

        /// <summary>
        /// Verifies whether 2 players can connect to and disconnect from a game room incl. the expected notifications
        /// </summary>
        [TestMethod]
        public void TestMethod_GameRoomConnectDisconnect()
        {
            GameRoomConnectBothPlayers();

            // Check the events received by player 1

            // Player 1 should have received a game room event due to the connection of player 2
            Assert.IsTrue(player1Controller.EventQueue.Count > 0, "Player 1 did not received an event");
            KeyValuePair<INetworkBackgammonNotifier, INetworkBackgammonEvent> queueElement = player1Controller.EventQueue.Dequeue();
            Assert.IsInstanceOfType(queueElement.Value, typeof(NetworkBackgammonGameRoomEvent), "Player 1 did not receive a game room event");
            Assert.IsTrue(((NetworkBackgammonGameRoomEvent)(queueElement.Value)).EventType == NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerConnected, "Player 1 did not receive notification of connection of another player");

            // Check the events received by player 2

            // Player 2 should have received no game room event due to the connection of player 2
            Assert.IsTrue(player2Controller.EventQueue.Count == 0, "Player 2 did received an event");

            DisconnectPlayer1();

            // Check the events received by player 2

            // Player 2 should have received a game room event due to the disconnection of player 1
            Assert.IsTrue(player2Controller.EventQueue.Count > 0, "Player 2 did not received an event");
            queueElement = player2Controller.EventQueue.Dequeue();
            Assert.IsInstanceOfType(queueElement.Value, typeof(NetworkBackgammonGameRoomEvent), "Player 2 did not receive a game room event");
            Assert.IsTrue(((NetworkBackgammonGameRoomEvent)(queueElement.Value)).EventType == NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerDisconnected, "Player 2 did not receive notification of disconnection of another player");

            // Check the events received by player 1

            // Player 1 should have received no game room event due to the connection of player 1
            Assert.IsTrue(player1Controller.EventQueue.Count == 0, "Player 1 did received an event");

            DisconnectPlayer2();
        }

        #endregion

        #region Utility Methods

        private void GameRoomConnectBothPlayers()
        {
            ConnectPlayer1();

            ConnectPlayer2();
        }

        private void ConnectPlayer1()
        {
            bool bRetVal;
            string returnMessage = "";

            // Connect player 1 to game room
            bRetVal = player1Controller.OnConnect("Player1", "Player1", ref returnMessage);
            Assert.IsTrue(bRetVal, returnMessage);
        }

        private void ConnectPlayer2()
        {
            bool bRetVal;
            string returnMessage = "";

            // Connect player 2 to game room
            bRetVal = player2Controller.OnConnect("Player2", "Player2", ref returnMessage);
            Assert.IsTrue(bRetVal, returnMessage);
        }

        private void GameRoomDisconnectBothPlayers()
        {
            DisconnectPlayer1();

            DisconnectPlayer2();
        }

        private void DisconnectPlayer1()
        {
            bool bRetVal;
            string returnMessage = "";

            // Disconnect player 1 from game room
            bRetVal = player1Controller.OnDisconnect(ref returnMessage);
            Assert.IsTrue(bRetVal, returnMessage);
        }

        private void DisconnectPlayer2()
        {
            bool bRetVal;
            string returnMessage = "";

            // Disconnect player 2 from game room
            bRetVal = player2Controller.OnDisconnect(ref returnMessage);
            Assert.IsTrue(bRetVal, returnMessage);
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion
    }
}
