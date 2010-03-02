using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public class Position
    {
        public enum GameBoardPosition
        {
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
            NORMAL_END = TWENTYFOUR,
            BAR,
            OFFBOARD,
            INVALID,
            HOME_START = NINETEEN,
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
