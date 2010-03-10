using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    /// <summary>
    /// "Default" implementation
    /// </summary>
    public class NetworkBackgammonNotifier : INetworkBackgammonNotifier
    {
        // List of registered listeners
        List<INetworkBackgammonListener> m_listeners = new List<INetworkBackgammonListener>();

        // Destructor clears all listeners
        ~NetworkBackgammonNotifier()
        {
            foreach (INetworkBackgammonListener listner in m_listeners)
            {
                listner.RemoveNotifier(this);
            }

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
            bool retval = false;

            retval = m_listeners.Contains(listener);

            if (retval)
            {
                listener.RemoveNotifier(this);
                m_listeners.Remove(listener);
            }

            return retval;
        }

        // Broadcast notification to listeners
        public void Broadcast(INetworkBackgammonEvent notificationEvent)
        {
            foreach (INetworkBackgammonListener listner in m_listeners)
            {
                listner.OnEventNotification(this, notificationEvent);
            }
        }

        #endregion
    }
}
