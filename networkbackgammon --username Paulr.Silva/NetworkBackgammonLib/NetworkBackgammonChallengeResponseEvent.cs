using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    [Serializable]
    public class NetworkBackgammonChallengeResponseEvent : INetworkBackgammonEvent
    {
        bool m_challengeAccepted = false;

        public NetworkBackgammonChallengeResponseEvent(bool challengeAccepted)
        {
            m_challengeAccepted = challengeAccepted;
        }

        public bool ChallengeAccepted
        {
            get
            {
                return m_challengeAccepted;
            }
        }
    }
}
