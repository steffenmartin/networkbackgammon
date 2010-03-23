using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Event for updated checkers on players game boards
    /// </summary>
    [Serializable]
    public class GameSessionCheckerUpdatedEvent : NetworkBackgammonGameSessionEvent
    {
        #region Members

        /// <summary>
        /// Both players which have respective game data
        /// (checker, positions, etc) associated with them
        /// </summary>
        private NetworkBackgammonPlayer[] playerData = new NetworkBackgammonPlayer[2] { null, null };

        /// <summary>
        /// Both dice that have been rolled
        /// </summary>
        private NetworkBackgammonDice[] diceRolled = new NetworkBackgammonDice[2] { null, null };

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_player1">Player object for player 1</param>
        /// <param name="_player2">Player object for player 2</param>
        /// <param name="_dice1">Rolled dice number 1</param>
        /// <param name="_dice2">Rolled dice number 2</param>
        public GameSessionCheckerUpdatedEvent(
            NetworkBackgammonPlayer _player1,
            NetworkBackgammonPlayer _player2,
            NetworkBackgammonDice _dice1,
            NetworkBackgammonDice _dice2)
        {
            playerData[0] = _player1;
            playerData[1] = _player2;

            diceRolled[0] = _dice1;
            diceRolled[1] = _dice2;
        }

        /// <summary>
        /// Gets the player object identified by the player name
        /// </summary>
        /// <param name="_playerName">Name of the player the respective object needs to be returned for</param>
        /// <returns>Player object according to the specified name</returns>
        public NetworkBackgammonPlayer GetPlayerByName(string _playerName)
        {
            NetworkBackgammonPlayer retVal = null;

            foreach (NetworkBackgammonPlayer player in playerData)
            {
                if (player.PlayerName == _playerName)
                {
                    retVal = player;
                    break;
                }
            }

            return retVal;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the dice (both) that have been rolled
        /// </summary>
        public NetworkBackgammonDice[] DiceRolled
        {
            get
            {
                return diceRolled;
            }
        }

        #endregion
    }
}
