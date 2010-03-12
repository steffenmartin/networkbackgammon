using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Net;
using System.Net.Sockets;
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammon
{
    class NetworkBackgammonClient
    {
        static NetworkBackgammonClient instance=null;
        static readonly object padlock = new object();
        NetworkBackgammonRemoteClient client = new NetworkBackgammonRemoteClient();
        NetworkBackgammonRemoteServer server = null;
        HttpChannel channel = null;
        string serverIpAddress = "127.0.0.1";
        string serverPort = "8080";

        // Private Constructor
        NetworkBackgammonClient()
        {
        }

        // Access to the single instance of this class
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

        // Remotable client object
        public NetworkBackgammonRemoteClient Client
        {
            get
            {
                return client;
            }
        }

        // Remotable server object
        public NetworkBackgammonRemoteServer Server
        {
            get
            {
                return server;
            }
        }

        // Remotable server object
        public bool IsConnected
        {
            get
            {
                return (server == null ? false : true);
            }
        }

        // Server IP address property 
        public string ServerIpAddress
        {
            get
            {
                return serverIpAddress;
            }
        }

        // Server port property 
        public string ServerPort
        {
            get
            {
                return serverPort;
            }
        }

        // Connect to a remote server (remoting) via an ip address and port
        public bool ConnectServer(string ipAddr, string port)
        {
            bool retval = false;

            try
            {
                // First disconenct any previous connection with the server
                DisconnectServer();
                
                // Get an available channel to use
                int clientChannel = FindUnusedPort(IPAddress.Loopback);

                if (clientChannel != 0)
                {
                    BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
                    serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                    BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

                    IDictionary props = new Hashtable();
                    props["port"] = clientChannel;

                    // Register a client channel so the server an communicate back - it needs a channel
                    // opened for the callback to the CallbackSink object that is anchored on the client!
                    // channel = new HttpChannel(clientChannel++);
                    channel = new HttpChannel(props, clientProv, serverProv);
                }
                else
                {
                    throw new Exception("Couldn't find unused client port!");
                }

                // Registers a channel with the channel services
                ChannelServices.RegisterChannel(channel, false);

                // Now create a transparent proxy to the server component
                MarshalByRefObject obj = (MarshalByRefObject)RemotingServices.Connect(typeof(NetworkBackgammonRemoteServer), "http://" + ipAddr + ":" + port + "/Server");
                server = obj as NetworkBackgammonRemoteServer;

                serverIpAddress = ipAddr;
                serverPort = port;

                // Register the remotable client object as a listner 
                retval = server.AddListener(Client);
            }
            catch (Exception ex)
            {
                // Disconnect from the server
                DisconnectServer();
            }

            return retval;
        }

        // Disconnect as from the server and remove client as listener
        public void DisconnectServer()
        {
            if (IsConnected)
            {
                // UnRegister the remotable client object as a listner 
                server.RemoveListener(Client);
            }

            if (channel != null)
            {
                ChannelDataStore cds = (ChannelDataStore)channel.ChannelData;

                foreach (string s in cds.ChannelUris)
                {
                    channel.StopListening(s);
                }

                ChannelServices.UnregisterChannel(channel);
            }

            server = null;
            channel = null;
        }

        // Find the next available port to use
        int FindUnusedPort(IPAddress localAddr)
        {
            int retVal = 0;

            for (int p = 1024; p <= IPEndPoint.MaxPort; p++)
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    s.Bind(new IPEndPoint(localAddr, p));
                    s.Close();

                    retVal = p;

                    break;
                }
                catch (SocketException)
                {
                    continue;
                }
            }

            return retVal;
        }
    }
}
