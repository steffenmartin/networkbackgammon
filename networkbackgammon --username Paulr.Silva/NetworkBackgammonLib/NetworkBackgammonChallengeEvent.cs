﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonLib
{
    [Serializable]
    public class NetworkBackgammonChallengeEvent : INetworkBackgammonEvent
    {
        NetworkBackgammonPlayer m_challengingPlayer = null;
        NetworkBackgammonPlayer m_challengedPlayer = null;

        public NetworkBackgammonChallengeEvent(NetworkBackgammonPlayer challengingPlayer,
                                                NetworkBackgammonPlayer challengedPlayer)
        {
            m_challengingPlayer = challengingPlayer;
            m_challengedPlayer = challengedPlayer;
        }

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
    }
}
