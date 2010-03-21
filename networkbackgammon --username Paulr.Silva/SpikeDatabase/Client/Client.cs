using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpikeDatabase
{
    public partial class Client : Form
    {
        Server myServer;

        public Client()
        {
            InitializeComponent();

            // Make new server object
            myServer = new Server();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            // Get server to login user
            if (!myServer.LoginUser(userNameTextBox.Text, passwordTextBox.Text))
            {
                MessageBox.Show("Error: " + myServer.GetError());
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            // Get server to register user
            if (!myServer.UserRegister(userNameTextBox.Text, passwordTextBox.Text))
            {
                MessageBox.Show("Error: " + myServer.GetError());
            }
        }

        private void listButton_Click(object sender, EventArgs e)
        {
            // Get list of users
            List<String> thePlayers = myServer.GetUsers();
            playerListBox.Items.Clear();
            foreach (String player in thePlayers)
            {
                playerListBox.Items.Add(player);
            }
        }

    }
}
