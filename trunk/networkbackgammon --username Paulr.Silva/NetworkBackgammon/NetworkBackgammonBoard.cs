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
        ArrayList m_moveableButtonList = new ArrayList();
        // Static pieces that represent the opponents chips
        ArrayList m_unmoveableButtonList = new ArrayList();
        // Available board position list
        ArrayList m_boardPositionList = new ArrayList();
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
        }

        // Check to see if a position is ontop

        private void NetworkBackgammonBoard_Load(object sender, EventArgs e)
        {
            int i=0;
            
            // Get all the chips into the array
            for (i = 0; i < m_maxNumChips; i++)
            {
                m_moveableButtonList.Add(new NetworkBackgammonChip(CHIP_TYPE.OPPONENT_1));
                m_unmoveableButtonList.Add(new NetworkBackgammonChip(CHIP_TYPE.OPPONENT_2));
            }

            int chipWidth = ((NetworkBackgammonChip)m_moveableButtonList[0]).ChipSize.Width;
            int chipHeight = ((NetworkBackgammonChip)m_moveableButtonList[0]).ChipSize.Height;
            int col = 0, row = 0;

            // Build all the board positions based on chip and board properties
            for (col = 0; col < m_maxCols; col++)
            {
                for (row = 0; row < m_maxRows; row++)
                {
                    int arrayPos = (row + col * m_maxRows);
                    NetworkBackgammonBoardPosition tempObj = (NetworkBackgammonBoardPosition)m_boardPositionList[arrayPos];

                    tempObj.LocationSize = (new Size(chipWidth,chipHeight));

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
                        tempObj.LocationPoint = (new Point(GetBoardPosition((m_maxCols-1) - col, row).LocationPoint.X,
                                                              m_startPoint2.Y - row * chipHeight));
                    }
                }
            }

            // Place the initial chip
            NetworkBackgammonChip boardChip = (NetworkBackgammonChip)m_moveableButtonList[0];
            boardChip.ChipPixelPosition = GetBoardPosition(0, 0).LocationPoint;
            boardChip.ChipBoardPosition = GetBoardPosition(0, 0);
            
            // Place the second chip
            NetworkBackgammonChip secondBoardChip = (NetworkBackgammonChip)m_moveableButtonList[1];
            secondBoardChip.ChipPixelPosition = GetBoardPosition(0, 1).LocationPoint;
            secondBoardChip.ChipBoardPosition = GetBoardPosition(0, 1);

            // Place the second chip
            NetworkBackgammonChip thirdBoardChip = (NetworkBackgammonChip)m_moveableButtonList[2];
            thirdBoardChip.ChipPixelPosition = GetBoardPosition(20, 3).LocationPoint;
            thirdBoardChip.ChipBoardPosition = GetBoardPosition(20, 3);
        }

        // Get the board position object based on col row identifier
        public NetworkBackgammonBoardPosition GetBoardPosition(int col, int row)
        {
            NetworkBackgammonBoardPosition retval = new NetworkBackgammonBoardPosition(new Point(0,0));

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

        private void NetworkBackgammonBoard_Paint(object sender, PaintEventArgs e)
        {
            // Get this type's assembly
            Assembly assem = this.GetType().Assembly;
            // Load the bitmap directly from the manifest resource
            Bitmap backgroundImage = new Bitmap(this.GetType(), "Resources.BackgammonBoard.bmp");
            // Draw the back ground image
            e.Graphics.DrawImage(backgroundImage, this.ClientRectangle,
                   new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height),
                  GraphicsUnit.Pixel);

            // Paint all available chips
            for(int i = 0; i < m_moveableButtonList.Count; i++)
            {
                NetworkBackgammonChip boardChip = (NetworkBackgammonChip)m_moveableButtonList[i];
                // Draw the back ground image
                e.Graphics.DrawImage(boardChip.ChipImage,
                                     boardChip.ChipPixelPosition.X, 
                                     boardChip.ChipPixelPosition.Y);
            }
        }

        private void NetworkBackgammonBoard_MouseDown(object sender, MouseEventArgs e)
        {
            for(int i = (m_moveableButtonList.Count-1); i != -1 ; i--)
            {
                NetworkBackgammonChip boardChip = (NetworkBackgammonChip)m_moveableButtonList[i];
               if( boardChip.IsOnChip( new Point(e.X, e.Y) ) )
               {
                   boardChip.Moving = true;
                   break;
               }
            }
        }

        private void NetworkBackgammonBoard_MouseUp(object sender, MouseEventArgs e)
        {
            int i = -1;
            
            for (i = 0; i < m_boardPositionList.Count; i++)
            {
                NetworkBackgammonBoardPosition tempObj = (NetworkBackgammonBoardPosition)m_boardPositionList[i];
               
                if ( (e.X >= tempObj.LocationPoint.X) &&
                     (e.X < tempObj.LocationPoint.X + tempObj.LocationSize.Width) &&
                     (e.Y >= tempObj.LocationPoint.Y) &&
                     (e.Y < tempObj.LocationPoint.Y + tempObj.LocationSize.Height))
                {
                    break;
                }
            }

            // Any mouse up click will reset the moving flag
            for (int chipIndex = 0; chipIndex < m_moveableButtonList.Count; chipIndex++)
            {
                NetworkBackgammonChip boardChip = (NetworkBackgammonChip)m_moveableButtonList[chipIndex];

                if (boardChip.Moving )
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

        private void NetworkBackgammonBoard_MouseMove(object sender, MouseEventArgs e)
        {
            // Any mouse up click will reset the moving flag
            for (int i = 0; i < m_moveableButtonList.Count; i++)
            {
                NetworkBackgammonChip boardChip = (NetworkBackgammonChip)m_moveableButtonList[i];
                if (boardChip.Moving)
                {
                    boardChip.ChipPixelPosition = new Point(e.X - boardChip.ChipSize.Width/2, 
                                                            e.Y - boardChip.ChipSize.Height/2);
                    // Redraw the board
                    Refresh();
                }
            }
        }
    }
}
