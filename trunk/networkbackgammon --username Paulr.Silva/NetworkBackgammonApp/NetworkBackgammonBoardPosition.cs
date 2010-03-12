using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace NetworkBackgammon
{
    public class NetworkBackgammonBoardPosition
    {
        Point m_positionID = new Point(0, 0);
        Point m_positionLocation = new Point();
        Size m_locationSize = new Size();

        // Constructor - defines a position ID
        public NetworkBackgammonBoardPosition(Point posID)
        {
            m_positionID = posID;
        }

        // Constructor - defines a position ID, Center Location, and Size
        public NetworkBackgammonBoardPosition(Point posID, Point posCenterLoc, Size posSize)
        {
            m_positionID = posID;
            m_positionLocation = posCenterLoc;
            m_locationSize = posSize;
        }

        // Position ID property
        public Point PositionID
        {
            get
            {
                return m_positionID;
            }
            private set
            {
                m_positionID = value;
            }
        }
        // Upper Left Position property
        public Point LocationPoint
        {
            get
            {
                return m_positionLocation;
            }
            set
            {
                m_positionLocation = value;
            }
        }
        // Position location size (pixels) property
        public Size LocationSize
        {
            get
            {
                return m_locationSize;
            }
            set
            {
                m_locationSize = value;
            }
        }
    }
}
