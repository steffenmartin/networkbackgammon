using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkBackgammonRemotingLib;
using NetworkBackgammonLib;

namespace NetworkBackgammonServerConnectTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public partial class NetworkBackgammonServerConnectTest
    {
        /// <summary>
        /// The game room (under test)
        /// </summary>
        NetworkBackgammonRemoteGameRoom gameRoom = null;
        // Local delegated listener object
        //INetworkBackgammonListener m_localListener = new NetworkBackgammonListener();
        string ipAddress = "127.0.0.1";
        string port = "8080";

        public NetworkBackgammonServerConnectTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void Initialize()
        {
            // Create game room
            NetworkBackgammonActivateServer myServer = new NetworkBackgammonActivateServer();
            gameRoom = myServer.ActivateServer(port);

        }
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestPlayerGameRoomConnect()
        {
            //
            // TODO: Add test logic	here
            //
            bool retVal = NetworkBackgammonClient.Instance.ConnectServer(ipAddress, port);
            if (!retVal || !NetworkBackgammonClient.Instance.IsConnected)
            {
                //MessageBox.Show("Connection to NetworkBackgammon server failed, check IP and Port");
            }

            bool connected = NetworkBackgammonClient.Instance.IsConnected;

            Assert.IsNotNull(gameRoom, "Game room hasn't been created");
            Assert.IsTrue(gameRoom.VerifyConnection("ack") == "nack");
        }
        //public void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e)
        //{
        //}
        //public bool AddNotifier(INetworkBackgammonNotifier notifier)
        //{
        //    return m_localListener.AddNotifier(notifier);
        //}

        //public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        //{
        //    return m_localListener.RemoveNotifier(notifier);
        //}

    }
}
