using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Collections;

namespace MultipleChannelsSpike
{
    public partial class MainWindow : Form
    {
        private int channel1Port = 1024;
        private HttpChannel channel1;
        private int channel2Port = 1025;
        private HttpChannel channel2;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonChannel1_Click(object sender, EventArgs e)
        {
            try
            {
                BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
                serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

                IDictionary props = new Hashtable();
                props["port"] = channel1Port;
                props["machineName"] = System.Environment.MachineName;
                props["name"] = textBoxChannel1Name.Text;

                // Register a client channel so the server an communicate back - it needs a channel
                // opened for the callback to the CallbackSink object that is anchored on the client!
                // channel = new HttpChannel(clientChannel++);
                channel1 = new HttpChannel(props, clientProv, serverProv);

                ChannelServices.RegisterChannel(channel1, false);

                richTextBoxLogging.Text += "\nChannel with name '" + channel1.ChannelName + "' successfully registered";
            }
            catch (Exception ex)
            {
                richTextBoxLogging.Text += "\n" + ex.Message;
            }
        }

        private void buttonChannel2_Click(object sender, EventArgs e)
        {
            try
            {
                BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
                serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

                IDictionary props = new Hashtable();
                props["port"] = channel2Port;
                props["machineName"] = System.Environment.MachineName;
                props["name"] = textBoxChannel2Name.Text;

                // Register a client channel so the server an communicate back - it needs a channel
                // opened for the callback to the CallbackSink object that is anchored on the client!
                // channel = new HttpChannel(clientChannel++);
                channel2 = new HttpChannel(props, clientProv, serverProv);

                ChannelServices.RegisterChannel(channel2, false);

                richTextBoxLogging.Text += "\nChannel with name '" + channel2.ChannelName + "' successfully registered";
            }
            catch (Exception ex)
            {
                richTextBoxLogging.Text += "\n" + ex.Message;
            }
        }
    }
}
