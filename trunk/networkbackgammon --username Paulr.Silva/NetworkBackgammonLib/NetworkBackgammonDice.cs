using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Single dice for playing a game (e.g. backgammon)
    /// </summary>
    [Serializable]
    public class NetworkBackgammonDice
    {

        #region Declarations

        /// <summary>
        /// Enumeration of (possible) dice values
        /// </summary>
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

        #endregion

        #region Members

        /// <summary>
        /// Current value of this dice
        /// </summary>
        DiceValue currentValue = DiceValue.ONE;

        /// <summary>
        /// Random number generator
        /// </summary>
        Random rand = null;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkBackgammonDice()
        {
            rand = new Random();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_seed">Seed for random number generator</param>
        public NetworkBackgammonDice(Int32 _seed)
        {
            rand = new Random(_seed);
        }

        /// <summary>
        /// Rolls this dice (i.e. generates a new random dice value)
        /// </summary>
        /// <returns>Returns the new (current) dice value</returns>
        public DiceValue Roll()
        {
            currentValue = (DiceValue) Enum.Parse(typeof(DiceValue), rand.Next(Convert.ToInt32(DiceValue.MIN), Convert.ToInt32(DiceValue.MAX)).ToString());
            
            return currentValue;
        }

        /// <summary>
        /// Rolls this dice (i.e. generates a new random dice value)
        /// </summary>
        /// <returns>Returns the new (current) dice value (converted to UInt32)</returns>
        public UInt32 RollUInt32()
        {
            return Convert.ToUInt32(Roll());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current dice value
        /// </summary>
        public DiceValue CurrentValue
        {
            get
            {
                return currentValue;
            }
        }

        /// <summary>
        /// Gets the current dice value (converted to UInt32)
        /// </summary>
        public UInt32 CurrentValueUInt32
        {
            get
            {
                return Convert.ToUInt32(currentValue);
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return currentValue.ToString();
        }

        #endregion
    }
}
