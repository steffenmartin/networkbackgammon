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

        public void SendMessage(string _message)
        {
            //messageCallback(_message);

            Delegate[] chain = messageCallback.GetInvocationList();

            for (int i = 0; i < chain.Length; i++)
            {
                RemotingClient.ClientCallback cur = (RemotingClient.ClientCallback)chain[i];

                try
                {
                    cur(_message);
                }
                catch (Exception ex)
                {
                    messageCallback -= cur;
                }
            }
        }

        public void RegisterMessageCallback(RemotingClient.ClientCallback callback)
        {
            messageCallback += callback;
        }

        public void UnregisterMessageCallback(RemotingClient.ClientCallback callback)
        {
            messageCallback -= callback;
        }

        #endregion
    }
}
