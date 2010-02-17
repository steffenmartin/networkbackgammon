using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace NetworkBackgammon
{
    public class NetworkBackgammonListener
    {
        // List of notifiers that this class is registered with 
        ArrayList m_attachedNotifiers = new ArrayList();

        // Destructor removes itself from all notifiers it is registered with
        ~NetworkBackgammonListener()
        {
            foreach (NetworkBackgammonNotifier notifier in m_attachedNotifiers)
            {
                notifier.RemoveListener(this);
            }
        }

        // Add a notifier to the list - fails if notifier already is in list
        public bool AddNotifier(NetworkBackgammonNotifier notifier)
        {
            bool retval = false;

            if (!m_attachedNotifiers.Contains(notifier))
            {
                retval = (m_attachedNotifiers.Add(notifier) >= 0 ? true : false);
            }

            return retval;
        }

        // Remove a notifier from the list - fails if cannot find notifier
        public bool RemoveNotifier(NetworkBackgammonNotifier notifier)
        {
            bool retval = false;

            retval = m_attachedNotifiers.Contains(notifier);

            if( retval )
            {
                m_attachedNotifiers.Remove(notifier);
            }

            return retval;
        }

        // Handle incoming notification from a notifier
        public void OnEventNotification(NetworkBackgammonNotifier sender, INetworkBackgammonEvent e) { }
    }
}
