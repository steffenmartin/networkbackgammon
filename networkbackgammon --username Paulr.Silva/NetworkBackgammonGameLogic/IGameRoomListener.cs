using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public interface IGameRoomListener
    {
        void Notify(GameRoomEvent _event);
    }
}
