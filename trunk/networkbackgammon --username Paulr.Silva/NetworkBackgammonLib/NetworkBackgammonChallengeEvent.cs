using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    [Serializable]
    public class NetworkBackgammonChallengeEvent : INetworkBackgammonEvent
    {
        NetworkBackgammonPlayer m_challengingPlayer = null;

        public NetworkBackgammonChallengeEvent(NetworkBackgammonPlayer challengingPlayer)
        {
            m_challengingPlayer = challengingPlayer;
        }

        public NetworkBackgammonPlayer ChallengingPlayer
        {
            get
            {
                return m_challengingPlayer;
            }
        }
    }
}
