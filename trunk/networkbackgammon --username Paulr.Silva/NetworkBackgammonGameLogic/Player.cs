using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class Player : GameSessionSubject, IGameSessionListener
    {
        private string strPlayerName = "";
        private List<Checker> checkers = new List<Checker>();
        private bool bActive = false;

        public Player(string _strPlayerName)
        {
            strPlayerName = _strPlayerName;

            InitCheckers();
        }

        /// <summary>
        /// Initialize list of checkers in their initial positions.
        /// </summary>
        private void InitCheckers()
        {
            // 2 checkers on 1
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.ONE)));
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.ONE)));

            // 5 checkers on 12
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.TWELVE)));
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.TWELVE)));
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.TWELVE)));
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.TWELVE)));
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.TWELVE)));

            // 3 checkers on 17
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.SEVENTEEN)));
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.SEVENTEEN)));
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.SEVENTEEN)));

            // 5 checkers on 19
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.NINETEEN)));
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.NINETEEN)));
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.NINETEEN)));
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.NINETEEN)));
            checkers.Add(new Checker(new Position(Position.GameBoardPosition.NINETEEN)));
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
        public List<Checker> Checkers
        {
            get
            {
                return checkers;
            }
        }

        public override string ToString()
        {
            return strPlayerName;
        }

        #region IGameSessionListener Members

        public void Notify(GameSessionEvent _event, GameSessionSubject _subject)
        {
            // Forward event
            Broadcast(_event, _subject);
        }

        #endregion
    }
}
