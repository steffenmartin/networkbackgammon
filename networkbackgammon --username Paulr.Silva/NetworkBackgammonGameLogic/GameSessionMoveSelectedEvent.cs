using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkBackgammonLib;

namespace NetworkBackgammonGameLogic
{
    public class GameSessionMoveSelectedEvent : NetworkBackgammonGameSessionEvent
    {
        private Checker checkerSelected;
        private Dice moveSelected;

        public GameSessionMoveSelectedEvent(Checker _checkerSelected, Dice _moveSelected) 
            : base(GameSessionEventType.MoveSelected)
        {
            checkerSelected = _checkerSelected;
            moveSelected = _moveSelected;
        }

        public Checker CheckerSelected
        {
            get
            {
                return checkerSelected;
            }
        }
        public Dice MoveSelected
        {
            get
            {
                return moveSelected;
            }
        }
    }
}
