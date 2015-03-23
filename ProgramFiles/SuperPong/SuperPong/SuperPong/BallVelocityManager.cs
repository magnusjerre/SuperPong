using SuperPong.MJFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SuperPong
{
    public class BallVelocityManager : ResetGame, PointReset
    {

        private float speed;
        public float Speed { get { return speed; } }

        public float InitialSpeed { get; set; }

        public Vector2 InitialDirection { get; set; }

        public float MaxMultiplier { get; set; }
        public float TotalMultiplier { get; set; }
        public float Multiplier { get; set; }
        public int MultiplyRate { get; set; }
        private int counter;
        private Random random;
        
        private MJPhysicsBody ball;

        public BallVelocityManager(MJPhysicsBody ball, float initialSpeed, Vector2 initialDirection, float multiplier, int multiplyRate, int seed, float maxMultiplier)
        {
            this.ball = ball;
            this.InitialSpeed = initialSpeed;
            this.InitialDirection = initialDirection;
            this.Multiplier = multiplier;
            this.MultiplyRate = multiplyRate;
            this.MaxMultiplier = maxMultiplier;
            TotalMultiplier = 1f;
            random = new Random(seed);
            speed = InitialSpeed;
            counter = 0;
        }

        public BallVelocityManager(MJPhysicsBody ball) : this(ball, 550f, new Vector2(), 1.1f, 5, 2, 1.6f)
        {
            ResetAfterPoint();
        }

        public void Collision(MJIntersection collisionPair)
        {
            if (IsBallCollision(collisionPair))
            {
                counter++;
                if (counter == MultiplyRate && TotalMultiplier < MaxMultiplier)
                {
                    counter = 0;
                    speed *= Multiplier;
                    TotalMultiplier *= Multiplier;

                    Vector2 ballVelocityNormalized = ball.Velocity / ball.Velocity.Length();
                    ball.Velocity = ballVelocityNormalized * speed;
                }
            }
        }

        private Boolean IsBallCollision(MJIntersection collisionPair)
        {
            return collisionPair.Body1.Bitmask == Bitmasks.BALL || collisionPair.Body2.Bitmask == Bitmasks.BALL;
        }

        public void ResetGame()
        {
            ResetAfterPoint();
        }

        public void ResetAfterPoint()
        {
            InitialDirection = RandomDirection();
            speed = InitialSpeed;
            ball.Velocity = InitialDirection * speed;
            TotalMultiplier = 1f;

            counter = 0;
        }

        private Vector2 RandomDirection()
        {
            int newXDirection = random.Next(201) - 100;
            int newYDirection = random.Next(201) - 100;
            float length = (float)Math.Sqrt(newXDirection * newXDirection + newYDirection * newYDirection);
            return new Vector2(newXDirection / length, newYDirection / length);
        }
    }
}
