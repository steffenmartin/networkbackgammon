using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Event for acknowledging initial dice rolling (by players)
    /// </summary>
    [Serializable]
    public class GameSessionInitialDiceRollAcknowledgeEvent : NetworkBackgammonGameSessionEvent
    {
        #region Members

        /// <summary>
        /// Name of the player acknowledging the initial dice roll
        /// </summary>
        private string acknowledingPlayer;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_acknowledingPlayer">Name of the acknowledging player</param>
        public GameSessionInitialDiceRollAcknowledgeEvent(string _acknowledingPlayer)
        {
            acknowledingPlayer = _acknowledingPlayer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the acknowledging player
        /// </summary>
        public string AcknowledgingPlayer
        {
            get
            {
                return acknowledingPlayer;
            }
        }

        #endregion
    }
}
