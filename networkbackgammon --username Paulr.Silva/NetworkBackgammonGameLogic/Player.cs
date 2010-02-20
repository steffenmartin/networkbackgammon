using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class Player
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
    }
}
