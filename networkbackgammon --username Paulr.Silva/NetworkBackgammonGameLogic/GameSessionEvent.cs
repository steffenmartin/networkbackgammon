using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class GameSessionEvent
    {
        public enum GameSessionEventType
        {
            PlayerResigned,
            GameFinished,
            DiceRolled
        };

        GameSessionEventType type;

        public GameSessionEvent(GameSessionEventType _type)
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
