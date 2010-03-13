using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkBackgammonLib;

namespace NetworkBackgammonGameLogic
{
    public class NetworkBackgammonGameRoom : INetworkBackgammonNotifier
    {
        INetworkBackgammonNotifier defaultNotifier = null;
        List<NetworkBackgammonPlayer> connectedPlayers = new List<NetworkBackgammonPlayer>();
        List<NetworkBackgammonGameSession> gameSessions = new List<NetworkBackgammonGameSession>();

        public NetworkBackgammonGameRoom()
        {
            defaultNotifier = new NetworkBackgammonNotifier(this);
        }

        ~NetworkBackgammonGameRoom()
        {
            gameSessions.Clear();
            connectedPlayers.Clear();
        }

        /**
         * Join a game room by player's name (username)
         * @param string Username
         * @return a valid (non-null) player object
         */
        public NetworkBackgammonPlayer Enter(string _playerName)
        {
            NetworkBackgammonPlayer newPlayer = new NetworkBackgammonPlayer(_playerName);

            // Check the user is alread in the list
            if (!connectedPlayers.Contains(newPlayer))
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

        // Exit the game room by player's name (username)
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
                if (!IsPlayerInGameRoom(_challengingPlayer) && !IsPlayerInGameRoom(_challengedPlayer))
                {
                    // Broadcast a challenge event to the "challeged player"
                    //NetworkBackgammonChaallengeEvent
                    retval = true;
                }
            }

            return retval;
        }

        // Revoke a challenge (the player no longer wants to challenge the player)
        public bool RevokeChallenge(NetworkBackgammonPlayer _challengingPlayer)
        {
            bool retval = true;

            return retval;
        }

        // Accept a challenge 
        public bool AcceptChallenge(NetworkBackgammonPlayer _challengedPlayer)
        {
            bool retval = true;

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
                if (!IsPlayerInGameRoom(_challengingPlayer) && !IsPlayerInGameRoom(_challengedPlayer))
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
        public bool IsPlayerInGameRoom(NetworkBackgammonPlayer player)
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
