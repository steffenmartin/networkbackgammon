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
    public class NetworkBackgammonDice : IComparable
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
            MAX = SIX,
            INVALID
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
        [NonSerialized]
        Random rand = null;

        /// <summary>
        /// Flag to indicate whether or not the dice has been used
        /// (played)
        /// </summary>
        bool flagUsed = false;

        /// <summary>
        /// The seed used to create the (pseudo) random generator instance
        /// </summary>
        Int32 seed = 0;

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
        public NetworkBackgammonDice()
        {
            seed = DateTime.Now.Millisecond;

            rand = new Random(seed);

            // Create a unique object ID (Is this function really creating just uniqe IDs?)
            uniqueObjectID = System.Guid.NewGuid().GetHashCode();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_seed">Seed for random number generator</param>
        public NetworkBackgammonDice(Int32 _seed)
        {
            rand = new Random(_seed);

            // Create a unique object ID (Is this function really creating just uniqe IDs?)
            uniqueObjectID = System.Guid.NewGuid().GetHashCode();
        }

        /// <summary>
        /// Rolls this dice (i.e. generates a new random dice value)
        /// </summary>
        /// <returns>Returns the new (current) dice value</returns>
        public DiceValue Roll()
        {
            currentValue = (DiceValue) Enum.Parse(typeof(DiceValue), rand.Next(Convert.ToInt32(DiceValue.MIN), Convert.ToInt32(DiceValue.MAX)).ToString());

            flagUsed = false;

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
        /// <remarks>
        /// Remove 'set' part as soon as all testing & debugging is completed.
        /// It's currently used for XML (de-)serialization.
        /// </remarks>
        public DiceValue CurrentValue
        {
            get
            {
                return currentValue;
            }
            set
            {
                currentValue = value;
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

        /// <summary>
        /// Gets or sets the flag indicating whether dice has been used
        /// </summary>
        public bool FlagUsed
        {
            get
            {
                return flagUsed;
            }
            set
            {
                flagUsed = value;
            }
        }

        /// <summary>
        /// Gets the seed used to create the (pseudo) random generator instance
        /// </summary>
        public Int32 Seed
        {
            get
            {
                return seed;
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return currentValue.ToString();
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
        /// Tests two dice for equality
        /// </summary>
        /// <param name="a">Dice 1</param>
        /// <param name="b">Dice 2</param>
        /// <returns>"True" if both dice refer to the same original dice, otherwise "false"</returns>
        public static bool operator ==(NetworkBackgammonDice a, NetworkBackgammonDice b)
        {
            if ((object)b != null)
            {
                return a.uniqueObjectID == b.uniqueObjectID;
            }
            else
            {
                return (object)a == null;
            }

        }

        /// <summary>
        /// Tests two dice for inequality
        /// </summary>
        /// <param name="a">Dice 1</param>
        /// <param name="b">Dice 2</param>
        /// <returns>"True" if both dice refer to different original dice, otherwise "false"</returns>
        public static bool operator !=(NetworkBackgammonDice a, NetworkBackgammonDice b)
        {
            if ((object)b != null)
            {
                return a.uniqueObjectID != b.uniqueObjectID;
            }
            else
            {
                return (object)a != null;
            }
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return CurrentValueUInt32.CompareTo(((NetworkBackgammonDice)obj).CurrentValueUInt32);
        }

        #endregion
    }
}
