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

        /// <summary>
        /// A unique ID for every instance of this object
        /// </summary>
        /// <remarks>
        /// This is mainly required for remoting purposes to be able to compare two objects with at least on of them being transmitted via remoting, thus
        /// having a different object reference although it could potentially be the same exact object.
        /// </remarks>
        int uniqueObjectID = 0;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// Parameterless constructor, mainly required for serialization.
        /// </remarks>
        public NetworkBackgammonChecker()
        {
            currentPosition = new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.INVALID);

            // Create a unique object ID (Is this function really creating just uniqe IDs?)
            uniqueObjectID = System.Guid.NewGuid().GetHashCode();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_position">Initial position of this checker</param>
        public NetworkBackgammonChecker(NetworkBackgammonPosition _position)
        {
            currentPosition = _position;

            // Create a unique object ID (Is this function really creating just uniqe IDs?)
            uniqueObjectID = System.Guid.NewGuid().GetHashCode();
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
        /// <remarks>
        /// Remove 'set' part as soon as all testing & debugging is completed.
        /// It's currently used for XML (de-)serialization.
        /// </remarks>
        public NetworkBackgammonPosition CurrentPosition
        {
            get
            {
                return currentPosition;
            }
            set
            {
                currentPosition = value;
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return currentPosition.ToString();
        }

        public override int GetHashCode()
        {
            return uniqueObjectID;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Tests two checkers for equality
        /// </summary>
        /// <param name="a">Checker 1</param>
        /// <param name="b">Checker 2</param>
        /// <returns>"True" if both checker refer to the same original checker, otherwise "false"</returns>
        public static bool operator ==(NetworkBackgammonChecker a, NetworkBackgammonChecker b)
        {
            try
            {
                return a.uniqueObjectID == b.uniqueObjectID;
            }
            catch (NullReferenceException)
            {
                return ((object)a == null);
            }
        }

        /// <summary>
        /// Tests two checkers for inequality
        /// </summary>
        /// <param name="a">Checker 1</param>
        /// <param name="b">Checker 2</param>
        /// <returns>"True" if both checkers refer to different original checkers, otherwise "false"</returns>
        public static bool operator !=(NetworkBackgammonChecker a, NetworkBackgammonChecker b)
        {
            try
            {
                return a.uniqueObjectID != b.uniqueObjectID;
            }
            catch (NullReferenceException)
            {
                return ((object)a != null);
            }
        }

        #endregion
    }
}
