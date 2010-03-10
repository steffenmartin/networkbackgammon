using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public interface INetworkBackgammonListener
    {
        bool AddNotifier(INetworkBackgammonNotifier notifier);
        bool RemoveNotifier(INetworkBackgammonNotifier notifier);
        void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e);
    }
}
