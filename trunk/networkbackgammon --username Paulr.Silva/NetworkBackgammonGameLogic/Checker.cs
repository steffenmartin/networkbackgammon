using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class Checker
    {
        private Position currentPosition;

        private List<Dice.DiceValue> possibleMoves = new List<Dice.DiceValue>();

        public Checker(Position _position)
        {
            currentPosition = _position;
        }

        public List<Dice.DiceValue> PossibleMoves
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
