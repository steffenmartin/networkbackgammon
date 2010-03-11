using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    public interface INetworkBackgammonListener
    {
        /**
         * Add a notifier object to the notifier list. This is used to 
         * unregister this listener from all of its notification objects 
         * it has register with.
         * @param notifier Notification object to add to the notification list
         * @return bool Return True on successful addition of the notifier
         */
        bool AddNotifier(INetworkBackgammonNotifier notifier);

        /**
         * Remove notifier object from the notification list this listener has register with.
         * @param notifier Notification object to remove from notification list
         * @return bool Return True on successful removal of the notifier
         */ 
        bool RemoveNotifier(INetworkBackgammonNotifier notifier);

        /**
         * This routine is called when ever the notifier broadcasts an event to any of its listeners.
         * @param sender Notification object that send the event
         * @param e This is event type object
         */
        void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e);
    }
}
