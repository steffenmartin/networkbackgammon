using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public interface IGameSessionListener
    {
        void Notify(GameSessionEvent _event, GameSessionSubject _subject, IPlayerEventInfo _playerInfo);
    }
}
