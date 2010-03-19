using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.IO;
using NetworkBackgammonLib;
using NetworkBackgammonRemotingLib;

namespace NetworkBackgammon
{
    public partial class NetworkBackgammonBoard : Form, INetworkBackgammonListener
    {
        // Handles listening to NetworkBackgammon events
        INetworkBackgammonListener m_defaultListener = new NetworkBackgammonListener();
    
        // Initial point for player 1 first piece
        static Point m_startPoint1 = new Point(484, 12);
        // Initial point for player 2 first piece
        static Point m_startPoint2 = new Point(484, 359);
        // Moveable chip represent this players pieces
        ArrayList m_playerChipList = new ArrayList();
        // Static pieces that represent the opponents chips
        ArrayList m_opponentChipList = new ArrayList();
        // Available board position list
        ArrayList m_boardPositionList = new ArrayList();
        // List of available dice icon images
        ArrayList m_diceIconList = new ArrayList();
        // Chip adjacent placement padding 
        int m_chipAdjPadding = 4;
        // Bar width
        int m_barWidth = 27;
        // Maximum number of rows in a column
        int m_maxRows = 5;
        // Max number of columns
        int m_maxCols = 24;
        // Max number of chips (per player)
        int m_maxNumChips = 15;
        // Player needs to roll the dice
        bool m_playerRollDice = true;
        // Initial dice roll flag
        bool m_initDiceRoll = true;
        // Dice are currently being rolled
        bool m_diceRolling = false;
        // Button rolling dice
        Button m_rollDiceButton = new Button();
        // Dice animation count down timer
        int m_diceTimer = 0;
        // Dice index for the current player
        int[] m_playerDiceIndex = new int[2];
        // Dice index for the current player
        int[] m_opponentDiceIndex = new int[2];

        // Constructor
        public NetworkBackgammonBoard()
        {
            InitializeComponent();

            // Become a listener of the player 
            if (NetworkBackgammonClient.Instance.Player != null)
            {
                NetworkBackgammonClient.Instance.Player.AddListener(this);
            }

            // Create the board piece positions (just ID)
            for(int col = 0; col < m_maxCols; col++)
            {
                for (int row = 0; row < m_maxRows; row++)
                {
                    m_boardPositionList.Add(new NetworkBackgammonBoardPosition(new Point(col, row)));
                }
            }

            // Create icon image list
            for (int i = 1; i < 7; i++)
            {
                // Load the bitmap directly from the manifest resource
                Icon diceIcon = new Icon(this.GetType(), "Resources.Dice" + i + ".ico");
                // Set the chip bitmap
                Bitmap diceImage = new Bitmap(diceIcon.ToBitmap());
                m_diceIconList.Add( diceImage );
            }
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
            // TODO: Add game session events here...
        }

        #endregion

        // Get the board position object based on col row identifier
        public NetworkBackgammonBoardPosition GetBoardPosition(int col, int row)
        {
            NetworkBackgammonBoardPosition retval = new NetworkBackgammonBoardPosition(new Point(0, 0));

            for (int i = 0; i < m_boardPositionList.Count; i++)
            {
                NetworkBackgammonBoardPosition tempObj = (NetworkBackgammonBoardPosition)m_boardPositionList[i];
                if ((tempObj.PositionID.X == col) && (tempObj.PositionID.Y == row))
                {
                    retval = tempObj;
                    break;
                }
            }

            return retval;
        }

        // Map board position objects to pixel locations on the board
        private void MapBoardPositions()
        {
            NetworkBackgammonChip tempChip = new NetworkBackgammonChip(CHIP_TYPE.OPPONENT_1);
            int chipWidth = tempChip.ChipSize.Width;
            int chipHeight = tempChip.ChipSize.Height;
            int col = 0, row = 0;

            // Build all the board positions based on chip and board properties
            for (col = 0; col < m_maxCols; col++)
            {
                for (row = 0; row < m_maxRows; row++)
                {
                    int arrayPos = (row + col * m_maxRows);
                    NetworkBackgammonBoardPosition tempObj = (NetworkBackgammonBoardPosition)m_boardPositionList[arrayPos];

                    tempObj.LocationSize = (new Size(chipWidth, chipHeight));

                    if (arrayPos >= 0 && arrayPos < 30)
                    {
                        tempObj.LocationPoint = new Point(m_startPoint1.X - col * (chipWidth + m_chipAdjPadding),
                                                             m_startPoint1.Y + row * (chipHeight));
                    }
                    else if (arrayPos >= 30 && arrayPos < 60)
                    {
                        tempObj.LocationPoint = (new Point(m_startPoint1.X - col * (chipWidth + m_chipAdjPadding) - m_barWidth,
                                                              m_startPoint1.Y + row * chipHeight));

                    }
                    else if (arrayPos >= 60 && arrayPos < 120)
                    {
                        tempObj.LocationPoint = (new Point(GetBoardPosition((m_maxCols - 1) - col, row).LocationPoint.X,
                                                              m_startPoint2.Y - row * chipHeight));
                    }
                }
            }
        }

        // Get both players check positions and draw them tot he screen 
        private void DrawPlayerPositions()
        {
            NetworkBackgammonChip boardChip;
            
            // Current player
            NetworkBackgammonPlayer curPlayer = NetworkBackgammonClient.Instance.Player;

            // Remove old list
            m_playerChipList.Clear();

            if (curPlayer != null)
            {
                foreach (NetworkBackgammonChecker checkerAP in curPlayer.Checkers)
                {
                    boardChip =  new NetworkBackgammonChip(CHIP_TYPE.OPPONENT_1);

                    Int32 newColPos = Convert.ToInt32(checkerAP.CurrentPosition.CurrentPosition) - 1;
                    Int32 newRowPos = 0;

                    // Search board chip list and determine the row
                    foreach (NetworkBackgammonChip chip in m_playerChipList)
                    {
                        if (chip.ChipBoardPosition.PositionID.X == newColPos)
                        {
                            newRowPos++;
                        }
                    }

                    boardChip.ChipBoardPosition = GetBoardPosition(newColPos, newRowPos);

                    m_playerChipList.Add(boardChip);

                }
            }

            // Draw oppising players checkers too...
         
            NetworkBackgammonPlayer oppPlayer = NetworkBackgammonClient.Instance.GameRoom.GetOpposingPlayer(curPlayer);

            // Remove old list
            m_opponentChipList.Clear();

            if (oppPlayer != null)
            {
                foreach (NetworkBackgammonChecker checkerAP in oppPlayer.Checkers)
                {
                    boardChip = new NetworkBackgammonChip(CHIP_TYPE.OPPONENT_2);

                    Int32 newColPos = Convert.ToInt32(checkerAP.CurrentPosition.GetOppositePosition().CurrentPosition) - 1;
                    Int32 newRowPos = 0;

                    // Search board chip list and determine the row
                    foreach (NetworkBackgammonChip chip in m_opponentChipList)
                    {
                        if (chip.ChipBoardPosition.PositionID.X == newColPos)
                        {
                            newRowPos++;
                        }
                    }

                    boardChip.ChipBoardPosition = GetBoardPosition(newColPos, newRowPos);

                    m_opponentChipList.Add(boardChip);
                }
            }
        }

        // Overriden load function of the form
        private void NetworkBackgammonBoard_Load(object sender, EventArgs e)
        {
            // Start the dice roll timer
            timerRollDice.Start();

            // Map each board position coordinate to a specific pixel coordinate location 
            MapBoardPositions();

            // Draw the current player positions
            DrawPlayerPositions();
        }

        private void NetworkBackgammonBoard_Paint(object sender, PaintEventArgs e)
        {
            // Draw the board background image
            DrawBoardBackground(sender, e);

            // Draw all the available chips for the current player and the opponent
            DrawBoardChips(sender, e);

            // Draw board trays - includes player and opponent
            DrawBoardTrays(sender, e);

            // Check if the dice need rolling
            DrawDiceButton(sender, e);

            // Draw the animated dice rolling
            DrawRollingDice(sender, e);

             // Draw opponents current dice roll
            DrawOpponentDice(sender, e);
        }

        // Draw the board background image
        private void DrawBoardBackground(object sender, PaintEventArgs e)
        {
              // Load the bitmap directly from the manifest resource
            Bitmap backgroundImage = new Bitmap(this.GetType(), "Resources.BackgammonBoard.bmp");
            // Draw the back ground image
            e.Graphics.DrawImage(backgroundImage, this.ClientRectangle,
                   new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height),
                  GraphicsUnit.Pixel);
        }

        // Draw all the available chips for the current player and the opponent
        private void DrawBoardChips(object sender, PaintEventArgs e)
        {
            // Paint this player's chips on the board
            for (int i = 0; i < m_playerChipList.Count; i++)
            {
                NetworkBackgammonChip boardChip = (NetworkBackgammonChip)m_playerChipList[i];
                // Draw the back ground image
                e.Graphics.DrawImage(boardChip.ChipImage,
                                     boardChip.ChipPixelPosition.X,
                                     boardChip.ChipPixelPosition.Y);
            }

            // Paint oppponent's chips on the board
            for (int i = 0; i < m_opponentChipList.Count; i++)
            {
                NetworkBackgammonChip boardChip = (NetworkBackgammonChip)m_opponentChipList[i];
                // Draw the back ground image
                e.Graphics.DrawImage(boardChip.ChipImage,
                                     boardChip.ChipPixelPosition.X,
                                     boardChip.ChipPixelPosition.Y);
            }
        }

        // Draw board trays - includes this player and opponent's chips
        private void DrawBoardTrays(object sender, PaintEventArgs e)
        {
            // Pen for the outline of the tray
            Pen trayOutlinePen = new Pen(System.Drawing.Color.Gray, 1.0f);

            // TODO: Get ride of hard coded numbers

            // Get the player chip count - anything outside of this count will be draw in the tray
            int chipListSize = m_playerChipList.Count;

            // Draw players tray  
            for (int i = 0; i < m_maxNumChips; i++)
            {
                Rectangle rectPlayer = new Rectangle(531, 13 + 8 * (i), 32, 8); // 124
                e.Graphics.DrawRectangle(trayOutlinePen, rectPlayer);

                if (i > chipListSize)
                {
                    // Load the bitmap directly from the manifest resource
                    Icon trayIcon = new Icon(this.GetType(), "Resources.Player1TrayChip.ico");
                    // Set the chip bitmap
                    Bitmap trayImage = new Bitmap(trayIcon.ToBitmap());
                    // Draw the back ground image
                    e.Graphics.DrawImage(trayImage, 531, 13 + 8 * (i));
                }
            }

            // Get the player chip count - anything outside of this count will be draw in the tray
            int oppListSize = m_opponentChipList.Count;

            // Draw opponents tray
            for (int i = 0; i < m_maxNumChips; i++)
            {
                Rectangle rectOpponent = new Rectangle(531, 269 + 8 * (i), 32, 8);
                e.Graphics.DrawRectangle(trayOutlinePen, rectOpponent);

                if (i > oppListSize)
                {
                    // Load the bitmap directly from the manifest resource
                    Icon trayIcon = new Icon(this.GetType(), "Resources.Player2TrayChip.ico");
                    // Set the chip bitmap
                    Bitmap trayImage = new Bitmap(trayIcon.ToBitmap());
                    // Draw the back ground image
                    e.Graphics.DrawImage(trayImage, 531, 269 + 8 * (i));
                }
            }
        }

        // Draw/Animate the rolling dice
        private void DrawRollingDice(object sender, PaintEventArgs e)
        {
            if (m_diceRolling)
            {
               // Draw the back ground image
               e.Graphics.DrawImage((Bitmap)m_diceIconList[m_playerDiceIndex[0]], 385, 185);

               if (!m_initDiceRoll)
               {
                   e.Graphics.DrawImage((Bitmap)m_diceIconList[m_playerDiceIndex[1]], 417, 185);
               }
            }
            else // Check here if its the current players turn
            {
                if (!m_playerRollDice)
                {
                    // Draw the back ground image
                    e.Graphics.DrawImage((Bitmap)m_diceIconList[m_playerDiceIndex[0]], 385, 185);

                    if (!m_initDiceRoll)
                    {
                        e.Graphics.DrawImage((Bitmap)m_diceIconList[m_playerDiceIndex[1]], 417, 185);
                    }
                }
            }
        }

        // Draw opponents current dice roll
        private void DrawOpponentDice(object sender, PaintEventArgs e)
        {
           /* if (m_diceRolling)
            {
                // Draw the back ground image
                e.Graphics.DrawImage((Bitmap)m_diceIconList[m_playerDiceIndex[0]], 385, 185);
                e.Graphics.DrawImage((Bitmap)m_diceIconList[m_playerDiceIndex[1]], 417, 185);
            }
            else // Check here if its the current players turn
            {
                if (!m_playerRollDice)
                {
                    // Draw the back ground image
                    e.Graphics.DrawImage((Bitmap)m_diceIconList[m_playerDiceIndex[0]], 385, 185);
                    e.Graphics.DrawImage((Bitmap)m_diceIconList[m_playerDiceIndex[1]], 417, 185);
                }
            }
            */
        }

        // Draw the roll dice button and hook up the handlers
        private void DrawDiceButton(object sender, PaintEventArgs e)
        {
            if (m_playerRollDice)
            {
                m_rollDiceButton.Text = "Roll Dice";
                m_rollDiceButton.Name = "rollDiceButton";
                m_rollDiceButton.Size = new System.Drawing.Size(70, 32);
                m_rollDiceButton.Location = new System.Drawing.Point(385, 185);
                this.Controls.Add(m_rollDiceButton);
                m_rollDiceButton.Click += new System.EventHandler(OnClickRollButton);
            }
            else
            {
                this.Controls.Remove(m_rollDiceButton);
            }
        }

        // Button handler for the dice roll button
        private void OnClickRollButton(object sender, System.EventArgs e)
        {
            if (m_playerRollDice)
            {
                m_playerRollDice = false;
                m_diceRolling = true;
                m_diceTimer = 10;
                // Repaint the screen
                Refresh();
            }
        }

        // Handle mouse down event - check if mouse click position is inside a players chip
        private void NetworkBackgammonBoard_MouseDown(object sender, MouseEventArgs e)
        {
            if (!m_playerRollDice && !m_diceRolling )
            {
                for (int i = (m_playerChipList.Count - 1); i != -1; i--)
                {
                    NetworkBackgammonChip boardChip = (NetworkBackgammonChip)m_playerChipList[i];
                    if (boardChip.IsOnChip(new Point(e.X, e.Y)))
                    {
                        boardChip.Moving = true;
                        break;
                    }
                }
            }
        }

        // Handle mouse up click event - check if moving a chip and whether or not it can repositioned
        private void NetworkBackgammonBoard_MouseUp(object sender, MouseEventArgs e)
        {
            if (!m_playerRollDice && !m_diceRolling)
            {
                int i = -1;

                for (i = 0; i < m_boardPositionList.Count; i++)
                {
                    NetworkBackgammonBoardPosition tempObj = (NetworkBackgammonBoardPosition)m_boardPositionList[i];

                    if ((e.X >= tempObj.LocationPoint.X) &&
                         (e.X < tempObj.LocationPoint.X + tempObj.LocationSize.Width) &&
                         (e.Y >= tempObj.LocationPoint.Y) &&
                         (e.Y < tempObj.LocationPoint.Y + tempObj.LocationSize.Height))
                    {
                        break;
                    }
                }

                // Any mouse up click will reset the moving flag
                for (int chipIndex = 0; chipIndex < m_playerChipList.Count; chipIndex++)
                {
                    NetworkBackgammonChip boardChip = (NetworkBackgammonChip)m_playerChipList[chipIndex];

                    if (boardChip.Moving)
                    {
                        if (i == m_boardPositionList.Count)
                        {
                            // Move the location to the drop chip position
                            boardChip.ChipPixelPosition = boardChip.ChipBoardPosition.LocationPoint;
                        }
                        else
                        {
                            // Move the location to the drop chip position
                            boardChip.ChipPixelPosition = ((NetworkBackgammonBoardPosition)m_boardPositionList[i]).LocationPoint;
                            // Move the board location to the drop chip position
                            boardChip.ChipBoardPosition = (NetworkBackgammonBoardPosition)m_boardPositionList[i];
                        }
                    }

                    boardChip.Moving = false;

                    // Redraw the board
                    Refresh();
                }
            }
        }

        // Handle mouse move event - move the currently selected players chip (if any)
        private void NetworkBackgammonBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (!m_playerRollDice && !m_diceRolling)
            {
                // Any mouse up click will reset the moving flag
                for (int i = 0; i < m_playerChipList.Count; i++)
                {
                    NetworkBackgammonChip boardChip = (NetworkBackgammonChip)m_playerChipList[i];
                    if (boardChip.Moving)
                    {
                        boardChip.ChipPixelPosition = new Point(e.X - boardChip.ChipSize.Width / 2,
                                                                e.Y - boardChip.ChipSize.Height / 2);
                        // Redraw the board
                        Refresh();
                    }
                }
            }
        }

        // Roll dice timer event
        private void timerRollDice_Tick(object sender, EventArgs e)
        {
            // Check if we are rolling our dice
            if (m_diceRolling)
            {
                if (m_diceTimer-- > 0)
                {
                    Random random = new Random();

                    m_playerDiceIndex[0] = random.Next(0, 5);
                    
                    // Check if the this is the initial dice roll (only need one dice)
                    if (!m_initDiceRoll)
                    {
                        m_playerDiceIndex[1] = random.Next(0, 5);
                    }
                }
                else
                {
                    if( m_initDiceRoll )
                    {
                        m_playerDiceIndex[0] = (int)NetworkBackgammonClient.Instance.Player.InitialDice.CurrentValueUInt32;
                    }
                    else
                    {
                       // m_playerDiceIndex[0] = NetworkBackgammonClient.Instance.Player.InitialDice.CurrentValueUInt32;
                       // m_playerDiceIndex[1] = NetworkBackgammonClient.Instance.Player.InitialDice.CurrentValueUInt32;
                    }

                    m_diceRolling = false;
                }

                // Redraw the screen
                Refresh();
            }
        }

        // Handle the event when the form is closing
        private void NetworkBackgammonBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop the timer loop
            timerRollDice.Stop();

            // Stop listening to the player 
            if (NetworkBackgammonClient.Instance.Player != null)
            {
                NetworkBackgammonClient.Instance.Player.RemoveListener(this);
            }
        }
    }
}
