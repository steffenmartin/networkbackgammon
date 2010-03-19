using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// "Default" implementation
    /// </summary>
    [Serializable]
    public class NetworkBackgammonNotifier : INetworkBackgammonNotifier
    {
        List<INetworkBackgammonListener> m_listeners = new List<INetworkBackgammonListener>();
        // The actual notifier (sender)
        INetworkBackgammonNotifier sender = null;
        // Lock to make the singleton thread sage
        static readonly object padlock = new object();

        // Constructor sets the actual notifier
        public NetworkBackgammonNotifier(INetworkBackgammonNotifier _sender)
        {
            sender = _sender;
        }

        // Destructor clears all listeners
        ~NetworkBackgammonNotifier()
        {
            m_listeners.Clear();
        }

        #region INetworkBackgammonNotifier Members

        // Register a listener - fails if already in the list
        public bool AddListener(INetworkBackgammonListener listener)
        {
            bool retval = false;

            lock (padlock)
            {
                if (!m_listeners.Contains(listener))
                {
                    listener.AddNotifier(this);
                    m_listeners.Add(listener);

                    retval = true;
                }
            }

            return retval;
        }

        // Remove listener - fails if not found
        public bool RemoveListener(INetworkBackgammonListener listener)
        {
            bool retval = false;

            lock (padlock)
            {
                retval = m_listeners.Contains(listener);

                if (retval)
                {
                    //listener.RemoveNotifier(this);
                    m_listeners.Remove(listener);
                }
            }

            return retval;
        }

        // Broadcast notification to listeners
        public void Broadcast(INetworkBackgammonEvent notificationEvent)
        {
            lock (padlock)
            {
                foreach (INetworkBackgammonListener listner in m_listeners)
                {
                    listner.OnEventNotification(sender, notificationEvent);
                }
            }
        }

        // Broadcast notification to listeners on behalt of a sender
        public void Broadcast(INetworkBackgammonEvent notificationEvent, INetworkBackgammonNotifier notifier)
        {
            lock (padlock)
            {
                foreach (INetworkBackgammonListener listner in m_listeners)
                {
                    listner.OnEventNotification(notifier, notificationEvent);
                }
            }
        }

        #endregion
    }
}
