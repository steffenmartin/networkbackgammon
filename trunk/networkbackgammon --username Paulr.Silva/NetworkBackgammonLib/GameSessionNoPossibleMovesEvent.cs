using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Event for informing players that currently active player does not have (valid) moves
    /// </summary>
    [Serializable]
    public class GameSessionNoPossibleMovesEvent : NetworkBackgammonGameSessionEvent
    {
        #region Members

        /// <summary>
        /// Player who has no (valid) moves
        /// </summary>
        private string playerName;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_player">Player who has no (valid) moves</param>
        public GameSessionNoPossibleMovesEvent(string _playerName)
        {
            playerName = _playerName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the player who has no (valid) moves
        /// </summary>
        public string PlayerName
        {
            get
            {
                return playerName;
            }
        }

        #endregion
    }
}
