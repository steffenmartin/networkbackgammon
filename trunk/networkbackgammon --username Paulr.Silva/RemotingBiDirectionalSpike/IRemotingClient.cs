using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace RemotingBiDirectionalSpike
{
    public interface IRemotingClient
    {
        event RemotingClient.ClientCallback clientCallback;

        [OneWay]
        void SendMessage(string _message);
    }
}
