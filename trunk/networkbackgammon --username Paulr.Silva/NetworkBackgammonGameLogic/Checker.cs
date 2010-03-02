using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class Checker
    {
        private Position currentPosition;

        private List<Dice> possibleMoves = new List<Dice>();

        public Checker(Position _position)
        {
            currentPosition = _position;
        }

        public void MoveChecker(Dice _move)
        {
            currentPosition += _move;
        }

        public List<Dice> PossibleMoves
        {
            get
            {
                return possibleMoves;
            }
        }
        public Position CurrentPosition
        {
            get
            {
                return currentPosition;
            }
        }

        public override string ToString()
        {
            return currentPosition.ToString();
        }
    }
}
