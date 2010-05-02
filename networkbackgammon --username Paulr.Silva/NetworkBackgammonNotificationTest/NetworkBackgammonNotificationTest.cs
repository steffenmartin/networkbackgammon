using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkBackgammonLib;

namespace NetworkBackgammonNotificationTest
{
    /// <summary>
    /// Summary description for NetworkBackgammonNotificationTest
    /// </summary>
    [TestClass]
    public class NetworkBackgammonNotificationTest : INetworkBackgammonListener
    {
        /// <summary>
        /// Handles listening to NetworkBackgammon events
        /// </summary>
        INetworkBackgammonListener m_defaultListener = new NetworkBackgammonListener();
        /// <summary>
        /// Notification object
        /// </summary>
        NetworkBackgammonNotifier m_notiifer;
        /// <summary>
        /// Notification recieved flag
        /// </summary>
        bool m_notifcationRecv = false;

        public NetworkBackgammonNotificationTest()
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
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        #region INetworkBackgammonListener Members

        public bool AddNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_defaultListener.AddNotifier(notifier);
        }

        public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_defaultListener.RemoveNotifier(notifier);
        }

        // Handle incoming notification from a notifier
        public void OnEventNotification(INetworkBackgammonNotifier notifier, INetworkBackgammonEvent e)
        {
            if (e is NetworkBackgammonGameRoomEvent)
            {
                NetworkBackgammonGameRoomEvent grEvent = (NetworkBackgammonGameRoomEvent)e;

                if (grEvent.EventType == NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerConnected)
                {
                    m_notifcationRecv = true;
                }
            }
        }

        #endregion

        [TestMethod]
        public void TestNotification()
        {
            try
            {
                NetworkBackgammonNotifier m_notiifer = new NetworkBackgammonNotifier(null); ;

                m_notiifer.AddListener(this);

                NetworkBackgammonGameRoomEvent nbEvent = new NetworkBackgammonGameRoomEvent(NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerConnected);

                m_notiifer.Broadcast(nbEvent);

                Assert.IsTrue(m_notifcationRecv);

                m_notiifer.RemoveListener(this);
            }
            catch(Exception)
            {
                Assert.IsTrue(false);
            }
        }
    }
}
