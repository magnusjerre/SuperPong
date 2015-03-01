using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SuperPong.MJFrameWork;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SuperPong
{
    public class GameScene : MJScene, ScoreObserver, ResetGame
    {

        MJSprite paddleLeft, paddleRight, ball;
        Texture2D paddleTexture, ballTexture;
        MJNode topWall, bottomWall, leftGoal, rightGoal;
        const float STATIC_MASS = 1000000;
        int height = 1080, width = 1920;
        Vector2 wallSize, goalSize;
        ScoreKeeper scoreKeeper;
        int maxScore = 5;
        Vector2 initialBallPosition, initialLeftPaddlePosition, initialRightPaddlePosition, initialVelocity;
        SpriteFont font;
        ScoreFont scoreFont;
        Boolean gameIsOver = false;

        public GameScene(ContentManager content) : base(content, "GameScene")
        {

        }

        public override void Initialize()
        {
            

            initialBallPosition = new Vector2(width / 2, height / 2);
            initialLeftPaddlePosition = new Vector2(100, height / 2);
            initialRightPaddlePosition = new Vector2(width - 100, height / 2);
            initialVelocity = new Vector2(500, 300); 
            wallSize = new Vector2(width, 100);
            goalSize = new Vector2(100, height);


            AttachPhysicsManager(MJPhysicsManager.getInstance());

            paddleLeft = new MJSprite(paddleTexture);   //Points right
            paddleLeft.Name = "PaddleLeft";
            paddleLeft.origin = new Vector2(0.5f, 0.5f);
            paddleLeft.AttachPhysicsBody(MJPhysicsBody.PolygonPathMJPhysicsBody(generatePaddleLeftShape()));
            paddleLeft.PhysicsBody.Mass = STATIC_MASS;
            paddleLeft.PhysicsBody.Bitmask = 1;
            paddleLeft.PhysicsBody.CollisionMask = 1;
            paddleLeft.Position = new Vector2(100, 500);
            AddChild(paddleLeft);

            paddleRight = new MJSprite(paddleTexture);  //Points left
            paddleRight.Name = "PaddleRight";
            paddleRight.origin = new Vector2(0.5f, 0.5f);
            paddleRight.SEffects = SpriteEffects.FlipHorizontally;
            paddleRight.AttachPhysicsBody(MJPhysicsBody.PolygonPathMJPhysicsBody(generatePaddleRightShape()));
            paddleRight.PhysicsBody.Mass = STATIC_MASS;
            paddleRight.PhysicsBody.Bitmask = 1;
            paddleRight.PhysicsBody.CollisionMask = 1;
            paddleRight.Position = new Vector2(1800, 550);
            AddChild(paddleRight);

            ball = new MJSprite(ballTexture);
            ball.Name = "Ball";
            ball.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(ball.Size.X / 2));
            ball.PhysicsBody.Bitmask = 1;
            ball.PhysicsBody.CollisionMask = 1;
            ball.PhysicsBody.IntersectionMask = 2;
            ball.PhysicsBody.Velocity = new Vector2(300, 400);
            ball.Position = new Vector2(width / 2, height / 2);
            AddChild(ball);

            topWall = new MJNode();
            topWall.Name = "TopWall";
            topWall.Position = new Vector2(0, 0);
            topWall.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(wallSize, new Vector2(0, 1)));   //Origin is at left bottom corner
            topWall.PhysicsBody.Mass = STATIC_MASS;
            topWall.PhysicsBody.CollisionMask = 1;
            topWall.PhysicsBody.Bitmask = 1;
            AddChild(topWall);

            bottomWall = new MJNode();
            bottomWall.Name = "BottomWall";
            bottomWall.Position = new Vector2(0, height);
            bottomWall.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(wallSize, new Vector2(0,0))); //Origin at top right corner
            bottomWall.PhysicsBody.Mass = STATIC_MASS;
            bottomWall.PhysicsBody.CollisionMask = 1;
            bottomWall.PhysicsBody.Bitmask = 1;
            AddChild(bottomWall);

            leftGoal = new MJNode();
            leftGoal.Name = "LeftGoal";
            leftGoal.Position = new Vector2(0, 0);
            leftGoal.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(goalSize, new Vector2(1, 0)));
            leftGoal.PhysicsBody.Bitmask = 2;
            leftGoal.PhysicsBody.IntersectionMask = 1;
            AddChild(leftGoal);

            rightGoal = new MJNode();
            rightGoal.Name = "RightGoal";
            rightGoal.Position = new Vector2(width, 0);
            rightGoal.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(goalSize, new Vector2(0, 0)));
            rightGoal.PhysicsBody.Bitmask = 2;
            rightGoal.PhysicsBody.IntersectionMask = 1;
            
            AddChild(rightGoal);

            scoreFont = new ScoreFont(new Vector2(width, height), font);

            scoreKeeper = new ScoreKeeper(maxScore);
            scoreKeeper.AddObserver(this);
            scoreKeeper.AddObserver(scoreFont);
        }

        private List<Vector2> generatePaddleLeftShape()
        {
            List<Vector2> paddleLeftShape = new List<Vector2>();
            paddleLeftShape.Add(new Vector2(50, -50));
            paddleLeftShape.Add(new Vector2(0, -100));
            paddleLeftShape.Add(new Vector2(-50, -100));
            paddleLeftShape.Add(new Vector2(0, -50));
            paddleLeftShape.Add(new Vector2(0, 50));
            paddleLeftShape.Add(new Vector2(-50, 100));
            paddleLeftShape.Add(new Vector2(0, 100));
            paddleLeftShape.Add(new Vector2(50, 50));
            return paddleLeftShape;
        }

        private List<Vector2> generatePaddleRightShape()
        {
            List<Vector2> paddleRightShape = new List<Vector2>();
            paddleRightShape.Add(new Vector2(-50, -50));    //0
            paddleRightShape.Add(new Vector2(-50, 50));     //1
            paddleRightShape.Add(new Vector2(0, 100));      //2
            paddleRightShape.Add(new Vector2(50, 100));     //3
            paddleRightShape.Add(new Vector2(0, 50));       //4
            paddleRightShape.Add(new Vector2(0, -50));      //5
            paddleRightShape.Add(new Vector2(50, -100));    //6
            paddleRightShape.Add(new Vector2(0, -100));     //7
            return paddleRightShape;
        }

        private void ResetAfterPointScored()
        {
            ball.Position = initialBallPosition;
            ball.PhysicsBody.Velocity = initialVelocity;
            ball.PhysicsBody.Acceleration = new Vector2();
            paddleLeft.Position = initialLeftPaddlePosition;
            paddleRight.Position = initialRightPaddlePosition;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            scoreFont.Draw(spriteBatch);
        }

        public void ResetGame()
        {
            ResetAfterPointScored();
            scoreKeeper.Reset();
            scoreFont.ResetGame();
        }

        public override void LoadContent()
        {
            paddleTexture = LoadTexture2D("Paddle");
            ballTexture = LoadTexture2D("ball");
            font = content.Load<SpriteFont>("TheSpriteFont");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                paddleRight.PhysicsBody.Velocity = new Vector2(0, 500);
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                paddleRight.PhysicsBody.Velocity = new Vector2(0, -500);
            else
                paddleRight.PhysicsBody.Velocity = new Vector2(0, 0);

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                paddleLeft.PhysicsBody.Velocity = new Vector2(0, 500);
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
                paddleLeft.PhysicsBody.Velocity = new Vector2(0, -500);
            else
                paddleLeft.PhysicsBody.Velocity = new Vector2(0, 0);

            if (gameIsOver && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                ResetGame();
                gameIsOver = false;
            }


        }

        public override void CollisionBegan(MJCollisionPair pair)
        {
            Console.WriteLine("Collision between: [" + pair.Body1.Parent.Name + ", " + pair.Body2.Parent.Name + "] began");
        }

        public override void CollisionEndded(MJCollisionPair pair)
        {
            Console.WriteLine("Collision between: [" + pair.Body1.Parent.Name + ", " + pair.Body2.Parent.Name + "] ended");
        }

        public override void IntersectionBegan(MJCollisionPair pair)
        {
            Console.WriteLine("Intersection between: [" + pair.Body1.Parent.Name + ", " + pair.Body2.Parent.Name + "] began");
            scoreKeeper.IncreaseScore(pair);
        }

        public override void IntersectionEnded(MJCollisionPair pair)
        {
            Console.WriteLine("Intersection between: [" + pair.Body1.Parent.Name + ", " + pair.Body2.Parent.Name + "] ended");
        }

        public void NotifyMaxScoreReached(string playerReachingMaxScore)
        {
            Console.WriteLine("Game Over!");
            gameIsOver = true;
        }

        public void NotifyLeftPlayerScored(int newScore)
        {
            Console.WriteLine("Hoorah! LeftPlayer now has " + newScore + " points");
            ResetAfterPointScored();
        }

        public void NotifyRightPlayerScored(int newScore)
        {
            Console.WriteLine("Hoorah! RightPlayer now has " + newScore + " points");
            ResetAfterPointScored();
        }

    }
}
