using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    [Serializable]
    public class NetworkBackgammonChatEvent : INetworkBackgammonEvent
    {
        string m_sender;
        string m_receiver;
        string m_msg;

        public NetworkBackgammonChatEvent(string sender, string receiver, string msg)
        {
            m_sender = sender;
            m_receiver = receiver;
            m_msg = msg;
        }

        public string Message
        {
            get
            {
                return m_msg;
            }
        }

        public string Sender
        {
            get
            {
                return m_sender;
            }
        }

        public string Recipient
        {
            get
            {
                return m_receiver;
            }
        }
    }
}
