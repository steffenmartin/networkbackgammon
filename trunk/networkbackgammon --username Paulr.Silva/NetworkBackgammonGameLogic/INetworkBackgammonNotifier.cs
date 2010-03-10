using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public interface INetworkBackgammonNotifier
    {
        bool AddListener(INetworkBackgammonListener listener);
        bool RemoveListener(INetworkBackgammonListener listener);
        void Broadcast(INetworkBackgammonEvent notificationEvent);
    }
}
