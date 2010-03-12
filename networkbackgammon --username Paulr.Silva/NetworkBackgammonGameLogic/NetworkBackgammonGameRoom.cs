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

        private List<NetworkBackgammonPlayer> connectedPlayers = new List<NetworkBackgammonPlayer>();
        private List<NetworkBackgammonGameSession> gameSessions = new List<NetworkBackgammonGameSession>();

        public NetworkBackgammonGameRoom()
        {
            defaultNotifier = new NetworkBackgammonNotifier(this);
        }
        ~NetworkBackgammonGameRoom()
        {
            gameSessions.Clear();
            connectedPlayers.Clear();
        }

        public NetworkBackgammonPlayer Login(string _playerName)
        {
            NetworkBackgammonPlayer newPlayer = new NetworkBackgammonPlayer(_playerName);

            connectedPlayers.Add(newPlayer);

            Broadcast(new NetworkBackgammonGameRoomEvent(NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerConnected));

            // Automatically create a game session once 2 players have logged in
            // First player logged in will be the challenged one, the second one will
            // be the challenger
            if (connectedPlayers.Count == 2)
            {
                StartGame(connectedPlayers[0], connectedPlayers[1]);
            }

            return newPlayer;
        }

        public void Logout(NetworkBackgammonPlayer _player)
        {
            if (connectedPlayers.Contains(_player))
            {
                connectedPlayers.Remove(_player);
            }

            ((INetworkBackgammonNotifier)this).Broadcast(new NetworkBackgammonGameRoomEvent(NetworkBackgammonGameRoomEvent.GameRoomEventType.PlayerDisconnected));
        }

        public void StartGame(NetworkBackgammonPlayer _challengingPlayer, NetworkBackgammonPlayer _challengedPlayer)
        {
            // TODO: Check whether challenged player accepts or rejects challenge

            NetworkBackgammonGameSession newSession = new NetworkBackgammonGameSession(_challengingPlayer, _challengedPlayer);

            if (newSession != null)
            {
                gameSessions.Add(newSession);

                newSession.Start();
            }
        }

        public void Shutdown()
        {
            foreach (NetworkBackgammonGameSession session in gameSessions)
            {
                session.Stop();
            }

            gameSessions.Clear();
            connectedPlayers.Clear();
        }

        public List<NetworkBackgammonPlayer> ConnectedPlayers
        {
            get
            {
                return connectedPlayers;
            }
        }

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
