using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    [Serializable]
    public class NetworkBackgammonChecker
    {
        NetworkBackgammonPosition currentPosition;
        List<NetworkBackgammonDice> possibleMoves = new List<NetworkBackgammonDice>();

        public NetworkBackgammonChecker(NetworkBackgammonPosition _position)
        {
            currentPosition = _position;
        }

        public void MoveChecker(NetworkBackgammonDice _move)
        {
            currentPosition += _move;
        }

        public List<NetworkBackgammonDice> PossibleMoves
        {
            get
            {
                return possibleMoves;
            }
        }
        public NetworkBackgammonPosition CurrentPosition
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
