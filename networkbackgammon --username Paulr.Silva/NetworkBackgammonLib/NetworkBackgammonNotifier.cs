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

            if (!m_listeners.Contains(listener))
            {
                listener.AddNotifier(this);
                m_listeners.Add(listener);

                retval = true;
            }

            return retval;
        }

        // Remove listener - fails if not found
        public bool RemoveListener(INetworkBackgammonListener listener)
        {
            bool retval = m_listeners.Contains(listener);

            if (retval)
            {
                //listener.RemoveNotifier(this);
                m_listeners.Remove(listener);
            }
            return retval;
        }

        // Broadcast notification to listeners
        public void Broadcast(INetworkBackgammonEvent notificationEvent)
        {
            foreach (INetworkBackgammonListener listner in m_listeners)
            {
                listner.OnEventNotification(sender, notificationEvent);
            }
        }

        // Broadcast notification to listeners on behalt of a sender
        public void Broadcast(INetworkBackgammonEvent notificationEvent, INetworkBackgammonNotifier notifier)
        {
           foreach (INetworkBackgammonListener listner in m_listeners)
            {
                listner.OnEventNotification(notifier, notificationEvent);
            }
        }

        #endregion
    }
}
