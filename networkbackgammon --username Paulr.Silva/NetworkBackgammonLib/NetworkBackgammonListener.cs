using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// "Default" implementation
    /// </summary>
    [Serializable]
    public class NetworkBackgammonListener : INetworkBackgammonListener
    {
        // List of notifiers that this class is registered with 
        List<INetworkBackgammonNotifier> m_attachedNotifiers = new List<INetworkBackgammonNotifier>();

        // Destructor removes itself from all notifiers it is registered with
        ~NetworkBackgammonListener()
        {
            foreach (INetworkBackgammonNotifier notifier in m_attachedNotifiers)
            {
                notifier.RemoveListener(this);
            }
        }

        #region INetworkBackgammonListener Members

        // Add a notifier to the list - fails if notifier already is in list
        bool INetworkBackgammonListener.AddNotifier(INetworkBackgammonNotifier notifier)
        {
            bool retval = false;

            if (!m_attachedNotifiers.Contains(notifier))
            {
                m_attachedNotifiers.Add(notifier);
                retval = true;
            }

            return retval;
        }

        // Remove a notifier from the list - fails if cannot find notifier
        bool INetworkBackgammonListener.RemoveNotifier(INetworkBackgammonNotifier notifier)
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
        void INetworkBackgammonListener.OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e) { }

        #endregion
    }
}
