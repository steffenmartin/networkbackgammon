using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemotingBiDirectionalSpike
{
    public class RemotingServer : MarshalByRefObject, IRemotingServer
    {
        #region IRemotingServer Members

        public event RemotingClient.ClientCallback messageCallback;

        public void SendMessage(string _message)
        {
            messageCallback(_message);

            if (MainWindow.mainWindow != null)
            {
                MainWindow.mainWindow.OnMessageReceivedOnServer(_message);
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
