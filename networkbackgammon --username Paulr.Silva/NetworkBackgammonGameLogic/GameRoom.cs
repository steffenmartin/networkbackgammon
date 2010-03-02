﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class GameRoom : GameRoomSubject
    {
        private List<Player> connectedPlayers = new List<Player>();
        private List<GameSession> gameSessions = new List<GameSession>();

        ~GameRoom()
        {
            gameSessions.Clear();
            connectedPlayers.Clear();
        }

        public Player Login(string _playerName)
        {
            Player newPlayer = new Player(_playerName);

            connectedPlayers.Add(newPlayer);

            BroadCast(new GameRoomEvent(GameRoomEvent.GameRoomEventType.PlayerConnected));

            // Automatically create a game session once 2 players have logged in
            // First player logged in will be the challenged one, the second one will
            // be the challenger
            if (connectedPlayers.Count == 2)
            {
                StartGame(connectedPlayers[0], connectedPlayers[1]);
            }

            return newPlayer;
        }

        public void Logout(Player _player)
        {
            if (connectedPlayers.Contains(_player))
            {
                connectedPlayers.Remove(_player);
            }

            BroadCast(new GameRoomEvent(GameRoomEvent.GameRoomEventType.PlayerDisconnected));
        }

        public void StartGame(Player _challengingPlayer, Player _challengedPlayer)
        {
            // TODO: Check whether challenged player accepts or rejects challenge

            GameSession newSession = new GameSession(_challengingPlayer, _challengedPlayer);

            if (newSession != null)
            {
                gameSessions.Add(newSession);

                newSession.Start();
            }
        }

        public void Shutdown()
        {
            foreach (GameSession session in gameSessions)
            {
                session.Stop();
            }

            gameSessions.Clear();
            connectedPlayers.Clear();
        }

        public List<Player> ConnectedPlayers
        {
            get
            {
                return connectedPlayers;
            }
        }
    }
}