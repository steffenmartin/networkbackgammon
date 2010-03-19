using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetworkBackgammonLib;
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammon
{
    public partial class NetworkBackgammonChat : Form, INetworkBackgammonListener
    {
        INetworkBackgammonListener m_defaultListener = new NetworkBackgammonListener();
        delegate void OnRecvMessage(string sender, string msg);

        public NetworkBackgammonChat()
        {
            InitializeComponent();
        }

        #region INetworkBackgammonListener Members

        public bool AddNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_defaultListener.AddNotifier(notifier);
        }

        public bool RemoveNotifier(INetworkBackgammonNotifier notifier)
        {
            return m_defaultListener.RemoveNotifier(notifier);
        }

        public void OnEventNotification(INetworkBackgammonNotifier notifier, INetworkBackgammonEvent e)
        {
            if (e is NetworkBackgammonChatEvent)
            {
                NetworkBackgammonChatEvent chatEvent = (NetworkBackgammonChatEvent)e;

                string sender = chatEvent.Sender;
                string recv = chatEvent.Recipient;
                string msg = chatEvent.Message;

                // Check if the message if for this player
                if (NetworkBackgammonClient.Instance.Player.PlayerName.CompareTo(recv) == 0)
                {
                    if (InvokeRequired)
                    {
                        BeginInvoke(new OnRecvMessage(RecvMessage), sender, msg);
                    }
                    else
                    {
                        RecvMessage(sender, msg);
                    }
                }
            }
        }
            
        #endregion

        private void RecvMessage(string sender, string msg)
        {
            msgTextBox.Text += ("\n" + sender + ": " + msg);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetworkBackgammonClient.Instance.SendMsg(textMsgSendBox.Text);
        }
    }
}
