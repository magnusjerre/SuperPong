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
    public class GameScene : MJScene, ScoreObserver, ResetGame, PointReset, InputListener
    {

        Player paddleLeft, paddleRight;
        MJSprite ball;
        Texture2D paddleTexture, ballTexture;
        MJNode topWall, bottomWall, leftGoal, rightGoal;
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
        InputHandler inputHandler;
        public static int Height, Width;

        public GameScene(ContentManager content, int height, int width) : base(content, "GameScene")
        {
            Height = height;
            Width = width;
            playerCreator = new PlayerCreator(new Vector2(100, Height / 2), new Vector2(Width - 100, Height / 2));
            initialBallPosition = new Vector2(Width / 2, Height / 2);
            wallSize = new Vector2(width, 100);
            goalSize = new Vector2(100, height);
            powerupManager = new PowerupManager(this, content, Width, Height);
            inputHandler = new InputHandler(this, InputType.KEYBOARD);
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
            ball.Position = new Vector2(Width / 2, Height / 2);
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
            bottomWall.Position = new Vector2(0, Height);
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
            rightGoal.Position = new Vector2(Width, 0);
            rightGoal.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(goalSize, new Vector2(0, 0)));
            rightGoal.PhysicsBody.Bitmask = Bitmasks.GOAL;
            rightGoal.PhysicsBody.IntersectionMask = Bitmasks.BALL | Bitmasks.POWERUP;
            
            AddChild(rightGoal);

            scoreFont = new ScoreFont(new Vector2(Width, Height), font);

            ballManager = new BallVelocityManager(ball.PhysicsBody);

            scoreKeeper = new ScoreKeeper(maxScore);
            scoreKeeper.AddObserver(this);
            scoreKeeper.AddObserver(scoreFont);
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

            inputHandler.Update(gameTime);
        }

        public override void CollisionBegan(MJIntersection pair)
        {
            ballManager.Collision(pair);
        }

        public override void IntersectionBegan(MJIntersection pair)
        {
            scoreKeeper.IncreaseScore(pair);
        }

        public override void IntersectionEnded(MJIntersection pair)
        {
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

        public void MovePlayer(int playerNumber, Vector2 direction)
        {
            if (playerNumber == 1)
                paddleLeft.Move(direction);
            else
                paddleRight.Move(direction);
        }

        public void StopPlayerMovement(int playerNumber)
        {
            if (playerNumber == 1)
                paddleLeft.StopMove();
            else
                paddleRight.StopMove();
        }

        public void UsePowerup(int playerNumber)
        {
            if (playerNumber == 1)
                powerupManager.UsePowerup(paddleLeft, new Vector2(100, 100));
            else
                powerupManager.UsePowerup(paddleRight, new Vector2(200, 300));
        }

        public void MovePlayerStick(int playerNumber, Vector2 direction)
        {
            if (playerNumber == 1)
            {
                //Do something
            }
            else
            {
                //DO something else
            }
        }

        public void RestartGame()
        {
            ResetGame();
            gameIsOver = false;
        }
    }
}
