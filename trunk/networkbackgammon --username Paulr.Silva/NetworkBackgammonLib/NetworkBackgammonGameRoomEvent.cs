using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkBackgammonLib;

namespace NetworkBackgammonLib
{
    public class NetworkBackgammonGameRoomEvent : INetworkBackgammonEvent
    {
        public enum GameRoomEventType
        {
            PlayerConnected,
            PlayerDisconnected
        };

        GameRoomEventType type;

        public NetworkBackgammonGameRoomEvent(GameRoomEventType _type)
        {
            type = _type;
        }

        public GameRoomEventType EventType
        {
            get
            {
                return type;
            }
        }
    }
}
