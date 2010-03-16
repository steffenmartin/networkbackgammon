using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NetworkBackgammonLib
{
    /// <summary>
    /// "Default" implementation
    /// </summary>
    [Serializable]
    public class NetworkBackgammonNotifier : INetworkBackgammonNotifier
    {
        // Named emaphore to lock network notifier across process boundaries
        //private static Semaphore _semLock;
        // List of registered listeners
        List<INetworkBackgammonListener> m_listeners = new List<INetworkBackgammonListener>();
        // The actual notifier (sender)
        INetworkBackgammonNotifier sender = null;
        // Semaphore lock timeout (ms)
        int semLockTimeout = 1000;

        // Constructor sets the actual notifier
        public NetworkBackgammonNotifier(INetworkBackgammonNotifier _sender)
        {
            //_semLock = new Semaphore(0, 1);// "NetworkBackgammonSemLock");

            sender = _sender;
        }

        // Destructor clears all listeners
        ~NetworkBackgammonNotifier()
        {
            // Wait for semaphore to become avaiable
            //if (_semLock.WaitOne(semLockTimeout))
            {
                foreach (INetworkBackgammonListener listner in m_listeners)
                {
                   listner.RemoveNotifier(this);
                }

                m_listeners.Clear();

                //_semLock.Release();
            }
        }

        #region INetworkBackgammonNotifier Members

        // Register a listener - fails if already in the list
        public bool AddListener(INetworkBackgammonListener listener)
        {
            bool retval = false;

             // Wait for semaphore to become avaiable
            //if (_semLock.WaitOne(semLockTimeout))
            {
                if (!m_listeners.Contains(listener))
                {
                    listener.AddNotifier(this);
                    m_listeners.Add(listener);

                    retval = true;
                }

                //_semLock.Release();
            }

            return retval;
        }

        // Remove listener - fails if not found
        public bool RemoveListener(INetworkBackgammonListener listener)
        {
            bool retval = false;

            // Wait for semaphore to become avaiable
            //if (_semLock.WaitOne(semLockTimeout))
            {
                retval = m_listeners.Contains(listener);

                if (retval)
                {
                    listener.RemoveNotifier(this);
                    m_listeners.Remove(listener);
                }

                //_semLock.Release();
            }

            return retval;
        }

        // Broadcast notification to listeners
        public void Broadcast(INetworkBackgammonEvent notificationEvent)
        {
            // Wait for semaphore to become avaiable
            //if (_semLock.WaitOne(semLockTimeout))
            {
                foreach (INetworkBackgammonListener listner in m_listeners)
                {
                    listner.OnEventNotification(sender, notificationEvent);
                }

                //_semLock.Release();
            }
        }

        // Broadcast notification to listeners on behalt of a sender
        public void Broadcast(INetworkBackgammonEvent notificationEvent, INetworkBackgammonNotifier notifier)
        {
             // Wait for semaphore to become avaiable
            //if (_semLock.WaitOne(semLockTimeout))
            {
                foreach (INetworkBackgammonListener listner in m_listeners)
                {
                    listner.OnEventNotification(notifier, notificationEvent);
                }

                //_semLock.Release();
            }
        }

        #endregion
    }
}
