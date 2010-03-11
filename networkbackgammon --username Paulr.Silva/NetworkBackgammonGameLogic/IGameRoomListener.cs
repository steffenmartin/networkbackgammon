using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkBackgammonLib;

namespace NetworkBackgammonGameLogic
{
    public interface IGameRoomListener : INetworkBackgammonListener
    {
        void OnEventNotification(GameRoomEvent _event);
    }
}
