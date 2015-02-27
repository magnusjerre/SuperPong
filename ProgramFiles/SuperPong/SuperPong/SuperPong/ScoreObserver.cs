using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong
{
    public interface ScoreObserver
    {
        void NotifyMaxScoreReached(String playerReachingMaxScore);
        void NotifyLeftPlayerScored(int newScore);
        void NotifyRightPlayerScored(int newScore);
    }
}
