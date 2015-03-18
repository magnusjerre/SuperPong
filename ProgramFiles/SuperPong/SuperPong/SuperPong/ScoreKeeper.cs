using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperPong.MJFrameWork;

namespace SuperPong
{
    public class ScoreKeeper
    {

        private int leftPlayerScore = 0, rightPlayerScore = 0, maxScore = 20;
        public int LeftPlayerScore { get { return leftPlayerScore; } }
        public int RightPlayerScore { get { return rightPlayerScore; } }
        public int MaxScore { get { return maxScore; } }

        public const String LEFT_PLAYER = "LeftPlayer", RIGHT_PLAYER = "RightPlayer";
        public const String LEFT_GOAL = "LeftGoal", RIGHT_GOAL = "RightGoal";

        List<ScoreObserver> observers;

        public ScoreKeeper(int maxScore)
        {
            this.maxScore = maxScore;
            observers = new List<ScoreObserver>();
        }

        public void AddObserver(ScoreObserver observerToAdd)
        {
            if (!observers.Contains(observerToAdd))
                observers.Add(observerToAdd);
        }

        public void Reset()
        {
            leftPlayerScore = 0;
            rightPlayerScore = 0;
        }

        public void RemoveObserver(ScoreObserver observerToRemove)
        {
            if (observers.Contains(observerToRemove))
                observers.Remove(observerToRemove);
        }

        public void IncreaseScore(MJIntersection collisionPair)
        {
            if (IsLeftGoalIntersection(collisionPair))
                NotifyPlayerScored(ref rightPlayerScore, RIGHT_PLAYER);
            else if (IsRightGoalIntersection(collisionPair))
                NotifyPlayerScored(ref leftPlayerScore, LEFT_PLAYER);
        }

        private void NotifyPlayerScored(ref int playerScore, String playerName)
        {
            playerScore++;
            if (ReachedMaxScore(playerScore))
            {
                foreach (ScoreObserver observer in observers)
                {
                    observer.NotifyMaxScoreReached(playerName);
                }
            }
            else
            {
                foreach (ScoreObserver observer in observers)
                {
                    if (playerName.Equals(RIGHT_PLAYER))
                        observer.NotifyRightPlayerScored(playerScore);
                    else if (playerName.Equals(LEFT_PLAYER))
                        observer.NotifyLeftPlayerScored(playerScore);
                }
            }
        }

        private Boolean ReachedMaxScore(int playerScore)
        {
            return playerScore >= maxScore;
        }

        private Boolean IsLeftGoalIntersection(MJIntersection collisionPair)
        {
            if (collisionPair.Body1.Bitmask != Bitmasks.BALL && collisionPair.Body2.Bitmask != Bitmasks.BALL)
                return false;

            return collisionPair.Body1.Parent.Name.Equals(LEFT_GOAL) ||
                collisionPair.Body2.Parent.Name.Equals(LEFT_GOAL);
        }

        private Boolean IsRightGoalIntersection(MJIntersection collisionPair)
        {
            if (collisionPair.Body1.Bitmask != Bitmasks.BALL && collisionPair.Body2.Bitmask != Bitmasks.BALL)
                return false;
            return collisionPair.Body1.Parent.Name.Equals(RIGHT_GOAL) ||
                collisionPair.Body2.Parent.Name.Equals(RIGHT_GOAL);
        }

    }
}
