﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Net;
using System.Net.Sockets;

using NetworkBackgammonLib;
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammonServer
{
    public partial class ServerControlForm : Form, INetworkBackgammonListener
    {
        // Channel the server is connected to
        private HttpChannel m_channel = null;
        // Local server object
        private NetworkBackgammonRemoteGameRoom m_server = null;
        // Local delegated listener object
        INetworkBackgammonListener m_localListener = new NetworkBackgammonListener();
        // Log Message Delegate
        private delegate void OnLogMessageDelegate(string _message);

        public ServerControlForm()
        {
            InitializeComponent();
        }

        /**
        *   Activate/Deactivate server
        */
        private void m_activateServer_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_activateServer.Text == "Start Server")
                {
                    if (m_channel == null)
                    {
                        // We need to use binary formatters, which allow the serialization of generic collections
                        BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
                        serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                        BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

                        IDictionary props = new Hashtable();
                        props["port"] = Convert.ToInt32(m_portText.Text);

                        m_channel = new HttpChannel(props, clientProv, serverProv);
                    }

                    ChannelServices.RegisterChannel(m_channel, false);
                    RemotingConfiguration.RegisterWellKnownServiceType( typeof(NetworkBackgammonRemoteGameRoom), "GameRoom", WellKnownObjectMode.Singleton);

                    // Assign the instantiated remote server to the local server
                    MarshalByRefObject obj = (MarshalByRefObject)RemotingServices.Connect(typeof(NetworkBackgammonRemoteGameRoom), "http://127.0.0.1:" + m_portText.Text + "/GameRoom");
                    m_server = obj as NetworkBackgammonRemoteGameRoom;

                    // Register delegated listener as a listener of the server object
                    m_server.AddListener(this);
            
                    m_activateServer.Text = "Stop Server";
                    m_portText.Enabled = false;
                  
                    // Log status message 
                    Log("Server Started...");
                }
                else
                {
                    // Stop listening to the server
                    m_server.RemoveListener(this);

                    if (m_channel != null)
                    {
                        ChannelServices.UnregisterChannel(m_channel);

                        m_channel = null;
                    }

                    m_activateServer.Text = "Start Server";
                    m_portText.Enabled = true;

                    // Log status message 
                    Log("Server Stopped...");
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        // Log a local message
        private void Log(string message)
        {
            m_serverLogListBox.Items.Add(message);
        }

        #region INetworkBackgammonListener Members

        public bool AddNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_localListener.AddNotifier(notifier);
        }

        public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_localListener.RemoveNotifier(notifier);
        }

        public void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e)
        {
            if (InvokeRequired)
            {
                // In case the caller has called this routine on a different thread
                BeginInvoke(new OnLogMessageDelegate(Log), sender.ToString() + e.ToString());

                BeginInvoke(new OnLogMessageDelegate(Log), ("***** Event ***** "));
                BeginInvoke(new OnLogMessageDelegate(Log), "Sender: " + sender.ToString());
                BeginInvoke(new OnLogMessageDelegate(Log), "Event: " + e.ToString());
            }
            else
            {
                Log("***** Event ***** ");
                Log("Sender: " + sender.ToString());
                Log("Event: " + e.ToString());
            }
        }

        #endregion

    }
}