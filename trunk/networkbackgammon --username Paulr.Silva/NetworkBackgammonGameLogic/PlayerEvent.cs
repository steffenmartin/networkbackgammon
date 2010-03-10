using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class PlayerEvent : INetworkBackgammonEvent
    {
        private Checker selectedChecker;
        private Dice selectedMove;

        public PlayerEvent(Checker _checker, Dice _move)
        {
            selectedChecker = _checker;
            selectedMove = _move;
        }

        public Checker SelectedChecker
        {
            get
            {
                return selectedChecker;
            }
        }

        public Dice SelectedMove
        {
            get
            {
                return selectedMove;
            }
        }

        public Checker GetSelectedChecker()
        {
            return SelectedChecker;
        }

        public Dice GetSelectedMove()
        {
            return selectedMove;
        }
    }
}
