using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;

namespace RemotingBiDirectionalSpike
{
    public class RemotingClient : MarshalByRefObject
    {
        public delegate void ClientCallback(string _message);

        public event ClientCallback clientCallback;

        #region RemotingClient Members

        public void SendMessage(string _message)
        {
            if (clientCallback != null)
                clientCallback(_message);
        }

        #endregion
    }
}
