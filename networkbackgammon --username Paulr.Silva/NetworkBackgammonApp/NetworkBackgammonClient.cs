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
using NetworkBackgammonLib;

namespace NetworkBackgammon
{
    class NetworkBackgammonClient
    {
        static NetworkBackgammonClient instance=null;
        // Lock to make the singleton thread sage
        static readonly object padlock = new object();
        // Remotable client object that will receive 
        NetworkBackgammonPlayer player = null;
        // Remote game room
        NetworkBackgammonRemoteGameRoom gameRoom = null;
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
        public NetworkBackgammonPlayer Player
        {
            get
            {
                return player;
            }

            set
            {
                player = value;
            }
        }

        // Remotable server object
        public NetworkBackgammonRemoteGameRoom GameRoom
        {
            get
            {
                return gameRoom;
            }
        }

        // Remotable server object
        public bool IsConnected
        {
            get
            {
                return (gameRoom == null ? false : true);
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

        // Send a message to the listeners of Player
        public void SendMsg(string msg)
        {
            if( IsConnected )
            {
                NetworkBackgammonPlayer recpPlayer = GameRoom.GetOpposingPlayer(Player);

                if( recpPlayer != null )
                {
                    Player.Broadcast( new NetworkBackgammonChatEvent(Player.PlayerName, recpPlayer.PlayerName, msg) );
                }
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
                    props["machineName"] = System.Environment.MachineName;

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
                MarshalByRefObject obj = (MarshalByRefObject)RemotingServices.Connect(typeof(NetworkBackgammonRemoteGameRoom), "http://" + ipAddr + ":" + port + "/GameRoom");
                gameRoom = obj as NetworkBackgammonRemoteGameRoom;

                serverIpAddress = ipAddr;
                serverPort = port;

                retval = gameRoom.VerifyConnection("ack") == "nack";
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
            if (IsConnected && (Player != null))
            {
                // Remove the player from the game room
                gameRoom.Leave(Player);
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

            gameRoom = null;
            Player = null;
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
