using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkBackgammonLib;
using NetworkBackgammonGameLogic;

namespace NetworkBackgammonRemotingLib
{
    public class NetworkBackgammonRemoteGameRoom : MarshalByRefObject, 
                                                   INetworkBackgammonNotifier
    {
        INetworkBackgammonNotifier defaultNotifier = null;
        List<NetworkBackgammonPlayer> connectedPlayers = new List<NetworkBackgammonPlayer>();
        List<NetworkBackgammonGameSession> gameSessions = new List<NetworkBackgammonGameSession>();
        Dictionary<NetworkBackgammonPlayer, NetworkBackgammonPlayer> challengeList = new Dictionary<NetworkBackgammonPlayer, NetworkBackgammonPlayer>();

        // Hashtable with clients username and passwords
        Dictionary<string, string> clientUsernameList = new Dictionary<string, string>();

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
                connectedPlayers.Remove(_player);

                // Broadcast the player disconnected event to all registered listeners
                ((INetworkBackgammonNotifier)this).Broadcast(new NetworkBackgammonGameRoomEvent(NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerDisconnected));
            }
        }

        // Challenge an opponent to a game
        public bool Challenge(NetworkBackgammonPlayer _challengingPlayer, NetworkBackgammonPlayer _challengedPlayer)
        {
            bool retval = false;

            // Check if the players are in the available playe game room list
            if (connectedPlayers.Contains(_challengingPlayer) && connectedPlayers.Contains(_challengedPlayer))
            {
                // Check if players are currently free to participate in a game session
                if (!IsPlayerInGameSession(_challengingPlayer) && !IsPlayerInGameSession(_challengedPlayer))
                {
                    // Add players to challenge list
                    challengeList.Add(_challengedPlayer, _challengingPlayer);
                    // Broadcast a challenge event to the "challeged player"
                    Broadcast(new NetworkBackgammonChallengeEvent(_challengingPlayer, _challengedPlayer));
          
                    retval = true;
                }
            }

            return retval;
        }

        // Revoke a challenge (the player no longer wants to challenge the player)
        public bool RevokeChallenge(NetworkBackgammonPlayer _challengingPlayer)
        {
            bool retval = true;

            // Search for challenging player(key) in the challenge list 

            return retval;
        }

        // Accept a challenge 
        public bool AcceptChallenge(NetworkBackgammonPlayer _challengedPlayer)
        {
            bool retval = false;

            if( challengeList.ContainsKey(_challengedPlayer) )
            {
                // and broadcast challenge accepted event
                Broadcast(new NetworkBackgammonAcceptChallengeEvent(challengeList[_challengedPlayer], _challengedPlayer));
                // remove challenger from the list...
                challengeList.Remove(_challengedPlayer);
            }

            return retval;
        }

        // Deny a challenge 
        public bool DenyChallenge(NetworkBackgammonPlayer _challengedPlayer)
        {
            bool retval = true;

            // Search for challenged player in the challenge list 

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
    }
}
