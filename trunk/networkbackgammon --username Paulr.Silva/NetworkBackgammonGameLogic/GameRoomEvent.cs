using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class GameRoomEvent
    {
        public enum GameRoomEventType
        {
            PlayerConnected,
            PlayerDisconnected
        };

        GameRoomEventType type;

        public GameRoomEvent(GameRoomEventType _type)
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
