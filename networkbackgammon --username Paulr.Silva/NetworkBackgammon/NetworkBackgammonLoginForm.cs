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

using NetworkBackgammonRemotingLib;

namespace NetworkBackgammon
{
    public partial class NetworkBackgammonLoginForm : Form
    {
        private HttpChannel channel = null;
        private NetworkBackgammonRemoteServer m_server = null;
      
        public NetworkBackgammonLoginForm()
        {
            InitializeComponent();

            m_maskedIPAddressTextBox.Text = "127.000.000.001";
            m_portTextBox.Text = "8080";

            foreach (Control control in m_loginGroup.Controls)
            {
                control.Enabled = (m_server == null ? false : true);
            }
        }

        private void m_registerRegister_Click(object sender, EventArgs e)
        {
            if (m_server != null)
            {
                if (!m_server.RegisterUser(m_usernameTextBox.Text, m_passwordTextBox.Text))
                {
                    MessageBox.Show("Could not register: " + m_usernameTextBox.Text);
                }
            }
        }

        private void m_loginButton_Click(object sender, EventArgs e)
        {
            if (m_server != null)
            {
                if (!m_server.Login(m_usernameTextBox.Text, m_passwordTextBox.Text))
                {
                    MessageBox.Show("Could not login: " + m_usernameTextBox.Text);
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void m_connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_connectButton.Text == "Connect")
                {
                   
                    if (channel == null)
                    {
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
                    }

                    // Registers a channel with the channel services
                    ChannelServices.RegisterChannel(channel, false);

                    // Now create a transparent proxy to the server component
                    MarshalByRefObject obj = (MarshalByRefObject)RemotingServices.Connect(typeof(NetworkBackgammonRemoteServer), "http://" + m_maskedIPAddressTextBox.Text + ":" + m_portTextBox.Text + "/Server");
                    m_server = obj as NetworkBackgammonRemoteServer;


                    m_connectButton.Text = m_server.m_test;// "Disconnect";
                    m_maskedIPAddressTextBox.Enabled = false;
                    m_portTextBox.Enabled = false;

                    // Enable all controls in the login group after a successful login
                    foreach (Control control in m_loginGroup.Controls)
                    {
                        control.Enabled = true;
                    }
                }
                else
                {
                   
                    if (channel != null)
                    {
                        ChannelDataStore cds = (ChannelDataStore)channel.ChannelData;

                        foreach (string s in cds.ChannelUris)
                        {
                            channel.StopListening(s);
                        }

                        ChannelServices.UnregisterChannel(channel);
                    }

                    m_server = null;
                    channel = null;

                    m_connectButton.Text = "Connect";
                    m_maskedIPAddressTextBox.Enabled = true;
                    m_portTextBox.Enabled = true;

                    foreach (Control control in m_loginGroup.Controls)
                    {
                        control.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
                    continue;
                }
            }

            return retVal;
        }

        #endregion
    }
}
