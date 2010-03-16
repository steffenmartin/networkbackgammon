using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    [Serializable]
    public class NetworkBackgammonDice
    {
        public enum DiceValue
        {
            MIN = 1,
            ONE = MIN,
            TWO,
            THREE,
            FOUR,
            FIVE,
            SIX,
            MAX = SIX
        };

        private DiceValue currentValue = DiceValue.ONE;
        Random rand = null;

        public NetworkBackgammonDice()
        {
            rand = new Random();
        }

        public NetworkBackgammonDice(Int32 _seed)
        {
            rand = new Random(_seed);
        }

        public DiceValue Roll()
        {
            currentValue = (DiceValue) Enum.Parse(typeof(DiceValue), rand.Next(Convert.ToInt32(DiceValue.MIN), Convert.ToInt32(DiceValue.MAX)).ToString());
            
            return currentValue;
        }

        public UInt32 RollUInt32()
        {
            return Convert.ToUInt32(Roll());
        }

        public DiceValue CurrentValue
        {
            get
            {
                return currentValue;
            }
        }

        public UInt32 CurrentValueUInt32
        {
            get
            {
                return Convert.ToUInt32(currentValue);
            }
        }

        public override string ToString()
        {
            return currentValue.ToString();
        }
    }
}
