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
        INetworkBackgammonNotifier defaultNotifier = null;
        INetworkBackgammonListener defaultListener = new NetworkBackgammonListener();
        List<NetworkBackgammonChecker> checkers = new List<NetworkBackgammonChecker>();
        string strPlayerName = ""; 
        bool bActive = false;

        public NetworkBackgammonPlayer(string _strPlayerName)
        {
            defaultNotifier = new NetworkBackgammonNotifier(this);

            strPlayerName = _strPlayerName;

            InitCheckers();
        }

        // Initialize list of checkers in their initial positions.
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

        #region Properties

        public string PlayerName
        {
            get
            {
                return strPlayerName;
            }
        }
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
        public List<NetworkBackgammonChecker> Checkers
        {
            get
            {
                return checkers;
            }
        }

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
