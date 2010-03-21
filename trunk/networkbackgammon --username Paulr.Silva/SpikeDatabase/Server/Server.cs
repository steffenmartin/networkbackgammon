using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Server.Properties;
using System.IO;
using System.Reflection;
using System.Xml;

namespace SpikeDatabase
{
    public partial class Server : Form
    {
        DataTable userDataTable;
        string lastError = null;
        XmlDocument userListXmlDoc;
        string userListFile;

        public Server()
        {
            InitializeComponent();

            userDataTable = new DataTable("NetworkBackgammon");
            userListFile = "BackgammonUserList.xml";

            InitializeTable();

            userListXmlDoc = new XmlDocument();
        }

        private bool InitializeTable()
        {
            // Create table columns
            DataColumn myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ReadOnly = false;
            myColumn.Caption = "User Name";
            myColumn.ColumnName = "Username";
            userDataTable.Columns.Add(myColumn);

            myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ReadOnly = false;
            myColumn.Caption = "User Password";
            myColumn.ColumnName = "Password";
            userDataTable.Columns.Add(myColumn);

            myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.Boolean");
            myColumn.ReadOnly = false;
            myColumn.Caption = "User Login";
            myColumn.ColumnName = "Login";
            userDataTable.Columns.Add(myColumn);

            //$$ DEBUG
            // Create fake user
            //AddUser("PLombrozo", "mypassword");
            //AddUser("SMartin", "steffenspassword");
            //AddUser("PSilva", "PaulsPassWord");

            userDataGridView.DataSource = userDataTable;
            GetUserList();
            userDataGridView.Show();

            return true;
        }

        public bool UserRegister(string userName, string userPassword)
        {
            // Check if already registered
            if (FindUser(userName) == -1)
            {
                // Add to database
                AddUser(userName, userPassword);
                SaveUserList();
                return true;
            }
            else
            {
                lastError = "User " + userName + " is already registered with NetwordBackgammon";
                return false;
            }
        }

        public bool LoginUser(string loginName, string loginPassword)
        {
            int userIndex = FindUser(loginName);
            if (userIndex > -1)
            {
                userDataTable.Rows[userIndex]["Login"] = true;
                return true;
            }
            else
            {
                lastError = "User " + loginName + " is not registered with NetworkBackgammon";
                return false;
            }
        }

        public bool LogoutUser(string loginName)
        {
            int userIndex = FindUser(loginName);
            if (userIndex > -1)
            {
                userDataTable.Rows[userIndex]["Login"] = false;
                return true;
            }
            else
            {
                lastError = "User " + loginName + " is not registered with NetworkBackgammon";
                return false;
            }
        }

        // Return list of users logged in
        public List<String> GetUsers()
        {
            List<String> myList = new List<String>();

            int rowCount = userDataTable.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                myList.Add(userDataTable.Rows[i]["Username"].ToString());
            }
            return myList;
        }

        private int FindUser(string userName)
        {
            // Find user in database
            // Open database
            DataRow userRow;
            int rowCount = userDataTable.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                userRow = userDataTable.Rows[i];
                if (userName.ToLower().Equals(userRow["Username"].ToString().ToLower()))
                {
                    return i;
                }
            }
            return -1;
        }

        private void AddUser(string newName, string newPassword)
        {
            DataRow userRow = userDataTable.NewRow();
            userRow["Username"] = newName;
            userRow["Password"] = newPassword;
            userRow["Login"] = false;
            userDataTable.Rows.Add(userRow);
        }

        public string GetError()
        {
            return lastError;
        }

        // Save User DataTable to XML file
        private void SaveUserList()
        {
            if (File.Exists(userListFile))
                File.Delete(userListFile);

            XmlDocument userListXmlDoc = new XmlDocument();
            userListXmlDoc.LoadXml("<userListXmlDoc></userListXmlDoc>");
            int rowCount = userDataTable.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                DataRow userRow = userDataTable.Rows[i];
                XmlNode userNode = userListXmlDoc.CreateNode(XmlNodeType.Element, "User", "");
                if (userListXmlDoc.LastChild != null)
                    userListXmlDoc.LastChild.AppendChild(userNode);
                else
                    userListXmlDoc.AppendChild(userNode);

                XmlNode userElement = userListXmlDoc.CreateNode(XmlNodeType.Element, "Username", "");
                userElement.InnerText = userRow["Username"].ToString();
                userNode.AppendChild(userElement);

                userElement = userListXmlDoc.CreateNode(XmlNodeType.Element, "Password", "");
                userElement.InnerText = userRow["Password"].ToString();
                userNode.AppendChild(userElement);

                userElement = userListXmlDoc.CreateNode(XmlNodeType.Element, "Login", "");
                userElement.InnerText = userRow["Login"].ToString();
                userNode.AppendChild(userElement);
            }

            XmlWriterSettings mySettings = new XmlWriterSettings();
            mySettings.Indent = true;
            mySettings.IndentChars = "    ";
            mySettings.OmitXmlDeclaration = true;
            XmlWriter writer = XmlWriter.Create(userListFile, mySettings);
            userListXmlDoc.WriteTo(writer);
            writer.Flush();
            writer.Close();
        }

        // Get UserList from XML file into DataTable
        private void GetUserList()
        {
            try
            {
                if (File.Exists(userListFile))
                {
                    TextReader reader = new StreamReader(userListFile);
                    string xmlData = reader.ReadToEnd();
                    reader.Close();
                    if (xmlData.Length > 0)
                    {
                        XmlDocument userListXmlDoc = new XmlDocument();
                        userListXmlDoc.LoadXml(xmlData);
                        XmlNodeList userNodes = userListXmlDoc.SelectNodes("userListXmlDoc/User");

                        foreach (XmlNode userRowNode in userNodes)
                        {
                            DataRow userRow = userDataTable.NewRow();
                            userRow["Username"] = "";
                            userRow["Password"] = "";
                            userRow["Login"] = false;
                            foreach (XmlNode user in userRowNode)
                            {
                                // Put each Bud node into the userListXmlDoc
                                switch (user.Name)
                                {
                                    case "Username":
                                        userRow["Username"] = user.InnerText;
                                        break;
                                    case "Password":
                                        userRow["Password"] = user.InnerText;
                                        break;
                                    case "Login":
                                        userRow["Login"] = user.InnerText.Contains("1"); ;
                                        break;
                                }
                            }
                            userDataTable.Rows.Add(userRow);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error reading User list: " + exp.Message);
            }
        }
    }
}
