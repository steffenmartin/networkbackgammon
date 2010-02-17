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

namespace NetworkBackgammon
{
    public partial class NetworkBackgammonBoard : Form
    {
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
        // Dice are currently being rolled
        bool m_diceRolling = false;
        // Button rolling dice
        Button m_rollDiceButton = new Button();
        // Dice animation count down timer
        int m_diceTimer = 100;
        // Dice index for the current player
        int[] m_playerDiceIndex = new int[2];
        // Dice index for the current player
        int[] m_opponentDiceIndex = new int[2];

        // Constructor
        public NetworkBackgammonBoard()
        {
            InitializeComponent();

            // Create the board piece positions (just ID)
            for(int col = 0; col < m_maxCols; col++)
            {
                for (int row = 0; row < m_maxRows; row++)
                {
                    m_boardPositionList.Add(new NetworkBackgammonBoardPosition(new Point(col, row)));
                }
            }

            // Get all the chips into the array
            for (int i = 0; i < m_maxNumChips; i++)
            {
                m_playerChipList.Add(new NetworkBackgammonChip(CHIP_TYPE.OPPONENT_1));
                m_opponentChipList.Add(new NetworkBackgammonChip(CHIP_TYPE.OPPONENT_2));
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
            int chipWidth = ((NetworkBackgammonChip)m_playerChipList[0]).ChipSize.Width;
            int chipHeight = ((NetworkBackgammonChip)m_playerChipList[0]).ChipSize.Height;
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

        // Just for debugging purposes - this routine sets up the chips (player & opponent) in their
        // initial game positions
        private void TestGameStartPosition()
        {
            NetworkBackgammonChip boardChip;

            ///////////////////////////////////////////////////////
            // Player
            ///////////////////////////////////////////////////////
            boardChip = (NetworkBackgammonChip)m_playerChipList[0];
            boardChip.ChipBoardPosition = GetBoardPosition(0, 0);
            boardChip = (NetworkBackgammonChip)m_playerChipList[1];
            boardChip.ChipBoardPosition = GetBoardPosition(0, 1);
        
            boardChip = (NetworkBackgammonChip)m_playerChipList[2];
            boardChip.ChipBoardPosition = GetBoardPosition(11, 0);
            boardChip = (NetworkBackgammonChip)m_playerChipList[3];
            boardChip.ChipBoardPosition = GetBoardPosition(11, 1);
            boardChip = (NetworkBackgammonChip)m_playerChipList[4];
            boardChip.ChipBoardPosition = GetBoardPosition(11, 2);
            boardChip = (NetworkBackgammonChip)m_playerChipList[5];
            boardChip.ChipBoardPosition = GetBoardPosition(11, 3);
            boardChip = (NetworkBackgammonChip)m_playerChipList[6];
            boardChip.ChipBoardPosition = GetBoardPosition(11, 4);

            boardChip = (NetworkBackgammonChip)m_playerChipList[7];
            boardChip.ChipBoardPosition = GetBoardPosition(16, 0);
            boardChip = (NetworkBackgammonChip)m_playerChipList[8];
            boardChip.ChipBoardPosition = GetBoardPosition(16, 1);
            boardChip = (NetworkBackgammonChip)m_playerChipList[9];
            boardChip.ChipBoardPosition = GetBoardPosition(16, 2);

            boardChip = (NetworkBackgammonChip)m_playerChipList[10];
            boardChip.ChipBoardPosition = GetBoardPosition(18, 0);
            boardChip = (NetworkBackgammonChip)m_playerChipList[11];
            boardChip.ChipBoardPosition = GetBoardPosition(18, 1);
            boardChip = (NetworkBackgammonChip)m_playerChipList[12];
            boardChip.ChipBoardPosition = GetBoardPosition(18, 2);
            boardChip = (NetworkBackgammonChip)m_playerChipList[13];
            boardChip.ChipBoardPosition = GetBoardPosition(18, 3);
            boardChip = (NetworkBackgammonChip)m_playerChipList[14];
            boardChip.ChipBoardPosition = GetBoardPosition(18, 4);

            ///////////////////////////////////////////////////////
            // Opponent
            ///////////////////////////////////////////////////////
            boardChip = (NetworkBackgammonChip)m_opponentChipList[0];
            boardChip.ChipBoardPosition = GetBoardPosition(23, 0);
            boardChip = (NetworkBackgammonChip)m_opponentChipList[1];
            boardChip.ChipBoardPosition = GetBoardPosition(23, 1);

            boardChip = (NetworkBackgammonChip)m_opponentChipList[2];
            boardChip.ChipBoardPosition = GetBoardPosition(12, 0);
            boardChip = (NetworkBackgammonChip)m_opponentChipList[3];
            boardChip.ChipBoardPosition = GetBoardPosition(12, 1);
            boardChip = (NetworkBackgammonChip)m_opponentChipList[4];
            boardChip.ChipBoardPosition = GetBoardPosition(12, 2);
            boardChip = (NetworkBackgammonChip)m_opponentChipList[5];
            boardChip.ChipBoardPosition = GetBoardPosition(12, 3);
            boardChip = (NetworkBackgammonChip)m_opponentChipList[6];
            boardChip.ChipBoardPosition = GetBoardPosition(12, 4);

            boardChip = (NetworkBackgammonChip)m_opponentChipList[7];
            boardChip.ChipBoardPosition = GetBoardPosition(7, 0);
            boardChip = (NetworkBackgammonChip)m_opponentChipList[8];
            boardChip.ChipBoardPosition = GetBoardPosition(7, 1);
            boardChip = (NetworkBackgammonChip)m_opponentChipList[9];
            boardChip.ChipBoardPosition = GetBoardPosition(7, 2);

            boardChip = (NetworkBackgammonChip)m_opponentChipList[10];
            boardChip.ChipBoardPosition = GetBoardPosition(5, 0);
            boardChip = (NetworkBackgammonChip)m_opponentChipList[11];
            boardChip.ChipBoardPosition = GetBoardPosition(5, 1);
            boardChip = (NetworkBackgammonChip)m_opponentChipList[12];
            boardChip.ChipBoardPosition = GetBoardPosition(5, 2);
            boardChip = (NetworkBackgammonChip)m_opponentChipList[13];
            boardChip.ChipBoardPosition = GetBoardPosition(5, 3);
            boardChip = (NetworkBackgammonChip)m_opponentChipList[14];
            boardChip.ChipBoardPosition = GetBoardPosition(5, 4);
        }

        // Overriden load function of the form
        private void NetworkBackgammonBoard_Load(object sender, EventArgs e)
        {
            // Start the dice roll timer
            timerRollDice.Start();

            // Map each board position coordinate to a specific pixel coordinate location 
            MapBoardPositions();

            //////////////////////////////////////////////////////////////////////////////////////
            // TEST
            TestGameStartPosition();
            //////////////////////////////////////////////////////////////////////////////////////
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

        // Draw board trays - includes this player and opponents chips
        private void DrawBoardTrays(object sender, PaintEventArgs e)
        {
            // Pen for the outline of the tray
            Pen trayOutlinePen = new Pen(System.Drawing.Color.Gray, 1.0f);

            // TODO: Get ride of hard coded numbers

            // Draw players tray  
            for (int i = 0; i < m_maxNumChips; i++)
            {
                Rectangle rectPlayer = new Rectangle(531, 13 + 8 * (i), 32, 8); // 124
                e.Graphics.DrawRectangle(trayOutlinePen, rectPlayer);

                // Load the bitmap directly from the manifest resource
                Icon trayIcon = new Icon(this.GetType(), "Resources.Player1TrayChip.ico");
                // Set the chip bitmap
                Bitmap trayImage = new Bitmap(trayIcon.ToBitmap());
                // Draw the back ground image
                e.Graphics.DrawImage(trayImage, 531, 13 + 8 * (i));
            }

            // Draw opponents tray
            for (int i = 0; i < m_maxNumChips; i++)
            {
                Rectangle rectOpponent = new Rectangle(531, 269 + 8 * (i), 32, 8);
                e.Graphics.DrawRectangle(trayOutlinePen, rectOpponent);

                if (i < 5)
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
                    m_playerDiceIndex[1] = random.Next(0, 5);
                }
                else
                {
                    m_diceRolling = false;
                }

                Refresh();
            }
        }

        // Handle the event when the form is closing
        private void NetworkBackgammonBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop the timer loop
            timerRollDice.Stop();
        }
    }
}
