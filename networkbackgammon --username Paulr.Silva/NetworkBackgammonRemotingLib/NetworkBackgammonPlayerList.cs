using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace NetworkBackgammonRemotingLib
{
    class NetworkBackgammonPlayerList
    {
        DataTable playerDataTable;
        string lastError = null;
        XmlDocument playerListXmlDoc;
        string playerListFile;

        /// <summary>
        /// Constructor creates data table to hold player information
        /// and initializes table
        /// </summary>
        public NetworkBackgammonPlayerList()
        {
            playerDataTable = new DataTable("NetworkBackgammon");
            playerListFile = "BackgammonPlayerList.xml";

            InitializeTable();
        }

        /// <summary>
        /// Initialize DataTable with Play name/password
        /// Read from XML file
        /// </summary>
        /// <returns>True if no errors</returns>
        private bool InitializeTable()
        {
            // Create table columns
            DataColumn myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ReadOnly = false;
            myColumn.Caption = "Player Name";
            myColumn.ColumnName = "PlayerName";
            playerDataTable.Columns.Add(myColumn);

            myColumn = new DataColumn();
            myColumn.DataType = System.Type.GetType("System.String");
            myColumn.ReadOnly = false;
            myColumn.Caption = "Player Password";
            myColumn.ColumnName = "Password";
            playerDataTable.Columns.Add(myColumn);

            ReadPlayerList();
            return true;
        }

        /// <summary>
        /// Verify registered player
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns>True if registered</returns>
        public bool VerifyPlayer(string playerName)
        {
            return FindPlayer(playerName) > -1;
        }

        /// <summary>
        /// Verify registered player password
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="password"></param>
        /// <returns>True if correct password</returns>
        public bool VerifyLogin(string playerName, string password)
        {
            int playerIndex = FindPlayer(playerName);
            if (playerIndex > -1)
            {
                return playerDataTable.Rows[playerIndex]["Password"].
                                ToString().Equals(password);
            }
            return false;
        }

        /// <summary>
        /// Add player to player list, save updated table to XML file
        /// Sets error string if already in list
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="newPassword"></param>
        /// <returns>True if player not already in list</returns>
        public bool AddPlayer(string newName, string newPassword)
        {
            // Check if already registered
            if (FindPlayer(newName) == -1)
            {
                // Add to database
                DataRow playerRow = playerDataTable.NewRow();
                playerRow["PlayerName"] = newName;
                playerRow["Password"] = newPassword;
                playerDataTable.Rows.Add(playerRow);
                SavePlayerList();
                return true;
            }
            else
            {
                lastError = "Player " + newName + " is already registered with NetwordBackgammon";
                return false;
            }
        }

        /// <summary>
        /// Return last error string
        /// </summary>
        /// <returns></returns>
        public string GetError()
        {
            return lastError;
        }

        /// <summary>
        /// Return list of registered players
        /// </summary>
        /// <returns></returns>
        public ArrayList GetPlayerList()
        {
            int rowCount = playerDataTable.Rows.Count;
            ArrayList thisArray = new ArrayList();
            for (int i = 0; i < rowCount; i++)
            {
                thisArray.Add(playerDataTable.Rows[i]["PlayerName"].ToString());
            }
            return thisArray;
        }

        /// <summary>
        /// Finds player in player list
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns>Index of player in DataTable, -1 if not found</returns>
        private int FindPlayer(string playerName)
        {
            // Find player in database
            // Open database
            DataRow playerRow;
            int rowCount = playerDataTable.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                playerRow = playerDataTable.Rows[i];
                if (playerName.ToLower().Equals(playerRow["PlayerName"].ToString().ToLower()))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Save player list DataTable to XML file 
        /// </summary>
        private void SavePlayerList()
        {
            if (File.Exists(playerListFile))
                File.Delete(playerListFile);

            playerListXmlDoc = new XmlDocument();
            playerListXmlDoc.LoadXml("<playerListXmlDoc></playerListXmlDoc>");
            int rowCount = playerDataTable.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                DataRow playerRow = playerDataTable.Rows[i];
                XmlNode playerNode = playerListXmlDoc.CreateNode(XmlNodeType.Element, "player", "");
                if (playerListXmlDoc.LastChild != null)
                    playerListXmlDoc.LastChild.AppendChild(playerNode);
                else
                    playerListXmlDoc.AppendChild(playerNode);

                XmlNode playerElement = playerListXmlDoc.CreateNode(XmlNodeType.Element, "playername", "");
                playerElement.InnerText = playerRow["playername"].ToString();
                playerNode.AppendChild(playerElement);

                playerElement = playerListXmlDoc.CreateNode(XmlNodeType.Element, "password", "");
                playerElement.InnerText = playerRow["Password"].ToString();
                playerNode.AppendChild(playerElement);
            }

            XmlWriterSettings mySettings = new XmlWriterSettings();
            mySettings.Indent = true;
            mySettings.IndentChars = "    ";
            mySettings.OmitXmlDeclaration = true;
            XmlWriter writer = XmlWriter.Create(playerListFile, mySettings);
            playerListXmlDoc.WriteTo(writer);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Get playerList from XML file into DataTable 
        /// </summary>
        private void ReadPlayerList()
        {
            try
            {
                if (File.Exists(playerListFile))
                {
                    TextReader reader = new StreamReader(playerListFile);
                    string xmlData = reader.ReadToEnd();
                    reader.Close();
                    if (xmlData.Length > 0)
                    {
                        playerListXmlDoc = new XmlDocument();
                        playerListXmlDoc.LoadXml(xmlData);
                        XmlNodeList playerNodes = playerListXmlDoc.SelectNodes("playerListXmlDoc/player");

                        foreach (XmlNode playerRowNode in playerNodes)
                        {
                            DataRow playerRow = playerDataTable.NewRow();
                            playerRow["PlayerName"] = "";
                            playerRow["Password"] = "";
                            foreach (XmlNode player in playerRowNode)
                            {
                                // Put each Bud node into the playerListXmlDoc
                                switch (player.Name)
                                {
                                    case "playername":
                                        playerRow["PlayerName"] = player.InnerText;
                                        break;
                                    case "password":
                                        playerRow["Password"] = player.InnerText;
                                        break;
                                }
                            }
                            playerDataTable.Rows.Add(playerRow);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                lastError = "Error reading player list: " + exp.Message;
            }
        }
    }
}
