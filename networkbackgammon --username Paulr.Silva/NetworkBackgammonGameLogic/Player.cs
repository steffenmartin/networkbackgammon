using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class Player : GameSessionSubject, IGameSessionListener
    {
        private string strPlayerName = "";
        private Checker[] checkers = null;
        private bool bActive = false;

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

        public bool Active
        {
            get
            {
                return bActive;
            }
            set
            {
                bActive = value;
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

        #region IGameSessionListener Members

        public void Notify(GameSessionEvent _event, GameSessionSubject _subject)
        {
            // throw new NotImplementedException();
        }

        #endregion
    }
}
