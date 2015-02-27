using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperPong;

namespace UnitTestSuperPong
{
    public class Observer : ScoreObserver
    {

        public Boolean maxReached = false;
        public int leftPlayerScore = 0, rightPlayerScore = 0;

        public void NotifyMaxScoreReached(string playerReachingMaxScore)
        {
            maxReached = true;
        }

        public void NotifyLeftPlayerScored(int newScore)
        {
            leftPlayerScore = newScore;
        }

        public void NotifyRightPlayerScored(int newScore)
        {
            rightPlayerScore = newScore;
        }
    }
}
