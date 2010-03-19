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

            /// <summary>
            /// Gets the semaphore for synchronizing this challenge
            /// </summary>
            public Semaphore ChallengeSemaphore
            {
                get
                {
                    return challengeSemaphore;
                }
            }

            /// <summary>
            /// Gets the result (response) to a game challenge
            /// </summary>
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

        /// <summary>
        /// Instance of the default notifier implementation
        /// </summary>
        /// <remarks>
        /// Created and set during construction time. Used for delegating function
        /// calls on this class' notifier interface implementation.
        /// </remarks>
        INetworkBackgammonNotifier defaultNotifier = null;

        /// <summary>
        /// Instance of the default listener implementation
        /// </summary>
        /// <remarks>
        /// Used for delegating function calls on this class' listener interface implementation.
        /// </remarks>
        INetworkBackgammonListener defaultListener = new NetworkBackgammonListener();

        /// <summary>
        /// List of backgammon players connected to this game room
        /// </summary>
        List<NetworkBackgammonPlayer> connectedPlayers = new List<NetworkBackgammonPlayer>();

        /// <summary>
        /// List of currently active game sessions in this game room
        /// </summary>
        List<NetworkBackgammonGameSession> gameSessions = new List<NetworkBackgammonGameSession>();

        /// <summary>
        /// Hashtable with clients username and passwords
        /// </summary>
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
        /// Timeout for challenge request (ms)
        /// </summary>
        int challengeRequestTimeoutMs = 10000;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkBackgammonRemoteGameRoom()
        {
            defaultNotifier = new NetworkBackgammonNotifier(this);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~NetworkBackgammonRemoteGameRoom()
        {
            Shutdown();
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

                // Have the player start listening to this game room
                AddListener(newPlayer);

                // Broadcast the player connected event to all registered listeners
                Broadcast(new NetworkBackgammonGameRoomEvent(NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerConnected));
            }
            else
            {
                newPlayer = null;
            }

            return newPlayer;
        }

        /// <summary>
        /// Exit the game room by player
        /// </summary>
        /// <param name="_player">Backgammon player who wants to leave the game room</param>
        public void Leave(NetworkBackgammonPlayer _player)
        {
            if (connectedPlayers.Contains(_player))
            {
                // The disconnected player should no longer be listening...
                RemoveListener(_player);
                // Stop listening to this player
                _player.RemoveListener(this);

                // Check if player is associated with a game room and remove if necessary
                NetworkBackgammonGameSession _playerGameSession = GetGameSession(_player);

                if (_playerGameSession != null)
                {
                    // Disconnect the player from the game session
                    _player.RemoveListener(_playerGameSession);
                    // Disconnect the game session from the player
                    _playerGameSession.RemoveListener(_player);

                    // Get the opposing player
                    NetworkBackgammonPlayer opposingPlayer = _playerGameSession.GetOpponent(_player);

                    if (opposingPlayer != null)
                    {
                        // Disconnect the player from the game session
                        opposingPlayer.RemoveListener(_playerGameSession);
                        // Disconnect the game session from the player
                        _playerGameSession.RemoveListener(opposingPlayer);
                    }

                    // Halt the game sesssion
                    _playerGameSession.Stop();
                }

                // Remove player from connected player list
                connectedPlayers.Remove(_player);

                // Broadcast the player disconnected event to all registered listeners
                Broadcast(new NetworkBackgammonGameRoomEvent(NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerDisconnected));
            }
        }

        /// <summary>
        /// Challenge an opponent to a game
        /// </summary>
        /// <param name="_challengingPlayer">Challenging backgammon player</param>
        /// <param name="_challengedPlayer">Challenged backgammon player</param>
        /// <returns></returns>
        //public bool Challenge(NetworkBackgammonPlayer _challengingPlayer, NetworkBackgammonPlayer _challengedPlayer)
        public bool Challenge(string _challengingPlayerName, string _challengedPlayerName)
        {
            bool retval = false;

            NetworkBackgammonPlayer _challengingPlayer = GetPlayerByName(_challengingPlayerName);
            NetworkBackgammonPlayer _challengedPlayer = GetPlayerByName(_challengedPlayerName);

            // Check if the players are in the available player game room list
            //if (connectedPlayers.Contains(_challengingPlayer) && connectedPlayers.Contains(_challengedPlayer))
            if (_challengingPlayer != null && _challengedPlayer != null)
            {
                // Check if players are currently free to participate in a game session
                if (!IsPlayerInGameSession(_challengingPlayer) && !IsPlayerInGameSession(_challengedPlayer))
                {
                    Semaphore challengeSemaphore = new Semaphore(0, 1);

                    // Add game challenge data container instance to be used for the (asynchronous) challenge procedure
                    challengeSyncList.Add(_challengedPlayer, new NetworkBackgammonChallengeDataContainer(challengeSemaphore));

                    // Broadcast the player challenge event
                    Broadcast(new NetworkBackgammonChallengeEvent(_challengingPlayerName, _challengedPlayerName));

                    // Wait for response from challenged player (or timeout)
                    if (challengeSemaphore.WaitOne(challengeRequestTimeoutMs))
                    {
                        retval = challengeSyncList[_challengedPlayer].ChallengeAccepted;

                        // Create and start game session if challenge has been accepted by challenged player
                        if (retval)
                        {
                            NetworkBackgammonGameSession gameSession = new NetworkBackgammonGameSession(_challengingPlayer, _challengedPlayer);

                            // Game Session is listening for events from Player 1
                            _challengingPlayer.AddListener(gameSession);
                            // Player 1 is listening for events from Game Session
                            gameSession.AddListener(_challengingPlayer);
                            // Game Session is listening for events form Player 2
                            _challengedPlayer.AddListener(gameSession);
                            // Player 2 is listening for events from Game Session
                            gameSession.AddListener(_challengedPlayer);
                            // Append to list of game sessions...
                            gameSessions.Add(gameSession);
                            // Start the game...
                            gameSession.Start();
                        }

                        // Give the challenging player the challenge response 
                        Broadcast(new NetworkBackgammonChallengeResponseEvent(retval, _challengedPlayerName, _challengingPlayerName));
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

        /// <summary>
        /// Start the game session with two consenting players
        /// </summary>
        /// <param name="_challengingPlayer">Challenging backgammon player</param>
        /// <param name="_challengedPlayer">Challenged backgammon player</param>
        /// <returns></returns>
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

        /// <summary>
        /// Shutdown the game room - stop all sessions
        /// </summary>
        public void Shutdown()
        {
            foreach (NetworkBackgammonGameSession session in gameSessions)
            {
                session.Stop();
            }

            gameSessions.Clear();
            connectedPlayers.Clear();
        }

        /// <summary>
        /// Determine whether or not player is participating in a game session
        /// </summary>
        /// <param name="player">Backgammon player to be checked</param>
        /// <returns>"True" if backgammon player in question is in a game session, otherwise "false"</returns>
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

        /// <summary>
        /// Get the game session the the "player" is currently apart of
        /// </summary>
        /// <param name="player">Backgammon player to be checked</param>
        /// <returns>null if player is not part of a game session</returns>
        public NetworkBackgammonGameSession GetGameSession(NetworkBackgammonPlayer player)
        {
            NetworkBackgammonGameSession retval = null;

            // Look through the list of game sessions and make sure the player is not already playing in another room
            foreach (NetworkBackgammonGameSession session in gameSessions)
            {
                if (session.ContainsPlayer(player))
                {
                    retval = session;
                    break;
                }
            }

            return retval;
        }

        /// <summary>
        /// Get the player's opponent
        /// </summary>
        /// <param name="player">Backgammon player to be checked</param>
        /// <returns>null if player is not part of a game session</returns>
        public NetworkBackgammonPlayer GetOpposingPlayer(NetworkBackgammonPlayer player)
        {
            NetworkBackgammonPlayer retval = null;

            // Get the game session associated with this player
            NetworkBackgammonGameSession gameSession = GetGameSession(player);

            if (gameSession != null)
            {
                retval = gameSession.GetOpponent(player);
            }

            return retval;
        }

        /// <summary>
        /// Determine whether or not player is participating in a game session
        /// </summary>
        /// <param name="player">Backgammon player to be checked</param>
        /// <returns>null NetworkBackgammonPlayer object if name not found</returns>
        public NetworkBackgammonPlayer GetPlayerByName(string playerName)
        {
            NetworkBackgammonPlayer retval = null;

            // Look through the list of game sessions and make sure the player is not already playing in another room
            foreach (NetworkBackgammonPlayer player in connectedPlayers)
            {
                if (player.PlayerName.CompareTo(playerName) == 0)
                {
                    retval = player;
                    break;
                }
            }

            return retval;
        }

        
        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of connected players
        /// </summary>
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
                else if (e is NetworkBackgammonChatEvent)
                {
                    // Pass the message through to the listeners
                    Broadcast((NetworkBackgammonChatEvent)e);
                }
            }
        }

        #endregion
    }
}
