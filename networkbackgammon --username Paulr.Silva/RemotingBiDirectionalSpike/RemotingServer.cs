using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemotingBiDirectionalSpike
{
    public class RemotingServer : MarshalByRefObject
    {
        #region RemotingServer Members

        public event RemotingClient.ClientCallback messageCallback;

        /// <summary>
        /// Hashtable with clients and callback functions they registered for messaging
        /// </summary>
        private Dictionary<RemotingClient, RemotingClient.ClientCallback> clientCallbackList = new Dictionary<RemotingClient, RemotingClient.ClientCallback>();

        public void SendMessage(string _message)
        {
            if (messageCallback != null)
            {
                Delegate[] chain = messageCallback.GetInvocationList();

                for (int i = 0; i < chain.Length; i++)
                {
                    RemotingClient.ClientCallback cur = (RemotingClient.ClientCallback)chain[i];

                    try
                    {
                        cur(_message);
                    }
                    catch (Exception)
                    {
                        messageCallback -= cur;
                    }
                }
            }
        }

        public void SendMessage(RemotingClient _client, string _message)
        {
            if (clientCallbackList.ContainsKey(_client))
            {
                clientCallbackList[_client](_message);
            }
        }

        public void RegisterMessageCallback(RemotingClient.ClientCallback callback)
        {
            messageCallback += callback;
        }

        public void RegisterMessageCallback(RemotingClient.ClientCallback callback, RemotingClient client)
        {
            clientCallbackList.Add(client, callback);

            try
            {
                // Notify all registered clients
                foreach (RemotingClient remoteClient in clientCallbackList.Keys)
                {
                    remoteClient.OnServerNotification();
                }
            }
            catch (Exception)
            {
            }
        }

        public void UnregisterMessageCallback(RemotingClient.ClientCallback callback)
        {
            if (messageCallback != null)
            {
                messageCallback -= callback;
            }
        }

        public void UnregisterMessageCallback(RemotingClient client)
        {
            if (clientCallbackList.ContainsKey(client))
            {
                clientCallbackList[client] = null;

                clientCallbackList.Remove(client);

                try
                {
                    // Notify all registered clients
                    foreach (RemotingClient remoteClient in clientCallbackList.Keys)
                    {
                        remoteClient.OnServerNotification();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public Dictionary<RemotingClient, RemotingClient.ClientCallback> ClientCallbackList
        {
            get
            {
                return clientCallbackList;
            }
        }

        #endregion
    }
}
