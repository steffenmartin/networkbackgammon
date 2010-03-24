using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Event for signaling players which one of them is epxected to make the next move
    /// </summary>
    [Serializable]
    public class GameSessionMoveExpectedEvent : NetworkBackgammonGameSessionEvent
    {
        #region Members

        /// <summary>
        /// Active player (the one who's expected to make the next move)
        /// </summary>
        private string activePlayer;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_activePlayer">Active player</param>
        public GameSessionMoveExpectedEvent(string _activePlayer)
        {
            activePlayer = _activePlayer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the active player (the one who's expected to make the next move)
        /// </summary>
        public string ActivePlayer
        {
            get
            {
                return activePlayer;
            }
        }

        #endregion
    }
}
