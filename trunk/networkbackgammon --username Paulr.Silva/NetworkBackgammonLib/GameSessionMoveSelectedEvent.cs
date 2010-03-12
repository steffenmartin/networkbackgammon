using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    public class GameSessionMoveSelectedEvent : NetworkBackgammonGameSessionEvent
    {
        private NetworkBackgammonChecker checkerSelected;
        private NetworkBackgammonDice moveSelected;

        public GameSessionMoveSelectedEvent(NetworkBackgammonChecker _checkerSelected, NetworkBackgammonDice _moveSelected) 
            : base(GameSessionEventType.MoveSelected)
        {
            checkerSelected = _checkerSelected;
            moveSelected = _moveSelected;
        }

        public NetworkBackgammonChecker CheckerSelected
        {
            get
            {
                return checkerSelected;
            }
        }
        public NetworkBackgammonDice MoveSelected
        {
            get
            {
                return moveSelected;
            }
        }
    }
}
