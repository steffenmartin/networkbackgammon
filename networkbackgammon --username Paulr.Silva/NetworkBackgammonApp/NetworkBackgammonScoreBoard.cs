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
    public partial class NetworkBackgammonScoreBoard : Form, INetworkBackgammonListener
    {
        INetworkBackgammonListener m_defaultListener = new NetworkBackgammonListener();
    
        public NetworkBackgammonScoreBoard()
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

        public void OnEventNotification(INetworkBackgammonNotifier sender, INetworkBackgammonEvent e)
        {
            // TODO: Handle events
        }

        #endregion

        #region Properties

        // Set the title for the score board
        public string Title
        {
            get
            {
                return groupPlayerScoreBoard.Text;
            }
            set
            {
                groupPlayerScoreBoard.Text = value;
            }
        }

        #endregion

        private void NetworkBackgammonScoreBoard_Load(object sender, EventArgs e)
        {
            
        }
    }
}
