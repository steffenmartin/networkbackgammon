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

namespace RemotingBiDirectionalSpike
{
    public partial class MainWindow : Form
    {
        private HttpChannel channel = null;
        private RemotingServer server = null;
        private RemotingClient client = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            try
            {
                if (buttonStartServer.Text == "Start Server")
                {
                    if (channel == null)
                    {
                        SoapServerFormatterSinkProvider serverProv = new SoapServerFormatterSinkProvider();
                        serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                        SoapClientFormatterSinkProvider clientProv = new SoapClientFormatterSinkProvider();

                        IDictionary props = new Hashtable();
                        props["port"] = Convert.ToInt32(textBoxServerPort.Text);

                        // HttpChannel channel = new HttpChannel(Convert.ToInt32(textBoxServerPort.Text));
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

                    // Hook into the event exposed on the Sink object so we can transfer a server 
                    // message through to this class.
                    client.clientCallback += new RemotingClient.ClientCallback(OnMessageReceived);

                    if (channel == null)
                    {
                        int clientChannel = 9001;
                        bool searchForChannel = true;

                        while (searchForChannel)
                        {
                            try
                            {
                                // Register a client channel so the server an communicate back - it needs a channel
                                // opened for the callback to the CallbackSink object that is anchored on the client!
                                channel = new HttpChannel(clientChannel++);

                                searchForChannel = false;
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }
                    }

                    // Registers a channel with the channel services
                    ChannelServices.RegisterChannel(channel, false);

                    // Now create a transparent proxy to the server component
                    MarshalByRefObject obj = (MarshalByRefObject)RemotingServices.Connect(typeof(RemotingServer), "http://" + textBoxServerIPAddress.Text + ":" + textBoxServerPortConnect.Text + "/Server");
                    server = obj as RemotingServer;

                    // Register callback
                    server.RegisterMessageCallback(new RemotingClient.ClientCallback(client.SendMessage));

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
                        server.UnregisterMessageCallback(new RemotingClient.ClientCallback(client.SendMessage));

                        server = null;
                    }

                    if (channel != null)
                    {
                        ChannelServices.UnregisterChannel(channel);

                        channel = null;
                    }

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
                    server.SendMessage(textBoxMessage.Text);
                }
            }
            catch (Exception ex)
            {
                listBoxLog.Items.Add(ex.Message);
            }
        }

        private delegate void OnMessageReceivedDelegate(string _message);

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

        internal void OnMessageReceivedOnServer(string _message)
        {
            if (InvokeRequired)
            {
                // In case the caller has called this routine on a different thread
                BeginInvoke(new OnMessageReceivedDelegate(OnMessageReceivedOnServer), _message);
            }
            else
            {
                listBoxLog.Items.Add("Client -> Server: " + _message);
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
