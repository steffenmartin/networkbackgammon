using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    public interface INetworkBackgammonNotifier
    {
        /**
         * Add a listener object to the notifier list. This is used to broadcast an event
         * object to all listeners contained (registered) in the list.
         * @param notifier INetworkBackgammonListener object to add to the listener list
         * @return bool Return True on successful addition of the listener
         */
        bool AddListener(INetworkBackgammonListener listener);

        /**
        * Remove an existing listener object from the listener list. 
        * @param notifier INetworkBackgammonListener object to be removed from the listener list
        * @return bool Return True on successful removal of the listener
        */
        bool RemoveListener(INetworkBackgammonListener listener);

        /**
        * Broadcast an event to all registered listeners
        * @param notificationEvent INetworkBackgammonEvent object broadcast
        */
        void Broadcast(INetworkBackgammonEvent notificationEvent);
    }
}
