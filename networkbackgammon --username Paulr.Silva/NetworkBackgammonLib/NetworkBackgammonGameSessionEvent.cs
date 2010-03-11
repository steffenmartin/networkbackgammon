using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    public class NetworkBackgammonGameSessionEvent : INetworkBackgammonEvent
    {
        public enum GameSessionEventType
        {
            PlayerResigned,
            GameFinished,
            DiceRolled,
            CheckerUpdated,
            MoveSelected,
            Error
        };

        protected GameSessionEventType type;

        public NetworkBackgammonGameSessionEvent(GameSessionEventType _type)
        {
            type = _type;
        }

        public GameSessionEventType EventType
        {
            get
            {
                return type;
            }
        }
    }
}
