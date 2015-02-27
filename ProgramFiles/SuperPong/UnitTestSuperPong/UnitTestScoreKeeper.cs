using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperPong;
using SuperPong.MJFrameWork;

namespace UnitTestSuperPong
{
    [TestClass]
    public class UnitTestScoreKeeper
    {
        [TestMethod]
        public void TestMaxScoreReached()
        {
            MJNode ball = new MJNode();
            ball.Name = "Ball";
            ball.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(10));

            MJNode leftGoal = new MJNode();
            leftGoal.Name = ScoreKeeper.LEFT_GOAL;
            leftGoal.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(10));

            MJNode other = new MJNode();
            other.Name = "OtherNode";
            other.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(10));

            MJCollisionPair scorePair = new MJCollisionPair(ball.PhysicsBody, leftGoal.PhysicsBody);
            MJCollisionPair otherPair = new MJCollisionPair(ball.PhysicsBody, other.PhysicsBody);

            ScoreKeeper scoreKeeper = new ScoreKeeper(5);
            Observer observer = new Observer();
            scoreKeeper.AddObserver(observer);

            Assert.AreEqual(0, observer.rightPlayerScore);
            Assert.IsFalse(observer.maxReached);
            scoreKeeper.IncreaseScore(scorePair);
            Assert.AreEqual(1, observer.rightPlayerScore);
            Assert.IsFalse(observer.maxReached);
            scoreKeeper.IncreaseScore(scorePair);
            Assert.AreEqual(2, observer.rightPlayerScore);
            Assert.IsFalse(observer.maxReached);
            scoreKeeper.IncreaseScore(scorePair);
            Assert.AreEqual(3, observer.rightPlayerScore);
            Assert.IsFalse(observer.maxReached);
            scoreKeeper.IncreaseScore(scorePair);
            Assert.AreEqual(4, observer.rightPlayerScore);
            Assert.IsFalse(observer.maxReached);
            scoreKeeper.IncreaseScore(otherPair);
            Assert.AreEqual(4, observer.rightPlayerScore);
            Assert.IsFalse(observer.maxReached);
            scoreKeeper.IncreaseScore(scorePair);
            Assert.AreEqual(true, observer.maxReached);
        }


        [TestMethod]
        public void TestNotifyScoreChange()
        {
            MJNode ball = new MJNode();
            ball.Name = "Ball";
            ball.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(10));

            MJNode leftGoal = new MJNode();
            leftGoal.Name = ScoreKeeper.LEFT_GOAL;
            leftGoal.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(10));

            MJNode rightGoal = new MJNode();
            rightGoal.Name = ScoreKeeper.RIGHT_GOAL;
            rightGoal.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(10));


            MJNode other = new MJNode();
            other.Name = "OtherNode";
            other.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(10));

            MJCollisionPair leftGoalScore = new MJCollisionPair(ball.PhysicsBody, leftGoal.PhysicsBody);
            MJCollisionPair rightGoalScore = new MJCollisionPair(ball.PhysicsBody, rightGoal.PhysicsBody);
            MJCollisionPair otherPair = new MJCollisionPair(ball.PhysicsBody, other.PhysicsBody);

            ScoreKeeper scoreKeeper = new ScoreKeeper(5);
            Observer observer = new Observer();
            scoreKeeper.AddObserver(observer);

            Assert.IsFalse(observer.maxReached);
            Assert.AreEqual(0, observer.leftPlayerScore);
            Assert.AreEqual(0, observer.rightPlayerScore);

            scoreKeeper.IncreaseScore(otherPair);
            Assert.IsFalse(observer.maxReached);
            Assert.AreEqual(0, observer.leftPlayerScore);
            Assert.AreEqual(0, observer.rightPlayerScore);

            scoreKeeper.IncreaseScore(leftGoalScore);
            Assert.IsFalse(observer.maxReached);
            Assert.AreEqual(0, observer.leftPlayerScore);
            Assert.AreEqual(1, observer.rightPlayerScore);

            scoreKeeper.IncreaseScore(leftGoalScore);
            Assert.IsFalse(observer.maxReached);
            Assert.AreEqual(0, observer.leftPlayerScore);
            Assert.AreEqual(2, observer.rightPlayerScore);

            scoreKeeper.IncreaseScore(rightGoalScore);
            Assert.IsFalse(observer.maxReached);
            Assert.AreEqual(1, observer.leftPlayerScore);
            Assert.AreEqual(2, observer.rightPlayerScore);

            scoreKeeper.IncreaseScore(otherPair);
            Assert.IsFalse(observer.maxReached);
            Assert.AreEqual(1, observer.leftPlayerScore);
            Assert.AreEqual(2, observer.rightPlayerScore);

            scoreKeeper.IncreaseScore(leftGoalScore);
            Assert.IsFalse(observer.maxReached);
            Assert.AreEqual(1, observer.leftPlayerScore);
            Assert.AreEqual(3, observer.rightPlayerScore);

            scoreKeeper.IncreaseScore(leftGoalScore);
            Assert.IsFalse(observer.maxReached);
            Assert.AreEqual(1, observer.leftPlayerScore);
            Assert.AreEqual(4, observer.rightPlayerScore);

            scoreKeeper.IncreaseScore(rightGoalScore);
            Assert.IsFalse(observer.maxReached);
            Assert.AreEqual(2, observer.leftPlayerScore);
            Assert.AreEqual(4, observer.rightPlayerScore);

            //When scoring the final goal, the observer class will not be able
            //to update its score value, therefore none of the players will 
            //actually have a final score equal to the max score
            scoreKeeper.IncreaseScore(leftGoalScore);
            Assert.IsTrue(observer.maxReached);
            Assert.AreEqual(2, observer.leftPlayerScore);
            Assert.AreEqual(4, observer.rightPlayerScore);
        }

        [TestMethod]
        public void TestRemoveObserver()
        {
            MJNode ball = new MJNode();
            ball.Name = "Ball";
            ball.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(10));

            MJNode leftGoal = new MJNode();
            leftGoal.Name = ScoreKeeper.LEFT_GOAL;
            leftGoal.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(10));

            MJNode rightGoal = new MJNode();
            rightGoal.Name = ScoreKeeper.RIGHT_GOAL;
            rightGoal.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(10));


            MJNode other = new MJNode();
            other.Name = "OtherNode";
            other.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(10));

            MJCollisionPair leftGoalScore = new MJCollisionPair(ball.PhysicsBody, leftGoal.PhysicsBody);
            MJCollisionPair rightGoalScore = new MJCollisionPair(ball.PhysicsBody, rightGoal.PhysicsBody);
            MJCollisionPair otherPair = new MJCollisionPair(ball.PhysicsBody, other.PhysicsBody);

            ScoreKeeper scoreKeeper = new ScoreKeeper(5);
            Observer observer = new Observer();
            scoreKeeper.AddObserver(observer);

            Assert.IsFalse(observer.maxReached);
            Assert.AreEqual(0, observer.leftPlayerScore);
            Assert.AreEqual(0, observer.rightPlayerScore);

            scoreKeeper.IncreaseScore(leftGoalScore);
            Assert.IsFalse(observer.maxReached);
            Assert.AreEqual(0, observer.leftPlayerScore);
            Assert.AreEqual(1, observer.rightPlayerScore);

            scoreKeeper.RemoveObserver(observer);
            scoreKeeper.IncreaseScore(leftGoalScore);
            Assert.IsFalse(observer.maxReached);
            Assert.AreEqual(0, observer.leftPlayerScore);
            Assert.AreEqual(1, observer.rightPlayerScore);
        }
    }
}
