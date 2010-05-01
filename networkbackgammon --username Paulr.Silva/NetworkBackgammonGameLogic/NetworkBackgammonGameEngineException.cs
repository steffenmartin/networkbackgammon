using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    /// <summary>
    /// Exceptions thrown by the Game Engine
    /// </summary>
    public class NetworkBackgammonGameEngineException : Exception
    {
        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception details</param>
        public NetworkBackgammonGameEngineException(string message) : base(message)
        {
        }

        #endregion
    }
}
