using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using NetworkBackgammonLib;

namespace NetworkBackgammonRemotingLib
{
   [Serializable]
    public class NetworkBackgammonRemoteClient : MarshalByRefObject,
                                                 INetworkBackgammonNotifier,
                                                 INetworkBackgammonListener
    {
        // Local delegate notifier
        INetworkBackgammonNotifier m_localNotifier = null;
        // Local delegate listener
        INetworkBackgammonListener m_localListener = new NetworkBackgammonListener();
        // Unique client ID
        int clientID = 0;

        public NetworkBackgammonRemoteClient()
        {
            // Create a unique client ID (Is this function really creating just uniqe IDs?)
            clientID = System.Guid.NewGuid().GetHashCode();

            m_localNotifier = new NetworkBackgammonNotifier(this);
        }

        ~NetworkBackgammonRemoteClient(){}

        // Unique client ID property
        public int ClientID
        {
            get
            {
                return clientID;
            }
        }

        #region NetworkBackgammonRemoteClient Operators

        public static bool operator ==(NetworkBackgammonRemoteClient a, NetworkBackgammonRemoteClient b)
        {
            if (((object)a != null) && ((object)b != null))
            {
                return a.clientID == b.clientID;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(NetworkBackgammonRemoteClient a, NetworkBackgammonRemoteClient b)
        {
            if (((object)a != null) && ((object)b != null))
            {
                return a.clientID != b.clientID;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(object obj)
        {
            try
            {
                return (NetworkBackgammonRemoteClient)obj == this;
            }
            catch (Exception)
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            if (clientID == 0)
            {
                return base.GetHashCode();
            }
            else
            {
                return clientID;
            }
        }

        public override string ToString()
        {
            return clientID.ToString();
        }

        #endregion

        #region INetworkBackgammonNotifier Members

        public bool AddListener(INetworkBackgammonListener listener)
        {
            return m_localNotifier != null ? m_localNotifier.AddListener(listener) : false;
        }

        public bool RemoveListener(INetworkBackgammonListener listener)
        {
            return m_localNotifier != null ? m_localNotifier.RemoveListener(listener) : false;
        }

        public void Broadcast(INetworkBackgammonEvent notificationEvent)
        {
            m_localNotifier.Broadcast(notificationEvent);
        }

        public void Broadcast(INetworkBackgammonEvent notificationEvent, INetworkBackgammonNotifier notifier)
        {
            m_localNotifier.Broadcast(notificationEvent, notifier);
        }

        #endregion

        #region INetworkBackgammonListener Members

        public bool AddNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_localListener.AddNotifier(notifier);
        }

        public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_localListener.RemoveNotifier(notifier);
        }

        public void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e)
        {
            // Filter out our own broadcasts
            if (sender != this)
            {
                // Forward event
                Broadcast(e, sender);
            }

            Console.WriteLine("***** Event ***** ");
            Console.WriteLine("Sender: " + sender.ToString());
            Console.WriteLine("Event: " + e.ToString());
         }

        #endregion
    }
}
