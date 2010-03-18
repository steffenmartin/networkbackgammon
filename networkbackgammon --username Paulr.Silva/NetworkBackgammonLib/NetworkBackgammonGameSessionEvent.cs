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
            PlayerResigned,
            GameFinished,
            InitialDiceRolled,
            InitialDiceRolledAcknowledged,
            CheckerUpdated,
            MoveSelected,
            Error,
            Invalid
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
