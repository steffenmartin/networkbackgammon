using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkBackgammonRemotingLib;
using NetworkBackgammonLib;

namespace NetworkBackgammonGameLogicUnitTester
{
    /// <summary>
    /// Test utility class to control (simulate) the behavior of a player
    /// </summary>
    public class NetworkBackgammonPlayerController : INetworkBackgammonListener
    {
        #region Members

        /// <summary>
        /// The game room this player is (going to be) connected to
        /// </summary>
        private NetworkBackgammonRemoteGameRoom gameRoom = null;

        /// <summary>
        /// The player object as returned by the game room on connect
        /// </summary>
        private NetworkBackgammonPlayer player = null;

        /// <summary>
        /// Default network backgammon listener
        /// </summary>
        INetworkBackgammonListener defaultListener = new NetworkBackgammonListener();

        /// <summary>
        /// Queue for incoming events from the game room
        /// </summary>
        Queue<KeyValuePair<INetworkBackgammonNotifier, INetworkBackgammonEvent>> eventQueue = new Queue<KeyValuePair<INetworkBackgammonNotifier,INetworkBackgammonEvent>>();

        #endregion

        #region Methods

        public bool OnConnect(string playerName, string playerPassword, ref string returnMessage)
        {
            bool bRetVal = false;

            if (player == null)
            {
                if (gameRoom != null)
                {
                    // Register (if not already; ignore 'false' return value if already registered)
                    gameRoom.RegisterPlayer(playerName, playerPassword);
                    
                    // Enter the game room
                    player = gameRoom.Enter(playerName, playerPassword);

                    if (player != null)
                    {
                        player.AddListener(this);

                        bRetVal = true;
                    }
                    else
                    {
                        if (returnMessage != null)
                        {
                            returnMessage += "\nPlayer couldn't login with name '" + playerName + "' and password '" + playerPassword + "'.";
                        }
                    }
                }
                else
                {
                    if (returnMessage != null)
                    {
                        returnMessage += "\nNo game room available to connect to!";
                    }
                }
            }
            else
            {
                if (returnMessage != null)
                {
                    returnMessage += "\nPlayer with name '" + playerName + "' is already connected!";
                }

                bRetVal = false;
            }

            return bRetVal;
        }

        public bool OnDisconnect(ref string returnMessage)
        {
            bool bRetVal = false;

            if (player != null)
            {
                if (gameRoom != null)
                {
                    gameRoom.Leave(player);

                    gameRoom.RemoveListener(this);
                    player.RemoveListener(this);

                    player = null;

                    bRetVal = true;
                }
                else
                {
                    if (returnMessage != null)
                    {
                        returnMessage += "\nNo game room available to disconnect from!";
                    }
                }
            }
            else
            {
                if (returnMessage != null)
                {
                    returnMessage += "\nPlayer is not connected!";
                }

                bRetVal = false;
            }

            return bRetVal;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets or gets the game room that this player is connected to
        /// </summary>
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

        /// <summary>
        /// Checks whether or not the player is actually connected to a game room
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return player != null;
            }
        }

        public Queue<KeyValuePair<INetworkBackgammonNotifier, INetworkBackgammonEvent>> EventQueue
        {
            get
            {
                return eventQueue;
            }
        }

        #endregion

        #region INetworkBackgammonListener Members

        public bool AddNotifier(INetworkBackgammonNotifier notifier)
        {
            if (notifier != null && defaultListener != null)
            {
                return defaultListener.AddNotifier(notifier);
            }
            else
            {
                return false;
            }
        }

        public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        {
            if (notifier != null && defaultListener != null)
            {
                return defaultListener.RemoveNotifier(notifier);
            }
            else
            {
                return false;
            }
        }

        public void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e)
        {
            eventQueue.Enqueue(new KeyValuePair<INetworkBackgammonNotifier, INetworkBackgammonEvent>(sender, e)); 
        }

        #endregion
    }
}
