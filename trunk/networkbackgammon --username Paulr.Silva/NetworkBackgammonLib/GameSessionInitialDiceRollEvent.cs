using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Event for initial dice rolling
    /// </summary>
    [Serializable]
    public class GameSessionInitialDiceRollEvent : NetworkBackgammonGameSessionEvent
    {
        #region Members

        /// <summary>
        /// Hashtable to associate (assign) a dice (initial dice roll) to a player
        /// </summary>
        private Dictionary<string, NetworkBackgammonDice> diceRolled = new Dictionary<string, NetworkBackgammonDice>();

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_player1Name">Name of player 1</param>
        /// <param name="_dicePlayer1Rolled">Dice (roll result) for player 1</param>
        /// <param name="_player2Name">Name of player 2</param>
        /// <param name="_dicePlayer2Rolled">Dice (roll result) for player 2</param>
        public GameSessionInitialDiceRollEvent(
            string _player1Name,
            NetworkBackgammonDice _dicePlayer1Rolled,
            string _player2Name,
            NetworkBackgammonDice _dicePlayer2Rolled)
        {
            diceRolled.Add(_player1Name, _dicePlayer1Rolled);
            diceRolled.Add(_player2Name, _dicePlayer2Rolled);
        }

        /// <summary>
        /// Gets the initial dice (roll result) for a selected player
        /// </summary>
        /// <param name="_player">Name of the player</param>
        /// <returns>The dice (roll result) for the selected player.</returns>
        public NetworkBackgammonDice GetDiceForPlayer(string _player)
        {
            NetworkBackgammonDice retVal = null;

            if (diceRolled.Keys.Contains(_player))
            {
                retVal = diceRolled[_player];
            }

            return retVal;
        }

        #endregion
    }
}
