using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkBackgammonLib;

namespace NetworkBackgammonRemotingLib
{
    public class NetworkBackgammonRemoteServer : MarshalByRefObject, 
                                                 INetworkBackgammonNotifier,
                                                 INetworkBackgammonListener
    {
        #region NetworkBackgammonRemoteServer Members

        // Local delegate notifier
        INetworkBackgammonNotifier m_localNotifier = null;
        // Local delegate listener
        INetworkBackgammonListener m_localListener = new NetworkBackgammonListener();
        // Hashtable with clients username and passwords
        Dictionary<string, string> clientUsernameList = new Dictionary<string, string>();
        // Hashtable with clients username and passwords
        List<string> clientActiveList = new List<string>();

        #endregion

        #region NetworkBackgammonRemoteServer Public Members

        // Constructor
        public NetworkBackgammonRemoteServer()
        {
            m_localNotifier = new NetworkBackgammonNotifier(this);
        }

        /**
         * Register as a user into the system.
         * @param username Unique name that the user has provided
         * @param pw Password for the the new user account
         * @return bool True if account was added
         */
        public bool RegisterUser(string username, string pw)
        {
            bool retval = !clientUsernameList.ContainsKey(username);

            if (retval)
            {
                clientUsernameList.Add(username, pw);

                // Broadcast login event
                Broadcast( new NetworkBackgammonChatEvent(), this);
            }

            return retval;
        }

        /**
         * Login user into the system.
         * @param username Unique name that the user has provided
         * @param pw Password for the the new user account
         * @return bool True if client has logged in successfully
         */
        public bool Login(string username, string pw)
        {
            bool retval = (clientUsernameList.ContainsKey(username) &&
                           !IsConnected(username));

            if (retval)
            {
                clientActiveList.Add(username);
            }

            return retval;
        }

        /**
        * Determines whether or not a client is currently logged into the server
        * @param username Unique username
        * @return bool True if user is connected
        */
        public bool IsConnected(string username)
        {
            return clientActiveList.Contains(username);
        }

        #region INetworkBackgammonNotifier Members

        public bool AddListener(INetworkBackgammonListener listener)
        {
            return m_localNotifier != null ? m_localNotifier.AddListener(listener) : false;
        }

        public bool RemoveListener(INetworkBackgammonListener listener)
        {
            return m_localNotifier != null ? m_localNotifier.RemoveListener(listener) : false;
        }

        public void Broadcast(INetworkBackgammonEvent notificationEvent)
        {
            m_localNotifier.Broadcast(notificationEvent);
        }

        public void Broadcast(INetworkBackgammonEvent notificationEvent, INetworkBackgammonNotifier notifier)
        {
            m_localNotifier.Broadcast(notificationEvent, notifier);
        }

        #endregion

        #region INetworkBackgammonListener Members

        public bool AddNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_localListener.AddNotifier(notifier);
        }

        public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_localListener.RemoveNotifier(notifier);
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

        #endregion
    }
}
