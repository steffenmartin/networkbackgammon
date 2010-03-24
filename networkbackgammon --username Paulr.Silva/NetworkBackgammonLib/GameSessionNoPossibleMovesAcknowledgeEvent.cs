using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Event for confirmation of the active players who has no (valid) moves
    /// </summary>
    [Serializable]
    public class GameSessionNoPossibleMovesAcknowledgeEvent : NetworkBackgammonGameSessionEvent
    {
        #region Members

        /// <summary>
        /// Player who akcnowledges no (valid) moves
        /// </summary>
        private string playerName;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_player">Player who akcnowledges no (valid) moves</param>
        public GameSessionNoPossibleMovesAcknowledgeEvent(string _playerName)
        {
            playerName = _playerName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the player who akcnowledges no (valid) moves
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
