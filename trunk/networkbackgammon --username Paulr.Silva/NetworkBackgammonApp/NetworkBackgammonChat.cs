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
        delegate void OnSendMessage();

        public NetworkBackgammonChat()
        {
            InitializeComponent();

            textMsgSendBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
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

        // Check if the enter key was pressed in the text box
        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)              
        {
            if (e.KeyChar == (char)13)
            {
                if (sender == textMsgSendBox)
                {
                    // Then Enter key was pressed...send message
                    BeginInvoke( new OnSendMessage(SendMessage));
                }
            }
        }

        // Handler for receiving message and displaying new message
        private void RecvMessage(string sender, string msg)
        {
            msgTextBox.AppendText(("\r\n" + sender + ": " + msg));
            msgTextBox.ScrollToCaret();
            msgTextBox.Refresh();
        }

        // Send a message out...
        private void SendMessage()
        {
            string sendString = String.Copy(textMsgSendBox.Text);

            if (!String.IsNullOrEmpty(sendString))
            {
                msgTextBox.AppendText(("\r\n" + NetworkBackgammonClient.Instance.Player.PlayerName + ": " + sendString));
                msgTextBox.ScrollToCaret();
                msgTextBox.Refresh();

                // Send out them message...
                NetworkBackgammonClient.Instance.SendMsg(sendString);
            }

            // Set the curor back to the beginning
            textMsgSendBox.SelectionStart = 0;
            textMsgSendBox.SelectionLength = 0;
            textMsgSendBox.Refresh();
            textMsgSendBox.Text = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void NetworkBackgammonChat_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                // Become a listener of the player 
                if (NetworkBackgammonClient.Instance.Player != null)
                {
                    NetworkBackgammonClient.Instance.Player.AddListener(this);
                }
            }
            else
            {
                // Remove self as a listener of player
                if (NetworkBackgammonClient.Instance.Player != null)
                {
                    NetworkBackgammonClient.Instance.Player.RemoveListener(this);
                }
            }
        }

        private void NetworkBackgammonChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Remove self as a listener of player
            if (NetworkBackgammonClient.Instance.Player != null)
            {
                NetworkBackgammonClient.Instance.Player.RemoveListener(this);
            }
        }
    }
}
