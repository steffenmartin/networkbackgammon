using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    class GameSession
    {
        public GameSession(Player _player1, Player _player2)
        {
            player1 = _player1;
            player2 = _player2;
        }

        private Player player1 = null;
        private Player player2 = null;

        public Player Player1
        {
            get
            {
                return player1;
            }
        }

        public Player Player2
        {
            get
            {
                return player2;
            }
        }
    }
}
