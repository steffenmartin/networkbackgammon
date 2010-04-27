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

        /// <summary>
        /// Delegate for handling pip updates
        /// </summary>
        delegate void OnUpdatePipCountDelegate(Int32 pipCount);

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
            if (e is GameSessionCheckerUpdatedEvent)
            {
                GameSessionCheckerUpdatedEvent updateEvent = (GameSessionCheckerUpdatedEvent)e;

                NetworkBackgammonPlayer curPlayer = updateEvent.GetPlayerByName(Title);

                if (curPlayer != null)
                {
                    // Update the pip count
                    if (InvokeRequired)
                    {
                        BeginInvoke(new OnUpdatePipCountDelegate(OnUpdatePipCount), curPlayer.PipCount);
                    }
                    else
                    {
                        OnUpdatePipCount(curPlayer.PipCount);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Handler for a checker/pip update
        /// </summary>
        private void OnUpdatePipCount(Int32 pipCount)
        {
            this.player1Pips.Text = pipCount.ToString();
        }


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

        private void groupPlayerScoreBoard_VisibleChanged(object sender, EventArgs e)
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

        private void NetworkBackgammonScoreBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Remove self as a listener of player
            if (NetworkBackgammonClient.Instance.Player != null)
            {
                NetworkBackgammonClient.Instance.Player.RemoveListener(this);
            }
        }
    }
}
