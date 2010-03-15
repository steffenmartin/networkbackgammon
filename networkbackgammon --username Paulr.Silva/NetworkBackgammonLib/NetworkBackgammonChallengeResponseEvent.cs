using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    [Serializable]
    public class NetworkBackgammonChallengeResponseEvent : INetworkBackgammonEvent
    {
        // NetworkBackgammonPlayer m_challengingPlayer = null;
        // NetworkBackgammonPlayer m_challengedPlayer = null;
        bool m_challengeAccepted = false;

        public NetworkBackgammonChallengeResponseEvent(
            /* NetworkBackgammonPlayer challengingPlayer,
            NetworkBackgammonPlayer challengedPlayer,*/
            bool challengeAccepted)
        {
            // m_challengingPlayer = challengingPlayer;
           //  m_challengedPlayer = challengedPlayer;
            m_challengeAccepted = challengeAccepted;
        }

        /*
        public NetworkBackgammonPlayer ChallengingPlayer
        {
            get
            {
                return m_challengingPlayer;
            }
        }

        public NetworkBackgammonPlayer ChallengedPlayer
        {
            get
            {
                return m_challengedPlayer;
            }
        }
        */

        public bool ChallengeAccepted
        {
            get
            {
                return m_challengeAccepted;
            }
        }
    }
}
