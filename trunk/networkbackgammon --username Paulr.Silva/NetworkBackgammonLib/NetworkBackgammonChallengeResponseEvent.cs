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
        string m_challengedPlayer;
        string m_challengingPlayer;

        public NetworkBackgammonChallengeResponseEvent(bool challengeAccepted, string challengedPlayer, string challengingPlayer)
        {
            m_challengeAccepted = challengeAccepted;
            m_challengedPlayer = challengedPlayer;
            m_challengingPlayer = challengingPlayer;
        }

        public bool ChallengeAccepted
        {
            get
            {
                return m_challengeAccepted;
            }
        }

        public string ChallengedPlayer
        {
            get
            {
                return m_challengedPlayer;
            }
        }

        public string ChallengingPlayer
        {
            get
            {
                return m_challengingPlayer;
            }
        }
    }
}
