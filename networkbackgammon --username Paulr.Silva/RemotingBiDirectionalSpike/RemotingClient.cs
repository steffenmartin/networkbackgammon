using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;

namespace RemotingBiDirectionalSpike
{
    [Serializable]
    public class RemotingClient : MarshalByRefObject
    {
        public delegate void ClientCallback(string _message);
        public delegate void ServerNotificationCallback();

        public event ClientCallback clientCallback;
        public event ServerNotificationCallback serverNotificationCallback;

        #region RemotingClient Members

        // This really should be private but for proper serialization this is required to be public
        // -> Need to find smarter solution
        public int clientID = 0;

        public RemotingClient()
        {
            // Create a unique client ID (Is this function really creating just uniqe IDs?)
            clientID = System.Guid.NewGuid().GetHashCode();
        }

        ~RemotingClient()
        {
            clientID = 0;
        }

        public void SendMessage(string _message)
        {
            if (clientCallback != null)
                clientCallback(_message);
        }
        
        public void OnServerNotification()
        {
            if (serverNotificationCallback != null)
            {
                serverNotificationCallback();
            }
        }

        public int ClientID
        {
            get
            {
                return clientID;
            }
        }
        

        #endregion

        #region RemotingClient Operators

        public static bool operator ==(RemotingClient a, RemotingClient b)
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

        public static bool operator !=(RemotingClient a, RemotingClient b)
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
                return (RemotingClient)obj == this;
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
    }
}
