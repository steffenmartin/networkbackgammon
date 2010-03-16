using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkBackgammonLib;

namespace NetworkBackgammonLib
{
    [Serializable]
    public class NetworkBackgammonPlayer : MarshalByRefObject,
                                           INetworkBackgammonNotifier, 
                                           INetworkBackgammonListener
    {
        #region Members

        /// <summary>
        /// Instance of the default notifier implementation
        /// </summary>
        /// <remarks>
        /// Created and set during construction time. Used for delegating function
        /// calls on this class' notifier interface implementation.
        /// </remarks>
        INetworkBackgammonNotifier defaultNotifier = null;
        
        /// <summary>
        /// Instance of the default listener implementation
        /// </summary>
        /// <remarks>
        /// Used for delegating function calls on this class' listener interface implementation.
        /// </remarks>
        INetworkBackgammonListener defaultListener = new NetworkBackgammonListener();

        /// <summary>
        /// List of checkers of this player
        /// </summary>
        List<NetworkBackgammonChecker> checkers = new List<NetworkBackgammonChecker>();

        /// <summary>
        /// Name of this player
        /// </summary>
        string strPlayerName = ""; 

        /// <summary>
        /// Status of this player
        /// </summary>
        bool bActive = false;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_strPlayerName">Name of this player</param>
        public NetworkBackgammonPlayer(string _strPlayerName)
        {
            defaultNotifier = new NetworkBackgammonNotifier(this);

            strPlayerName = _strPlayerName;

            InitCheckers();
        }

        /// <summary>
        /// Initializes list of checkers in their initial positions.
        /// </summary>
        private void InitCheckers()
        {
            // 2 checkers on 1
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.ONE)));
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.ONE)));

            // 5 checkers on 12
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.TWELVE)));
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.TWELVE)));
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.TWELVE)));
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.TWELVE)));
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.TWELVE)));

            // 3 checkers on 17
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.SEVENTEEN)));
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.SEVENTEEN)));
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.SEVENTEEN)));

            // 5 checkers on 19
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.NINETEEN)));
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.NINETEEN)));
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.NINETEEN)));
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.NINETEEN)));
            checkers.Add(new NetworkBackgammonChecker(new NetworkBackgammonPosition(NetworkBackgammonPosition.GameBoardPosition.NINETEEN)));
        }

        /// <summary>
        /// Respond to a game challenge
        /// </summary>
        /// <param name="acceptChallenge">"True" if challenge has been accepted, "false" if challenge is rejected</param>
        public void RespondToChallenge(bool acceptChallenge)
        {
            Broadcast(new NetworkBackgammonChallengeResponseEvent(acceptChallenge));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets this player's name
        /// </summary>
        public string PlayerName
        {
            get
            {
                return strPlayerName;
            }
        }

        /// <summary>
        /// Gets this player's status
        /// </summary>
        public bool Active
        {
            get
            {
                return bActive;
            }
            set
            {
                bActive = value;
            }
        }

        /// <summary>
        /// Gets list of checkers of this player
        /// </summary>
        public List<NetworkBackgammonChecker> Checkers
        {
            get
            {
                return checkers;
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return strPlayerName;
        }

        #endregion

        #region INetworkBackgammonNotifier Members

        public bool AddListener(INetworkBackgammonListener listener)
        {
            return defaultNotifier != null ? defaultNotifier.AddListener(listener) : false;
        }

        public bool RemoveListener(INetworkBackgammonListener listener)
        {
            return defaultNotifier != null ? defaultNotifier.RemoveListener(listener) : false;
        }

        public void Broadcast(INetworkBackgammonEvent notificationEvent)
        {
            defaultNotifier.Broadcast(notificationEvent);
        }

        public void Broadcast(INetworkBackgammonEvent notificationEvent, INetworkBackgammonNotifier notifier)
        {
            defaultNotifier.Broadcast(notificationEvent, notifier);
        }

        #endregion

        #region INetworkBackgammonListener Members

        public bool AddNotifier(INetworkBackgammonNotifier notifier)
        {
            return defaultListener.AddNotifier(notifier);
        }

        public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        {
            return defaultListener.RemoveNotifier(notifier);
        }

        public void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e)
        {
            // Filter out our own broadcasts
            if (sender != this)
            {
                // Forward event
                Broadcast(e, sender);
            }
        }

        #endregion
    }
}
