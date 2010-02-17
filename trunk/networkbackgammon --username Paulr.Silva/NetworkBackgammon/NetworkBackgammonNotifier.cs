using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace NetworkBackgammon
{
    public class NetworkBackgammonNotifier
    {
        // List of registered listeners
        ArrayList m_listeners = new ArrayList();

        // Destructor clears all listeners
        ~NetworkBackgammonNotifier()
        {
            foreach (NetworkBackgammonListener listner in m_listeners)
            {
                listner.RemoveNotifier(this);
            }

            m_listeners.Clear();
        }

        // Register a listener - fails if already in the list
        public bool AddListener(NetworkBackgammonListener listener)
        {
            bool retval = false;

            if (!m_listeners.Contains(listener))
            {
                listener.AddNotifier(this);
                retval = (m_listeners.Add(listener) >= 0 ? true : false);
            }

            return retval;
        }

        // Remove listener - fails if not found
        public bool RemoveListener(NetworkBackgammonListener listener)
        {
            bool retval = false;

            retval = m_listeners.Contains(listener);
            
            if( retval )
            {
                listener.RemoveNotifier(this);
                m_listeners.Remove(listener);
            }

            return retval;
        }
    
        // Broadcast notification to listeners
        public void Broadcast(INetworkBackgammonEvent notificationEvent)
        {
            foreach (NetworkBackgammonListener listner in m_listeners)
            {
                listner.OnEventNotification(this, notificationEvent);
            }
        }
    }
}
