using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    [Serializable]
    public class NetworkBackgammonChallengeEvent : INetworkBackgammonEvent
    {
        string m_challengingPlayer = null;
        string m_challengedPlayer = null;

        public NetworkBackgammonChallengeEvent(string challengingPlayer, string challengedPlayer)
        {
            m_challengingPlayer = challengingPlayer;
            m_challengedPlayer = challengedPlayer;
        }

        public string ChallengingPlayer
        {
            get
            {
                return m_challengingPlayer;
            }
        }

        public string ChallengedPlayer
        {
            get
            {
                return m_challengedPlayer;
            }
        }
    }
}
