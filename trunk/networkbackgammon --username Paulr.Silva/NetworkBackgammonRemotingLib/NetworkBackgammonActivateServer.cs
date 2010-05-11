using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Net;
using System.Net.Sockets;

using NetworkBackgammonLib;
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammonRemotingLib
{
    public class NetworkBackgammonActivateServer
    {
        // Channal the server is connected to
        private HttpChannel m_channel = null;
        // Local server object
        private NetworkBackgammonRemoteGameRoom gameRoom = null;
        /**
        *   Activate/Deactivate server
        */
        
        public NetworkBackgammonRemoteGameRoom ActivateServer(string port)
        {

            if (m_channel == null)
            {
                // We need to use binary formatters, which allow the serialization of generic collections
                BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
                serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

                IDictionary props = new Hashtable();
                props["port"] = Convert.ToInt32(port);
                props["name"] = String.Empty;

                m_channel = new HttpChannel(props, clientProv, serverProv);
            }
            ChannelServices.RegisterChannel(m_channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType( typeof(NetworkBackgammonRemoteGameRoom), "GameRoom", WellKnownObjectMode.Singleton);

            // Assign the instantiated remote server to the local server
            MarshalByRefObject obj = (MarshalByRefObject)RemotingServices.Connect(typeof(NetworkBackgammonRemoteGameRoom), "http://127.0.0.1:" + port + "/GameRoom");
            gameRoom = obj as NetworkBackgammonRemoteGameRoom;

            return gameRoom;
        }
    }
}