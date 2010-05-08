using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    [Serializable]
    public class NetworkBackgammonGameSessionEvent : INetworkBackgammonEvent
    {
        public enum GameSessionEventType
        {
            GameFinished,
            Error,
            Terminated,
            Invalid
        };

        protected GameSessionEventType type;

        public NetworkBackgammonGameSessionEvent()
        {
            type = GameSessionEventType.Invalid;
        }

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
