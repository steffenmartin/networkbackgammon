using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public abstract class GameRoomSubject
    {
        // List of registered listeners
        private List<IGameRoomListener> listeners = new List<IGameRoomListener>();

        // Destructor clears all listeners
        ~GameRoomSubject()
        {
            listeners.Clear();

            listeners = null;
        }

        public void AddListener(IGameRoomListener _listener)
        {
            if (!listeners.Contains(_listener))
            {
                listeners.Add(_listener);
            }
        }

        public void RemoveListener(IGameRoomListener _listener)
        {
            listeners.Remove(_listener);
        }

        public void BroadCast(GameRoomEvent _event)
        {
            foreach (IGameRoomListener listener in listeners)
            {
                listener.Notify(_event);
            }
        }
    }
}
