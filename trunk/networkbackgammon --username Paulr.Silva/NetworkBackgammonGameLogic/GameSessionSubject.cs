using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public abstract class GameSessionSubject
    {
        // List of registered listeners
        private List<IGameSessionListener> listeners = new List<IGameSessionListener>();

        // Destructor clears all listeners
        ~GameSessionSubject()
        {
            listeners.Clear();

            listeners = null;
        }

        public void AddListener(IGameSessionListener _listener)
        {
            if (!listeners.Contains(_listener))
            {
                listeners.Add(_listener);
            }
        }

        public void RemoveListener(IGameSessionListener _listener)
        {
            listeners.Remove(_listener);
        }

        public void Broadcast(GameSessionEvent _event, GameSessionSubject _subject)
        {
            foreach (IGameSessionListener listener in listeners)
            {
                listener.Notify(_event, _subject, null);
            }
        }

        public void Broadcast(GameSessionEvent _event, GameSessionSubject _subject, IPlayerEventInfo _playerInfo)
        {
            foreach (IGameSessionListener listener in listeners)
            {
                listener.Notify(_event, _subject, _playerInfo);
            }
        }
    }
}
