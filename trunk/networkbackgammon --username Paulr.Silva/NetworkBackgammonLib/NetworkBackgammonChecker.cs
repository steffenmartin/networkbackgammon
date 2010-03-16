using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Backgammon checker, also known as men, pieces, stones, or counters
    /// </summary>
    [Serializable]
    public class NetworkBackgammonChecker
    {
        #region Members

        /// <summary>
        /// The (current) position of this checker
        /// </summary>
        NetworkBackgammonPosition currentPosition;

        /// <summary>
        /// List of possible moves this checker is allowed to perform, e.g. valid moves
        /// for this checker
        /// </summary>
        List<NetworkBackgammonDice> possibleMoves = new List<NetworkBackgammonDice>();

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_position">Initial position of this checker</param>
        public NetworkBackgammonChecker(NetworkBackgammonPosition _position)
        {
            currentPosition = _position;
        }

        /// <summary>
        /// Performs a move of this checker
        /// </summary>
        /// <param name="_move">Move (dice value) to be performed</param>
        public void MoveChecker(NetworkBackgammonDice _move)
        {
            currentPosition += _move;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of possible moves for this checker
        /// </summary>
        public List<NetworkBackgammonDice> PossibleMoves
        {
            get
            {
                return possibleMoves;
            }
        }

        /// <summary>
        /// Gets the current position of this checker
        /// </summary>
        public NetworkBackgammonPosition CurrentPosition
        {
            get
            {
                return currentPosition;
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return currentPosition.ToString();
        }

        #endregion
    }
}
