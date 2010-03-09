using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class Position
    {
        /// <summary>
        /// (Possible) positions of checkers on the game board
        /// </summary>
        public enum GameBoardPosition
        {
            /// <summary>
            /// Start of "normal" positions (i.e. between 1 and 24)
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
            /// End of "normal" positions (i.e. between 1 and 24)
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

        private GameBoardPosition currentPosition;

        public Position(GameBoardPosition _position)
        {
            currentPosition = _position;
        }

        public GameBoardPosition CurrentPosition
        {
            get
            {
                return currentPosition;
            }
        }

        public static Position operator +(Position a, Dice b)
        {
            Position retVal = null;

            switch (a.currentPosition)
            {
                case GameBoardPosition.INVALID:
                    retVal = new Position(GameBoardPosition.INVALID);
                    break;
                case GameBoardPosition.BAR:
                    retVal = new Position((GameBoardPosition) Enum.Parse(typeof(GameBoardPosition), Convert.ToUInt32(b.CurrentValue).ToString()));
                    break;
                case GameBoardPosition.OFFBOARD:
                    retVal = new Position(GameBoardPosition.OFFBOARD);
                    break;
                default:
                    UInt32 newPos = Convert.ToUInt32(a.CurrentPosition) + Convert.ToUInt32(b.CurrentValue);

                    if (newPos > Convert.ToUInt32(GameBoardPosition.NORMAL_END))
                    {
                        retVal = new Position(GameBoardPosition.OFFBOARD);
                    }
                    else
                    {
                        retVal = new Position((GameBoardPosition)Enum.Parse(typeof(GameBoardPosition), Convert.ToUInt32(newPos).ToString()));
                    }
                    break;
            }

            return retVal;
        }

        public static bool operator ==(Position a, Position b)
        {
            return a.currentPosition == b.currentPosition;
        }

        public static bool operator !=(Position a, Position b)
        {
            return a.currentPosition != b.currentPosition;
        }

        public void Reset()
        {
            currentPosition = GameBoardPosition.BAR;
        }

        public Position GetOppositePosition()
        {
            Position retVal = null;

            if (currentPosition >= Position.GameBoardPosition.NORMAL_START &&
                currentPosition <= Position.GameBoardPosition.NORMAL_END)
            {
                UInt32 oppositeValue = (UInt32)GameBoardPosition.NORMAL_END - (UInt32)currentPosition + 1;

                GameBoardPosition oppositePosition = (GameBoardPosition)oppositeValue;

                retVal = new Position(oppositePosition);
            }
            else
            {
                retVal = new Position(GameBoardPosition.INVALID);
            }

            return retVal;
        }

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
    }
}
