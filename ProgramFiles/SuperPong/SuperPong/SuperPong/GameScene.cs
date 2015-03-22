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

        Player player1, player2;
        Cursor player1Cursor, player2Cursor;
        public MJNode CursorLayer, HUDLayer, GameLayer;
        const String CURSORLAYERNAME = "CURSOR_LAYER", HUDLAYERNAME = "HUD_LAYER", GAMELAYERNAME = "GAME_LAYER";
        MJSprite ball;
        Texture2D paddleTexture, ballTexture, cursorTexture, replayBackgroundTexture, aButtonTexture, bButtonTexture;
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
        List<InputHandler> inputHandlers;
        InputHandler inputHandler;
        public static int Height, Width;
        MJSprite replayGameNode;

        public GameScene(int height, int width) : base("GameScene")
        {
            Height = height;
            Width = width;
            
        }

        public override void Initialize()
        {
            playerCreator = new PlayerCreator(new Vector2(100, Height / 2), new Vector2(Width - 100, Height / 2));
            initialBallPosition = new Vector2(Width / 2, Height / 2);
            powerupManager = new PowerupManager(this, Content, Width, Height);
            
            wallSize = new Vector2(Width, 100);
            goalSize = new Vector2(100, Height);
            
            inputHandlers = new List<InputHandler>();
            inputHandlers.Add(new InputHandler(this, 1));
            inputHandlers.Add(new InputHandler(this, 2));
            
            CursorLayer = new MJNode();
            CursorLayer.Name = CURSORLAYERNAME;
            
            HUDLayer = new MJNode();
            HUDLayer.Name = HUDLAYERNAME;
            
            GameLayer = new MJNode();
            GameLayer.Name = GAMELAYERNAME;
            
            AddChild(GameLayer);
            AddChild(HUDLayer);
            AddChild(CursorLayer);

            AttachPhysicsManager(MJPhysicsManager.getInstance());

            base.Initialize();

            player1 = playerCreator.CreatePlayer1();
            AddToGameLayer(player1);
            player2 = playerCreator.CreatePlayer2();
            AddToGameLayer(player2);

            ball = new MJSprite(ballTexture);
            ball.Name = "Ball";
            ball.AttachPhysicsBody(MJPhysicsBody.CircularMJPhysicsBody(ball.Size.X / 2));
            ball.PhysicsBody.Bitmask = Bitmasks.BALL;
            ball.PhysicsBody.CollisionMask = Bitmasks.WALL | Bitmasks.PADDLE;
            ball.PhysicsBody.IntersectionMask = Bitmasks.GOAL;
            ball.Position = new Vector2(Width / 2, Height / 2);
            AddToGameLayer(ball);

            topWall = new MJNode();
            topWall.Name = "TopWall";
            topWall.Position = new Vector2(0, 0);
            topWall.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(wallSize, new Vector2(0, 1)));   //Origin is at left bottom corner
            topWall.PhysicsBody.IsStatic = true;
            topWall.PhysicsBody.Bitmask = Bitmasks.WALL;
            topWall.PhysicsBody.CollisionMask = Bitmasks.BALL | Bitmasks.POWERUP;
            AddToGameLayer(topWall);

            bottomWall = new MJNode();
            bottomWall.Name = "BottomWall";
            bottomWall.Position = new Vector2(0, Height);
            bottomWall.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(wallSize, new Vector2(0, 0))); //Origin at top right corner
            bottomWall.PhysicsBody.IsStatic = true;
            bottomWall.PhysicsBody.Bitmask = Bitmasks.WALL;
            bottomWall.PhysicsBody.CollisionMask = Bitmasks.BALL | Bitmasks.POWERUP;
            AddToGameLayer(bottomWall);

            leftGoal = new MJNode();
            leftGoal.Name = "LeftGoal";
            leftGoal.Position = new Vector2(0, 0);
            leftGoal.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(goalSize, new Vector2(1, 0)));
            leftGoal.PhysicsBody.Bitmask = Bitmasks.GOAL;
            leftGoal.PhysicsBody.IntersectionMask = Bitmasks.BALL | Bitmasks.POWERUP;
            AddToGameLayer(leftGoal);

            rightGoal = new MJNode();
            rightGoal.Name = "RightGoal";
            rightGoal.Position = new Vector2(Width, 0);
            rightGoal.AttachPhysicsBody(MJPhysicsBody.RectangularMJPhysicsBody(goalSize, new Vector2(0, 0)));
            rightGoal.PhysicsBody.Bitmask = Bitmasks.GOAL;
            rightGoal.PhysicsBody.IntersectionMask = Bitmasks.BALL | Bitmasks.POWERUP;

            AddToGameLayer(rightGoal);

            scoreFont = new ScoreFont(new Vector2(Width, Height), font);

            ballManager = new BallVelocityManager(ball.PhysicsBody);

            scoreKeeper = new ScoreKeeper(maxScore);
            scoreKeeper.AddObserver(this);
            scoreKeeper.AddObserver(scoreFont);

            player1Cursor = new Cursor(cursorTexture);
            player1Cursor.ColorTint = Color.Green;
            player1Cursor.Position = player1.Position;
            AddToCursorLayer(player1Cursor);

            player2Cursor = new Cursor(cursorTexture);
            player2Cursor.ColorTint = Color.Blue;
            player2Cursor.Position = player2.Position;
            AddToCursorLayer(player2Cursor);

            replayGameNode = new MJSprite(replayBackgroundTexture);
            replayGameNode.Position = new Vector2(Width / 2, Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            scoreFont.Draw(spriteBatch);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            MJPhysicsManager.getInstance().Clear();
        }

        public override void LoadContent()
        {
            paddleTexture = LoadTexture2D("paddle");
            playerCreator.LoadTextures(paddleTexture, paddleTexture);
            ballTexture = LoadTexture2D("ball");
            cursorTexture = LoadTexture2D("stick-cursor");
            font = Content.Load<SpriteFont>("TheSpriteFont");
            powerupManager.LoadContent();
            replayBackgroundTexture = LoadTexture2D("replay");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            powerupManager.Update(gameTime);

            foreach (InputHandler inputHandler in inputHandlers)
            {
                inputHandler.Update(gameTime);
            }
        }

        public override void CollisionBegan(MJIntersection pair)
        {
            ballManager.Collision(pair);
        }

        public override void IntersectionBegan(MJIntersection pair)
        {
            if (pair.Body1.Bitmask == Bitmasks.BALL || pair.Body2.Bitmask == Bitmasks.BALL)
                scoreKeeper.IncreaseScore(pair);
        }

        public override void IntersectionEnded(MJIntersection pair)
        {
        }

        public void NotifyMaxScoreReached(string playerReachingMaxScore)
        {
            Console.WriteLine("Game Over!");
            gameIsOver = true;
            ball.Position = new Vector2(-1000, -1000);
            ball.PhysicsBody.Velocity = Vector2.Zero;
            AddToHudLayer(replayGameNode);
            powerupManager.NotifyGameOver();
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
            player1Cursor.Position = player1.Position;
            player2Cursor.Position = player2.Position;
        }

        public void ResetAfterPoint()
        {
            ball.Position = initialBallPosition;
            ball.PhysicsBody.Acceleration = new Vector2();
            player1.ResetPoint();
            player2.ResetPoint();
            ballManager.ResetAfterPoint();
            powerupManager.ResetPoint();
        }

        public void MovePlayer(int playerNumber, Vector2 direction)
        {
            if (playerNumber == 1)
                player1.Move(direction);
            else
                player2.Move(direction);
        }

        public void StopPlayerMovement(int playerNumber)
        {
            if (playerNumber == 1)
                player1.StopMove();
            else
                player2.StopMove();
        }

        public void MovePlayerStick(int playerNumber, Vector2 direction)
        {
            if (playerNumber == 1)
                player1Cursor.Move(direction);
            else
                player2Cursor.Move(direction);
        }

        public void RestartGame()
        {
            if (gameIsOver)
            {
                ResetGame();
                gameIsOver = false;
                replayGameNode.RemoveFromParent();
            }
        }

        public void AddToGameLayer(MJNode node)
        {
            GameLayer.AddChild(node);
        }

        public void AddToHudLayer(MJNode node)
        {
            HUDLayer.AddChild(node);
        }

        public void AddToCursorLayer(MJNode node)
        {
            CursorLayer.AddChild(node);
        }


        public void BackButtonPressed()
        {
            if (gameIsOver)
                MJSceneManager.GetInstance().PopScene();
        }


        public void AButtonPressed(int playerNumber)
        {
            if (gameIsOver)
                RestartGame();
            else
            {
                if (playerNumber == 1)
                    powerupManager.UsePowerup(player1, player1Cursor.absoluteCoordinateSystem.Position);
                else
                    powerupManager.UsePowerup(player2, player2Cursor.absoluteCoordinateSystem.Position);
            }

        }
    }
}
