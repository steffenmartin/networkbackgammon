using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// Event for selecting a (valid) move of a checker (by the player)
    /// </summary>
    [Serializable]
    public class GameSessionMoveSelectedEvent : NetworkBackgammonGameSessionEvent
    {
        #region Members

        /// <summary>
        /// The selected checker to be moved
        /// </summary>
        private NetworkBackgammonChecker checkerSelected;

        /// <summary>
        /// The selected (valid) move to be performed with the selected checker (according to a dice value)
        /// </summary>
        private NetworkBackgammonDice moveSelected;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_checkerSelected">The selected checker to be moved</param>
        /// <param name="_moveSelected">The selected move to be performed with the selected checker</param>
        public GameSessionMoveSelectedEvent(NetworkBackgammonChecker _checkerSelected, NetworkBackgammonDice _moveSelected)
        {
            checkerSelected = _checkerSelected;
            moveSelected = _moveSelected;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the selected checker to be moved
        /// </summary>
        public NetworkBackgammonChecker CheckerSelected
        {
            get
            {
                return checkerSelected;
            }
        }

        /// <summary>
        /// Gets the selected move to be performed with the selected checker
        /// </summary>
        public NetworkBackgammonDice MoveSelected
        {
            get
            {
                return moveSelected;
            }
        }

        #endregion
    }
}
