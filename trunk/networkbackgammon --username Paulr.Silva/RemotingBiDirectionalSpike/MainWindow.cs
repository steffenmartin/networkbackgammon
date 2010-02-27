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
        HttpChannel channel = null;

        private IRemotingServer server = null;
        private IRemotingClient client = null;

        public static MainWindow mainWindow = null;

        public MainWindow()
        {
            InitializeComponent();

            mainWindow = this;
        }

        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            try
            {
                if (buttonStartServer.Text == "Start Server")
                {
                    if (channel == null)
                    {
                        /*
                        BinaryServerFormatterSinkProvider sinkProviderBin = new BinaryServerFormatterSinkProvider();
                        sinkProviderBin.TypeFilterLevel = TypeFilterLevel.Full;
                        BinaryClientFormatterSinkProvider sinkProviderBinClient = new BinaryClientFormatterSinkProvider();
                        HttpChannel channel = new HttpChannel(dictChannelProperties, sinkProviderBinClient, sinkProviderTrebuchet);
                        */

                        /*
                        BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
                        serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

                        BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();
                        */

                        SoapServerFormatterSinkProvider serverProv = new SoapServerFormatterSinkProvider();
                        serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                        SoapClientFormatterSinkProvider clientProv = new SoapClientFormatterSinkProvider();

                        IDictionary props = new Hashtable();
                        props["port"] = Convert.ToInt32(textBoxServerPort.Text);

                        // HttpChannel channel = new HttpChannel(Convert.ToInt32(textBoxServerPort.Text));
                        channel = new HttpChannel(props, clientProv, serverProv);
                    }

                    ChannelServices.RegisterChannel(channel);
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
                    // See http://www.codeproject.com/KB/IP/TwoWayRemoting.aspx

                    // Creates a client object that 'lives' here on the client.
                    client = new RemotingClient();

                    // Hook into the event exposed on the Sink
                    // object so we can transfer a server 
                    // message through to this class.
                    client.clientCallback += new RemotingClient.ClientCallback(OnMessageReceived);

                    if (channel == null)
                    {
                        // Register a client channel so the server
                        // can communicate back - it needs a channel
                        // opened for the callback to the CallbackSink
                        // object that is anchored on the client!
                        channel = new HttpChannel(9001);
                    }

                    ChannelServices.RegisterChannel(channel, false);

                    // Now create a transparent proxy to the server component
                    MarshalByRefObject obj = (MarshalByRefObject)RemotingServices.Connect(typeof(IRemotingServer), "http://" + textBoxServerIPAddress.Text + ":" + textBoxServerPortConnect.Text + "/Server");
                    server = obj as IRemotingServer;

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
                // Invoke(new OnMessageReceivedDelegate(OnMessageReceived), _message);
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
                // Invoke(new OnMessageReceivedDelegate(OnMessageReceived), _message);
                BeginInvoke(new OnMessageReceivedDelegate(OnMessageReceivedOnServer), _message);
            }
            else
            {
                listBoxLog.Items.Add("Client -> Server: " + _message);
            }
        }
    }
}
