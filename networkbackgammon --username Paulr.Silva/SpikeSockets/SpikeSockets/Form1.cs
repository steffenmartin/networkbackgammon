using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

// Use Sockets to make simple chat window
// I have both server and client in one class here,
// only difference is which button (Start Server or Send)
// is hit first (don't hit the other!)
// 1st Step: Entering text, it will appear on the server side window - done
// 2nd Step: 2 way between server and client

// Need to run 2 instances of this for it to work.  Use 127.0.0.1 for local test.
namespace SpikeSockets
{
    public partial class MyIRC : Form
    {
        public string remoteIP = null;
        public string remoteAddress = null;
        private Thread receiveThread = null;
        public string[] localChat;
        public string[] remoteChat;
        public bool canReceive;
        public int localCount;
        public int remoteCount;
        public IPAddress myIPAddress;
        public int port;
        public Socket mySocket;
        public bool isConnected;

        public MyIRC()
        {
            InitializeComponent();
            localCount = 0;
            remoteCount = 0;
            port = 3092;
            isConnected = false;
            remoteChat = new string[100];
            localChat = new string[100];
            canReceive = false;

            StartListen();
        }

        // This starts listen thread for server part
        private void StartListen()
        {
            receiveThread = new Thread(new ThreadStart(listenForClient));
            receiveThread.Start();
        }

        private void listenForClient()
        {
            byte[] myByteLine = new byte[100];
            string myLine;
            while (true)
            {
                if (canReceive)
                {
                    Encoding ascii = Encoding.ASCII;
                    int count = mySocket.Receive(myByteLine);
                    myLine = ascii.GetString(myByteLine);
                    remoteChat[remoteCount++] = myLine;
                    textBoxRemoteChat.Text = "\n" + myLine;
                }
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (textBoxRemoteAddress.Text.ToString().Length > 0)
            {
                remoteAddress = textBoxRemoteAddress.Text;
            }

            if (textBoxIPAddress.Text.ToString().Length > 0)
            {
                remoteIP = textBoxIPAddress.Text;
            }
            try
            {
                if (remoteAddress != null && remoteAddress.Length > 0)
                {
                    // Open connection to TCP/IP address
                    IPAddress[] IPs = Dns.GetHostAddresses(remoteAddress);
                    myIPAddress = IPs[0];
                }
                else if (remoteIP != null && remoteIP.Length > 0)
                {
                    // Convert string to IP
                    myIPAddress = IPAddress.Parse(remoteIP);
                }
                mySocket = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream, ProtocolType.Tcp);
                mySocket.Connect(myIPAddress, port);
                isConnected = true;
            }
            catch (ArgumentException exp)
            {
                MessageBox.Show("Bad Network Address " + exp.ToString());
            }
            catch (SocketException exp)
            {
                MessageBox.Show("Network connection error " + exp.ToString());
            }
            catch (Exception exp)
            {
                MessageBox.Show("Some other error");
            }
        }

        private void textBoxLocalChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Enter line into chat buffer
                Encoding ascii = Encoding.ASCII;
                string myLine = textBoxLocalChat.Text;
                Byte[] myByteLine = ascii.GetBytes(myLine);
                localChat[localCount] = myLine;
                localCount++;
                mySocket.Send(myByteLine);
            }
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            // create the socket
            Socket listenSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

            // bind the listening socket to the port
            myIPAddress = (Dns.Resolve(IPAddress.Any.ToString())).AddressList[0];
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
            listenSocket.Bind(ep);

            // start listening
            int backlog = 1;
            listenSocket.Listen(backlog);

            //Wait for chat receive
            mySocket = listenSocket.Accept();

            // Using the RemoteEndPoint property.
            string msgRemote = "I am connected to "
                            + IPAddress.Parse(((IPEndPoint)mySocket.RemoteEndPoint).Address.ToString())
                            + "on port number " + ((IPEndPoint)mySocket.RemoteEndPoint).Port.ToString();
            string msgLocal = "My local IpAddress is :"
                        + IPAddress.Parse(((IPEndPoint)mySocket.LocalEndPoint).Address.ToString())
                        + "I am connected on port number " + ((IPEndPoint)mySocket.LocalEndPoint).Port.ToString();
            MessageBox.Show(msgRemote + "\n" + msgLocal);
            canReceive = true;
        }

        private void MyIRC_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (receiveThread.IsAlive)
                receiveThread.Abort();
        }
    }
}
