using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using NetworkBackgammonLib;

namespace NetworkBackgammon
{
    enum CHIP_TYPE { OPPONENT_1, OPPONENT_2 };

    class NetworkBackgammonChip
    {
        bool m_chipMoving = false;
        Point m_chipPixelPosition = new Point();
        Size m_chipSize = new Size();
        Bitmap m_chipBitmap;
        NetworkBackgammonBoardPosition m_chipBoardPosition = new NetworkBackgammonBoardPosition(new Point(0,0));
        NetworkBackgammonChecker m_checker = null;

        // Constructor
        public NetworkBackgammonChip(CHIP_TYPE chipType)
        {
            // Load the icon directly from the manifest resource
            Icon blackChipIcon = new Icon(this.GetType(),
                (chipType == CHIP_TYPE.OPPONENT_1 ? "Resources.BlackChip.ico" : "Resources.WhipChip.ico"));

            // Set the chip bitmap
            m_chipBitmap = new Bitmap(blackChipIcon.ToBitmap());
            // Set the chip size based on the image size
            ChipSize = new Size(blackChipIcon.ToBitmap().Width, blackChipIcon.ToBitmap().Height);
        }

        public NetworkBackgammonChip(CHIP_TYPE chipType, NetworkBackgammonChecker checker)
        {
            // Load the icon directly from the manifest resource
            Icon blackChipIcon = new Icon(this.GetType(),
                (chipType == CHIP_TYPE.OPPONENT_1 ? "Resources.BlackChip.ico" : "Resources.WhipChip.ico"));

            // Set the chip bitmap
            m_chipBitmap = new Bitmap(blackChipIcon.ToBitmap());
            // Set the chip size based on the image size
            ChipSize = new Size(blackChipIcon.ToBitmap().Width, blackChipIcon.ToBitmap().Height);

            m_checker = checker;
        }

        // Chip moving property
        public bool Moving
        {
            get
            {
                return m_chipMoving;
            }
            set
            {
                m_chipMoving = value;
            }
        }

        // Chip position property
        public Point ChipPixelPosition
        {
            get
            {
                return m_chipPixelPosition;
            }
            set
            {
                m_chipPixelPosition = value;
            }
        }

        // Chip position property
        public NetworkBackgammonBoardPosition ChipBoardPosition
        {
            get
            {
                return m_chipBoardPosition;
            }
            set
            {
                m_chipBoardPosition = value;
                m_chipPixelPosition = m_chipBoardPosition.LocationPoint;
            }
        }

        // Chip size property
        public Size ChipSize
        {
            get
            {
                return m_chipSize;
            }
            set
            {
                m_chipSize = value;
            }
        }

        // Chip size property
        public Bitmap ChipImage
        {
            get
            {
                return m_chipBitmap;
            }
            set
            {
                m_chipBitmap = value;
            }
        }

        // Checker property
        public NetworkBackgammonChecker Checker
        {
            get
            {
                return m_checker;
            }
        }
       
        // Checks if the given position falls within the chip area
        public bool IsOnChip(Point pos)
        {
            int xCenterPos = 0;
            int yCenterPos = 0;

            xCenterPos = ChipPixelPosition.X + ChipSize.Width / 2;
            yCenterPos = ChipPixelPosition.Y + ChipSize.Height / 2;

            return ((Math.Pow((pos.X - xCenterPos), 2) + Math.Pow((pos.Y - yCenterPos), 2) < (Math.Pow(ChipSize.Width / 2, 2))));
        }
    }
}
