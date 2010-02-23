using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class Player : GameSessionSubject
    {
        private string strPlayerName = "";
        private Checker[] checkers = null;

        public Player(string _strPlayerName)
        {
            strPlayerName = _strPlayerName;
        }

        public string PlayerName
        {
            get
            {
                return strPlayerName;
            }
        }

        public string RollDice()
        {
            uint dice1 = 3;
            uint dice2 = 2;

            Broadcast(new GameSessionEvent(GameSessionEvent.GameSessionEventType.DiceRolled), this);

            return dice1.ToString() + ", " + dice2.ToString();
        }

        public override string ToString()
        {
            return strPlayerName;
        }
    }
}
