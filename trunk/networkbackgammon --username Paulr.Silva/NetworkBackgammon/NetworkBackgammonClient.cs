using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammon
{
    class NetworkBackgammonClient
    {
        static NetworkBackgammonClient instance=null;
        static readonly object padlock = new object();
        NetworkBackgammonRemoteClient client = new NetworkBackgammonRemoteClient();

        NetworkBackgammonClient()
        {
        }

        public static NetworkBackgammonClient Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new NetworkBackgammonClient();
                    }
                    return instance;
                }
            }
        }

        public NetworkBackgammonRemoteClient Client
        {
            get
            {
                return client;
            }
        }
    }
}
