using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammon
{
    public partial class NetworkBackgammonLoginForm : Form
    {
        public NetworkBackgammonLoginForm()
        {
            InitializeComponent();

            // Initialize the server property information with default data
            m_ipAddrTextBox.Text = NetworkBackgammonClient.Instance.ServerIpAddress;
            m_portTextBox.Text = NetworkBackgammonClient.Instance.ServerPort;

            // Disable/enable controls for the login group based on whether or not connected to the server
            foreach (Control control in m_loginGroup.Controls)
            {
                control.Enabled = NetworkBackgammonClient.Instance.IsConnected;
            }
        }

        // Registration button handler
        private void m_registerRegister_Click(object sender, EventArgs e)
        {
            if (NetworkBackgammonClient.Instance.IsConnected)
            {
                if (!NetworkBackgammonClient.Instance.Server.RegisterUser(m_usernameTextBox.Text, m_passwordTextBox.Text))
                {
                    MessageBox.Show("Could not register: " + m_usernameTextBox.Text);
                }
            }
        }

        // Login Button handler
        private void m_loginButton_Click(object sender, EventArgs e)
        {
            if (NetworkBackgammonClient.Instance.IsConnected)
            {
                if (!NetworkBackgammonClient.Instance.Server.Login(m_usernameTextBox.Text, m_passwordTextBox.Text))
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

        // Connect to the server button handler
        private void m_connectButton_Click(object sender, EventArgs e)
        {
            if (NetworkBackgammonClient.Instance.IsConnected)
            {
                NetworkBackgammonClient.Instance.DisconnectServer();
            }
            else
            {
                NetworkBackgammonClient.Instance.ConnectServer(m_ipAddrTextBox.Text, m_portTextBox.Text);
            }

            bool connected = NetworkBackgammonClient.Instance.IsConnected;

            m_connectButton.Text = (connected ? "Disconnect" : "Connect");
            m_ipAddrTextBox.Enabled = !connected;
            m_portTextBox.Enabled = !connected;

            // Enable all controls in the login group after a successful login
            foreach (Control control in m_loginGroup.Controls)
            {
                control.Enabled = connected;
            }
        }
    }
}
