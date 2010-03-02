using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkBackgammonGameLogic
{
    public interface IPlayerEventInfo
    {
        Checker GetSelectedChecker();
        Dice GetSelectedMove();
    }
}
