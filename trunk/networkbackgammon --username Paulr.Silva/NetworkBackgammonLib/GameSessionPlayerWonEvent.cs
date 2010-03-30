using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Event for a win of the game by one player
    /// </summary>
    public class GameSessionPlayerWonEvent : NetworkBackgammonGameSessionEvent
    {
        #region Members

        /// <summary>
        /// Name of the player who won the game
        /// </summary>
        private string winningPlayer;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_winningPlayer">Winning players name</param>
        public GameSessionPlayerWonEvent(string _winningPlayer)
        {
            winningPlayer = _winningPlayer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the player who won the game
        /// </summary>
        public string WinningPlayer
        {
            get
            {
                return winningPlayer;
            }
        }

        #endregion
    }
}
