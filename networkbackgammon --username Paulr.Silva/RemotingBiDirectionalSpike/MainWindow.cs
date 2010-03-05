using System;
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

namespace RemotingBiDirectionalSpike
{
    public partial class MainWindow : Form
    {
        private HttpChannel channel = null;
        private RemotingServer server = null;
        private RemotingClient client = null;
        private RemotingClient.ClientCallback clientCallback = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region GUI Control Event Handler

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            try
            {
                if (buttonStartServer.Text == "Start Server")
                {
                    if (channel == null)
                    {
                        // We need to use binary formatters, which allow the serialization of generic collections
                        BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
                        serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                        BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

                        IDictionary props = new Hashtable();
                        props["port"] = Convert.ToInt32(textBoxServerPort.Text);

                        channel = new HttpChannel(props, clientProv, serverProv);
                    }

                    ChannelServices.RegisterChannel(channel, false);
                    RemotingConfiguration.RegisterWellKnownServiceType(Type.GetType("RemotingBiDirectionalSpike.RemotingServer"), "Server", WellKnownObjectMode.Singleton);

                    buttonStartServer.Text = "Stop Server";
                    textBoxServerPort.Enabled = false;

                    foreach (Control control in tabPageClient.Controls)
                    {
                        control.Enabled = false;
                    }
                }
                else
                {
                    if (channel != null)
                    {
                        ChannelServices.UnregisterChannel(channel);

                        channel = null;
                    }

                    buttonStartServer.Text = "Start Server";
                    textBoxServerPort.Enabled = true;

                    foreach (Control control in tabPageClient.Controls)
                    {
                        control.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                listBoxLog.Items.Add(ex.Message);
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (buttonConnect.Text == "Connect")
                {
                    // Creates a client object that 'lives' here on the client.
                    client = new RemotingClient();

                    listBoxLog.Items.Add("Created client with ID " + client.ClientID);

                    // Hook into the event exposed on the Sink object so we can transfer a server 
                    // message through to this class.
                    client.clientCallback += OnMessageReceived;

                    // Hook into the event to allow the server to send (other) notifications
                    // (Currently only asserted when clients connect or disconnect from server by
                    //  means of registering respective event handlers associated with their client
                    //  references)
                    client.serverNotificationCallback += OnServerNotification;

                    if (channel == null)
                    {
                        int clientChannel = FindUnusedPort(IPAddress.Loopback);

                        listBoxLog.Items.Add("Using (unused) client port " + clientChannel);

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
                    }

                    // Registers a channel with the channel services
                    ChannelServices.RegisterChannel(channel, false);

                    // Now create a transparent proxy to the server component
                    MarshalByRefObject obj = (MarshalByRefObject)RemotingServices.Connect(typeof(RemotingServer), "http://" + textBoxServerIPAddress.Text + ":" + textBoxServerPortConnect.Text + "/Server");
                    server = obj as RemotingServer;

                    // Create messaging callback objects
                    clientCallback = new RemotingClient.ClientCallback(client.SendMessage);

                    // Register callback
                    if (clientCallback != null)
                    {
                        // Simple messaging callback for broadcasting of messages from one client
                        server.RegisterMessageCallback(clientCallback);
                        // Messaging callback associated with the client to allow for sending messages to a particular client
                        server.RegisterMessageCallback(clientCallback, client);
                    }

                    buttonConnect.Text = "Disconnect";
                    textBoxServerIPAddress.Enabled = false;
                    textBoxServerPortConnect.Enabled = false;

                    foreach (Control control in tabPageServer.Controls)
                    {
                        control.Enabled = false;
                    }
                }
                else
                {
                    if (server != null)
                    {
                        if (clientCallback != null)
                        {
                            server.UnregisterMessageCallback(clientCallback);
                        }

                        if (client != null)
                        {
                            server.UnregisterMessageCallback(client);
                        }
                    }

                    if (client != null)
                    {
                        client.clientCallback -= OnMessageReceived;
                        client.serverNotificationCallback -= OnServerNotification;
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

                    clientCallback = null;
                    server = null;
                    client = null;
                    channel = null;

                    OnServerNotification();

                    buttonConnect.Text = "Connect";
                    textBoxServerIPAddress.Enabled = true;
                    textBoxServerPortConnect.Enabled = true;

                    foreach (Control control in tabPageServer.Controls)
                    {
                        control.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                listBoxLog.Items.Add(ex.Message);
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (server != null)
                {
                    // If client (item) has been selected, send respective message to it ...
                    if (listBoxConnectedClients.SelectedItem != null)
                    {
                        server.SendMessage((RemotingClient)listBoxConnectedClients.SelectedItem, textBoxMessage.Text);
                    }
                    // ... otherwise send broadcast message (to all currently connected clients)
                    else
                    {
                        server.SendMessage(textBoxMessage.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                listBoxLog.Items.Add(ex.Message);
            }
        }

        #endregion

        #region Delegates and respective handlers

        private delegate void OnMessageReceivedDelegate(string _message);
        private delegate void OnServerNotificationDelegate();

        public void OnServerNotification()
        {
            if (InvokeRequired)
            {
                // In case the caller has called this routine on a different thread
                BeginInvoke(new OnServerNotificationDelegate(OnServerNotification));
            }
            else
            {
                try
                {
                    listBoxConnectedClients.Items.Clear();

                    if (server != null)
                    {
                        foreach (RemotingClient remoteClient in server.ClientCallbackList.Keys)
                        {
                            if (remoteClient != client)
                            {
                                listBoxConnectedClients.Items.Add(remoteClient);
                            }
                        }
                    }
                    else
                    {
                        listBoxConnectedClients.Items.Clear();
                    }
                }
                catch (Exception ex)
                {
                    listBoxLog.Items.Add(ex.Message);
                }
            }
        }
        public void OnMessageReceived(string _message)
        {
            if (InvokeRequired)
            {
                // In case the caller has called this routine on a different thread
                BeginInvoke(new OnMessageReceivedDelegate(OnMessageReceived), _message);
            }
            else
            {
                listBoxLog.Items.Add("Server -> Client: " + _message);
            }
        }

        #endregion

        #region Misc members

        public int FindUnusedPort(IPAddress localAddr)
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
                    // EADDRINUSE?
                    // if (ex.ErrorCode == 10048)
                        continue;
                    // else
                        // throw;
                }
            }

            return retVal;
        }

        #endregion
    }
}
