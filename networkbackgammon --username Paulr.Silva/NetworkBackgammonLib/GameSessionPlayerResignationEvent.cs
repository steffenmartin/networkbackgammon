using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Event for resignation of a player from a game
    /// </summary>
    [Serializable]
    public class GameSessionPlayerResignationEvent : NetworkBackgammonGameSessionEvent
    {
        #region Members

        /// <summary>
        /// Name of the player resigning from the game
        /// </summary>
        private string resigningPlayer;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_resigningPlayer">Name of the resigning player</param>
        public GameSessionPlayerResignationEvent(string _resigningPlayer)
        {
            resigningPlayer = _resigningPlayer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the player resigning from the game
        /// </summary>
        public string ResigningPlayer
        {
            get
            {
                return resigningPlayer;
            }
        }

        #endregion
    }
}
