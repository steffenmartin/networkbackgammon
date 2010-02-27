using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemotingBiDirectionalSpike
{
    public interface IRemotingServer
    {
        void SendMessage(string _message);

        void RegisterMessageCallback(RemotingClient.ClientCallback callback);
        void UnregisterMessageCallback(RemotingClient.ClientCallback callback);
    }
}
