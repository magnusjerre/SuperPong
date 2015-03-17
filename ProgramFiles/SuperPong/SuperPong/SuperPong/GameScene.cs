using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperPong.MJFrameWork;
using SuperPong.Powerups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong
{
    public class GameScene : MJScene, ScoreObserver, ResetGame, PointReset
    {

        Player paddleLeft, paddleRight;
        MJSprite ball;
        Texture2D paddleTexture, ballTexture;
        MJNode topWall, bottomWall, leftGoal, rightGoal;
        int height = 576, width = 1024;
        Vector2 wallSize, goalSize;
        ScoreKeeper scoreKeeper;
        int maxScore = 5;
        Vector2 initialBallPosition;
        SpriteFont font;
        ScoreFont scoreFont;
        Boolean gameIsOver = false;
        BallVelocityManager ballManager;
        PlayerCreator playerCreator;
        Vector2 moveDown = new Vector2(0, 1), moveUp = new Vector2(0, -1);
        PowerupManager powerupManager;

        public GameScene(ContentManager content) : base(content, "GameScene")
        {
            playerCreator = new PlayerCreator(new Vector2(100, height / 2), new Vector2(width - 100, height / 2));
            initialBallPosition = new Vector2(width / 2, height / 2);
            wallSize = new Vector2(width, 100);
            goalSize = new Vector2(100, height);
            powerupManager = new PowerupManager(this, content, width, height);
        }

        public override void Initialize()
        {
            AttachPhysicsManager(MJPhysicsManager.getInstance());
            paddleLeft = playerCreator.CreatePlayer1();
            AddChild(paddleLeft);
            paddleRight = playerCreator.CreatePlayer2();
            AddChild(paddleRight);

            ball = new MJSprite(ballTexture);
            ball.Name = "Ball";
            ball.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(ball.Size.X / 2));
            ball.PhysicsBody.Bitmask = Bitmasks.BALL;
            ball.PhysicsBody.CollisionMask = Bitmasks.WALL | Bitmasks.PADDLE;
            ball.PhysicsBody.IntersectionMask = Bitmasks.GOAL;
            ball.Position = new Vector2(width / 2, height / 2);
            AddChild(ball);

            topWall = new MJNode();
            topWall.Name = "TopWall";
            topWall.Position = new Vector2(0, 0);
            topWall.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(wallSize, new Vector2(0, 1)));   //Origin is at left bottom corner
            topWall.PhysicsBody.IsStatic = true;
            topWall.PhysicsBody.Bitmask = Bitmasks.WALL;
            topWall.PhysicsBody.CollisionMask = Bitmasks.BALL | Bitmasks.POWERUP;
            AddChild(topWall);

            bottomWall = new MJNode();
            bottomWall.Name = "BottomWall";
            bottomWall.Position = new Vector2(0, height);
            bottomWall.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(wallSize, new Vector2(0,0))); //Origin at top right corner
            bottomWall.PhysicsBody.IsStatic = true;
            bottomWall.PhysicsBody.Bitmask = Bitmasks.WALL;
            bottomWall.PhysicsBody.CollisionMask = Bitmasks.BALL | Bitmasks.POWERUP;
            AddChild(bottomWall);

            leftGoal = new MJNode();
            leftGoal.Name = "LeftGoal";
            leftGoal.Position = new Vector2(0, 0);
            leftGoal.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(goalSize, new Vector2(1, 0)));
            leftGoal.PhysicsBody.Bitmask = Bitmasks.GOAL;
            leftGoal.PhysicsBody.IntersectionMask = Bitmasks.BALL | Bitmasks.POWERUP;
            AddChild(leftGoal);

            rightGoal = new MJNode();
            rightGoal.Name = "RightGoal";
            rightGoal.Position = new Vector2(width, 0);
            rightGoal.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(goalSize, new Vector2(0, 0)));
            rightGoal.PhysicsBody.Bitmask = Bitmasks.GOAL;
            rightGoal.PhysicsBody.IntersectionMask = Bitmasks.BALL | Bitmasks.POWERUP;
            
            AddChild(rightGoal);

            scoreFont = new ScoreFont(new Vector2(width, height), font);

            ballManager = new BallVelocityManager(ball.PhysicsBody);

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            scoreFont.Draw(spriteBatch);
        }

        public override void LoadContent()
        {
            paddleTexture = LoadTexture2D("Paddle");
            playerCreator.LoadTextures(paddleTexture, paddleTexture);
            ballTexture = LoadTexture2D("ball");
            font = content.Load<SpriteFont>("TheSpriteFont");
            powerupManager.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            powerupManager.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                paddleRight.Move(moveDown);
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                paddleRight.Move(moveUp);
            else if (Keyboard.GetState().IsKeyDown(Keys.Q))
                powerupManager.UsePowerup(paddleLeft, new Vector2(400, 250));
            else
                paddleRight.StopMove();

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                paddleLeft.Move(moveDown);
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
                paddleLeft.Move(moveUp);
            else if (Keyboard.GetState().IsKeyDown(Keys.M))
                powerupManager.UsePowerup(paddleRight, new Vector2(400, 250));
            else
                paddleLeft.StopMove();
            
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

        public override void CollisionEnded(MJCollisionPair pair)
        {
            Console.WriteLine("Collision between: [" + pair.Body1.Parent.Name + ", " + pair.Body2.Parent.Name + "] ended");
            ballManager.CollisionEnded(pair);
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
            ResetAfterPoint();
        }

        public void NotifyRightPlayerScored(int newScore)
        {
            Console.WriteLine("Hoorah! RightPlayer now has " + newScore + " points");
            ResetAfterPoint();
        }

        public void ResetGame()
        {
            ResetAfterPoint();
            scoreKeeper.Reset();
            scoreFont.ResetGame();
            ballManager.ResetGame();
            powerupManager.ResetGame();
        }

        public void ResetAfterPoint()
        {
            ball.Position = initialBallPosition;
            ball.PhysicsBody.Acceleration = new Vector2();
            paddleLeft.ResetPoint();
            paddleRight.ResetPoint();
            ballManager.ResetAfterPoint();
            powerupManager.ResetPoint();
        }
    }
}
