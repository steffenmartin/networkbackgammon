using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;

namespace RemotingBiDirectionalSpike
{
    public class RemotingClient : MarshalByRefObject, IRemotingClient
    {
        public delegate void ClientCallback(string _message);

        #region IRemotingClient Members

        public event RemotingClient.ClientCallback clientCallback;

        public void SendMessage(string _message)
        {
            if (clientCallback != null)
                clientCallback(_message);
        }

        #endregion
    }
}
