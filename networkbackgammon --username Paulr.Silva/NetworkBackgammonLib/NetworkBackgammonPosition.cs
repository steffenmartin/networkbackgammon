using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Position (column) anywhere on the game board
    /// </summary>
    /// <remarks>
    /// Generally speaking, every backgammon game board has 24 possible "normal"
    /// positions (points 1 through 24), regardless of the viewpoint of the players.
    /// Besides these positions there are special positions where checkers can 
    /// move to, which is the "Bar" position (when a checker gets hit by opponents
    /// checker) and "off the board" (when a player "bears off" checkers).
    /// From one player's perspective, his/her checkers move from lower positions 
    /// (lower point number) to higher positions (higher point number) and the opponents
    /// checkers move from higher positions (higher point number) to lower positions (lower 
    /// point number).
    /// See <seealso cref="http://www.bkgm.com/glossary.html"/>.
    /// </remarks>
    [Serializable]
    public class NetworkBackgammonPosition
    {
        #region Declarations

        /// <summary>
        /// Enumeration of (possible) positions of checkers on the game board
        /// </summary>
        public enum GameBoardPosition
        {
            /// <summary>
            /// Start of "normal" positions (i.e. between points 1 and 24)
            /// </summary>
            NORMAL_START = 1,
            ONE = NORMAL_START,
            TWO,
            THREE,
            FOUR,
            FIVE,
            SIX,
            SEVEN,
            EIGHT,
            NINE,
            TEN,
            ELEVEN,
            TWELVE,
            THIRTEEN,
            FOURTEEN,
            FIFTEEN,
            SIXTEEN,
            SEVENTEEN,
            EIGHTEEN,
            NINETEEN,
            TWENTY,
            TWENTYONE,
            TWENTYTWO,
            TWENTYTHREE,
            TWENTYFOUR,
            /// <summary>
            /// End of "normal" positions (i.e. between points 1 and 24)
            /// </summary>
            NORMAL_END = TWENTYFOUR,
            /// <summary>
            /// Bar
            /// </summary>
            BAR,
            /// <summary>
            /// Off the board
            /// </summary>
            OFFBOARD,
            INVALID,
            /// <summary>
            /// Start of "home" positions (i.e. between 19 and 24)
            /// </summary>
            HOME_START = NINETEEN,
            /// <summary>
            /// End of "home" positions (i.e. between 19 and 24)
            /// </summary>
            HOME_END = TWENTYFOUR
        }

        #endregion

        #region Members

        /// <summary>
        /// Current position on the game board
        /// </summary>
        private GameBoardPosition currentPosition;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// Parameterless constructor, mainly required for serialization.
        /// </remarks>
        public NetworkBackgammonPosition()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_position">Initial position</param>
        public NetworkBackgammonPosition(GameBoardPosition _position)
        {
            currentPosition = _position;
        }

        /// <summary>
        /// Resets the current position to the "Bar" position
        /// </summary>
        public void Reset()
        {
            currentPosition = GameBoardPosition.BAR;
        }

        /// <summary>
        /// Gets the current position as seen from the opponent's (opposite) side
        /// </summary>
        /// <returns>Opposite position of the current position</returns>
        public NetworkBackgammonPosition GetOppositePosition()
        {
            NetworkBackgammonPosition retVal = null;

            // An opposite position exists only for "normal" positions
            if (currentPosition >= NetworkBackgammonPosition.GameBoardPosition.NORMAL_START &&
                currentPosition <= NetworkBackgammonPosition.GameBoardPosition.NORMAL_END)
            {
                UInt32 oppositeValue = (UInt32)GameBoardPosition.NORMAL_END - (UInt32)currentPosition + 1;

                GameBoardPosition oppositePosition = (GameBoardPosition)oppositeValue;

                retVal = new NetworkBackgammonPosition(oppositePosition);
            }
            else
            {
                retVal = new NetworkBackgammonPosition(GameBoardPosition.INVALID);
            }

            return retVal;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current position on the game board
        /// </summary>
        /// <remarks>
        /// Remove 'set' part as soon as all testing & debugging is completed.
        /// It's currently used for XML (de-)serialization.
        /// </remarks>
        public GameBoardPosition CurrentPosition
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

        #region Operators

        /// <summary>
        /// Calculates a new position based on a given position and a move (dice value) to be applied
        /// </summary>
        /// <param name="a">Position the move is to be based on</param>
        /// <param name="b">Move (dice value)</param>
        /// <returns>New position (after performing the potential move)</returns>
        public static NetworkBackgammonPosition operator +(NetworkBackgammonPosition a, NetworkBackgammonDice b)
        {
            NetworkBackgammonPosition retVal = null;

            switch (a.currentPosition)
            {
                case GameBoardPosition.INVALID:
                    retVal = new NetworkBackgammonPosition(GameBoardPosition.INVALID);
                    break;
                case GameBoardPosition.BAR:
                    retVal = new NetworkBackgammonPosition((GameBoardPosition)Enum.Parse(typeof(GameBoardPosition), Convert.ToUInt32(b.CurrentValue).ToString()));
                    break;
                case GameBoardPosition.OFFBOARD:
                    retVal = new NetworkBackgammonPosition(GameBoardPosition.OFFBOARD);
                    break;
                default:
                    UInt32 newPos = Convert.ToUInt32(a.CurrentPosition) + Convert.ToUInt32(b.CurrentValue);

                    if (newPos > Convert.ToUInt32(GameBoardPosition.NORMAL_END))
                    {
                        retVal = new NetworkBackgammonPosition(GameBoardPosition.OFFBOARD);
                    }
                    else
                    {
                        retVal = new NetworkBackgammonPosition((GameBoardPosition)Enum.Parse(typeof(GameBoardPosition), Convert.ToUInt32(newPos).ToString()));
                    }
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Tests two position for equality
        /// </summary>
        /// <param name="a">Position 1</param>
        /// <param name="b">Position 2</param>
        /// <returns>"True" if both positions refer to the same point (column), otherwise "false"</returns>
        public static bool operator ==(NetworkBackgammonPosition a, NetworkBackgammonPosition b)
        {
            return a.currentPosition == b.currentPosition;
        }

        /// <summary>
        /// Tests two position for inequality
        /// </summary>
        /// <param name="a">Position 1</param>
        /// <param name="b">Position 2</param>
        /// <returns>"True" if both positions refer to different points (column), otherwise "false"</returns>
        public static bool operator !=(NetworkBackgammonPosition a, NetworkBackgammonPosition b)
        {
            return a.currentPosition != b.currentPosition;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return currentPosition.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        #endregion
    }
}
