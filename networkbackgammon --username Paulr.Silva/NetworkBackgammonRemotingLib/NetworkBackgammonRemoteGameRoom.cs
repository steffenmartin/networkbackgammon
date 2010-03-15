using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkBackgammonLib;
using NetworkBackgammonGameLogic;
using System.Threading;

namespace NetworkBackgammonRemotingLib
{
    public class NetworkBackgammonRemoteGameRoom : MarshalByRefObject, 
                                                   INetworkBackgammonNotifier,
                                                   INetworkBackgammonListener       /* Needs to be a listener for events like
                                                                                     * two players challenging each other */
    {
        #region Declarations

        /// <summary>
        /// Container with data required for game challenges
        /// </summary>
        private class NetworkBackgammonChallengeDataContainer
        {

            #region Members

            /// <summary>
            /// Semaphore for asynchronous game challenge
            /// </summary>
            private Semaphore challengeSemaphore = null;

            /// <summary>
            /// Result (response) to a game challenge
            /// </summary>
            private bool challengeAccepted = false;

            #endregion

            #region Methods

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_semaphore"></param>
            public NetworkBackgammonChallengeDataContainer(Semaphore _semaphore)
            {
                challengeSemaphore = _semaphore;
            }

            #endregion

            #region Properties

            public Semaphore ChallengeSemaphore
            {
                get
                {
                    return challengeSemaphore;
                }
            }

            public bool ChallengeAccepted
            {
                get
                {
                    return challengeAccepted;
                }
                set
                {
                    challengeAccepted = value;
                }
            }

            #endregion
        }

        #endregion

        #region Members

        INetworkBackgammonNotifier defaultNotifier = null;
        /// <summary>
        /// Default listener
        /// </summary>
        INetworkBackgammonListener defaultListener = new NetworkBackgammonListener();
        List<NetworkBackgammonPlayer> connectedPlayers = new List<NetworkBackgammonPlayer>();
        List<NetworkBackgammonGameSession> gameSessions = new List<NetworkBackgammonGameSession>();
        Dictionary<NetworkBackgammonPlayer, NetworkBackgammonPlayer> challengeList = new Dictionary<NetworkBackgammonPlayer, NetworkBackgammonPlayer>();

        // Hashtable with clients username and passwords
        Dictionary<string, string> clientUsernameList = new Dictionary<string, string>();
        /// <summary>
        /// Hashtable with data required for synchronizing game challenged (requests/responses)
        /// </summary>
        /// <remarks>
        /// Hashtable with (challenged) player associated semaphores and challenge results (response)
        /// to coordinate challenge request and response in an asynchronous fashion
        /// </remarks>
        Dictionary<NetworkBackgammonPlayer, NetworkBackgammonChallengeDataContainer> challengeSyncList = 
            new Dictionary<NetworkBackgammonPlayer, NetworkBackgammonChallengeDataContainer>();
        /// <summary>
        /// Timeout for challenge request
        /// </summary>
        int challengeRequestTimeoutMs = 5000;

        #endregion

        #region Methods

        public NetworkBackgammonRemoteGameRoom()
        {
            defaultNotifier = new NetworkBackgammonNotifier(this);
        }

        ~NetworkBackgammonRemoteGameRoom()
        {
            gameSessions.Clear();
            connectedPlayers.Clear();
        }

        /**
         * Register as a user into the system.
         * @param username Unique name that the user has provided
         * @param pw Password for the the new user account
         * @return bool True if account was added
         */
        public bool RegisterPlayer(string username, string pw)
        {
            bool retval = !clientUsernameList.ContainsKey(username);

            if (retval)
            {
                clientUsernameList.Add(username, pw);
            }

            return retval;
        }

        /**
         * Join a game room by player's name (username)
         * @param string Username
         * @return a valid (non-null) player object
         */
        public NetworkBackgammonPlayer Enter(string _playerName, string pw)
        {
            NetworkBackgammonPlayer newPlayer = new NetworkBackgammonPlayer(_playerName);

            // Check the user is alread in the list
            if (!connectedPlayers.Contains(newPlayer) && 
                clientUsernameList.ContainsKey(_playerName) &&
                string.Compare(clientUsernameList[_playerName], pw, false) == 0)
            {
                // Add new player object to the game room list
                connectedPlayers.Add(newPlayer);

                // Start listening to this player
                newPlayer.AddListener(this);

                // Broadcast the player connected event to all registered listeners
                Broadcast(new NetworkBackgammonGameRoomEvent(NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerConnected));
            }
            else
            {
                newPlayer = null;
            }

            return newPlayer;
        }

        // Exit the game room by player
        public void Leave(NetworkBackgammonPlayer _player)
        {
            if (connectedPlayers.Contains(_player))
            {
                // Stop listening to this player
                _player.RemoveListener(this);

                connectedPlayers.Remove(_player);

                // Broadcast the player disconnected event to all registered listeners
                ((INetworkBackgammonNotifier)this).Broadcast(new NetworkBackgammonGameRoomEvent(NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerDisconnected));
            }
        }

        // Challenge an opponent to a game
        public bool Challenge(NetworkBackgammonPlayer _challengingPlayer, NetworkBackgammonPlayer _challengedPlayer)
        {
            bool retval = false;

            // Check if the players are in the available player game room list
            if (connectedPlayers.Contains(_challengingPlayer) && connectedPlayers.Contains(_challengedPlayer))
            {
                // Check if players are currently free to participate in a game session
                if (!IsPlayerInGameSession(_challengingPlayer) && !IsPlayerInGameSession(_challengedPlayer))
                {
                    Semaphore challengeSemaphore = new Semaphore(0, 1);

                    // Add game challenge data container instance to be used for the (asynchronous) challenge procedure
                    challengeSyncList.Add(_challengedPlayer, new NetworkBackgammonChallengeDataContainer(challengeSemaphore));

                    // Broadcast a challenge event to the "challenged player"
                    ((INetworkBackgammonListener)_challengedPlayer).OnEventNotification(this, new NetworkBackgammonChallengeEvent(_challengingPlayer));

                    // Wait for response from challenged player (or timeout)
                    if (challengeSemaphore.WaitOne(challengeRequestTimeoutMs))
                    {
                        retval = challengeSyncList[_challengedPlayer].ChallengeAccepted;

                        // Create and start game session if challenge has been accepted by challenged player
                        if (retval)
                        {
                            NetworkBackgammonGameSession gameSession = new NetworkBackgammonGameSession(_challengingPlayer, _challengedPlayer);

                            gameSessions.Add(gameSession);

                            gameSession.Start();
                        }
                    }
                    else
                    {
                        retval = false;
                    }

                    challengeSyncList.Remove(_challengedPlayer);
                }
            }

            return retval;
        }

        // Start the game session with two consenting players
        public bool StartGame(NetworkBackgammonPlayer _challengingPlayer, NetworkBackgammonPlayer _challengedPlayer)
        {
            bool retval = false;

            // Check if the players are in the available playe game room list
            if (connectedPlayers.Contains(_challengingPlayer) && connectedPlayers.Contains(_challengedPlayer))
            {
                // Check if players are currently free to participate in a game session
                if (!IsPlayerInGameSession(_challengingPlayer) && !IsPlayerInGameSession(_challengedPlayer))
                {
                    // Create new game session with the consenting players
                    NetworkBackgammonGameSession newSession = new NetworkBackgammonGameSession(_challengingPlayer, _challengedPlayer);

                    // Add the game session to the game session list
                    gameSessions.Add(newSession);

                    // Startup the game...
                    newSession.Start();

                    retval = true;
                }
            }

            return retval;
        }

        // Shutdown the game room - stop all sessions
        public void Shutdown()
        {
            foreach (NetworkBackgammonGameSession session in gameSessions)
            {
                session.Stop();
            }

            gameSessions.Clear();
            connectedPlayers.Clear();
        }

        // Determine whether or not player is participating in a game session
        public bool IsPlayerInGameSession(NetworkBackgammonPlayer player)
        {
            bool retval = false;

            // Look through the list of game sessions and make sure the player is not already playing in another room
            foreach (NetworkBackgammonGameSession session in gameSessions)
            {
                if (session.ContainsPlayer(player))
                {
                    retval = false;
                    break;
                }
            }

            return retval;
        }

        #endregion

        #region Properties

        // List of connected players
        public List<NetworkBackgammonPlayer> ConnectedPlayers
        {
            get
            {
                return connectedPlayers;
            }
        }

        #endregion

        #region INetworkBackgammonNotifier Members

        public bool AddListener(INetworkBackgammonListener listener)
        {
            return defaultNotifier != null ? defaultNotifier.AddListener(listener) : false;
        }

        public bool RemoveListener(INetworkBackgammonListener listener)
        {
            return defaultNotifier != null ? defaultNotifier.RemoveListener(listener) : false;
        }

        public void Broadcast(INetworkBackgammonEvent notificationEvent)
        {
            defaultNotifier.Broadcast(notificationEvent);
        }

        public void Broadcast(INetworkBackgammonEvent notificationEvent, INetworkBackgammonNotifier notifier)
        {
            defaultNotifier.Broadcast(notificationEvent, notifier);
        }

        #endregion

        #region INetworkBackgammonListener Members

        public bool AddNotifier(INetworkBackgammonNotifier notifier)
        {
            return defaultListener != null ? defaultListener.AddNotifier(notifier) : false;
        }

        public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        {
            return defaultListener != null ? defaultListener.RemoveNotifier(notifier) : false;
        }

        public void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e)
        {
            if (sender is NetworkBackgammonPlayer)
            {
                NetworkBackgammonPlayer player = (NetworkBackgammonPlayer) sender;

                if (e is NetworkBackgammonChallengeResponseEvent)
                {
                    NetworkBackgammonChallengeResponseEvent challengeResponseEvent = (NetworkBackgammonChallengeResponseEvent)e;
                    if (challengeSyncList.Keys.Contains(player))
                    {
                        if (challengeSyncList[player] != null)
                        {
                            challengeSyncList[player].ChallengeAccepted = challengeResponseEvent.ChallengeAccepted;
                            challengeSyncList[player].ChallengeSemaphore.Release();
                        }
                    }
                }
            }
        }

        #endregion
    }
}
