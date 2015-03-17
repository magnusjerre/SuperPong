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
    public class CollisionScene : MJScene
    {

        Player paddleLeft, paddleRight;
        MJSprite ball;
        Texture2D paddleTexture, ballTexture, triangleTexture;
        MJNode topWall, bottomWall, leftGoal, rightGoal;
        const float STATIC_MASS = 1000000;
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
        MJSprite ball1, ball2, triangle1, triangle2;

        public CollisionScene(ContentManager content) : base(content, "GameScene")
        {
            
        }

        public override void Initialize()
        {

            triangle1 = new MJSprite(triangleTexture);
            triangle1.Position = new Vector2(100, 200);
            List<Vector2> trianglePoints = new List<Vector2>();
            trianglePoints.Add(new Vector2(0, -25));
            trianglePoints.Add(new Vector2(-50, 25));
            trianglePoints.Add(new Vector2(50, 25));
            MJPhysicsBody triangle1Body = MJPhysicsBody.PolygonPathMJPhysicsBody(trianglePoints);
            triangle1Body.Bitmask = Bitmasks.WALL;
            triangle1Body.CollisionMask = Bitmasks.WALL;
            triangle1.AttachPhysicsBody(triangle1Body);

            triangle2 = new MJSprite(triangleTexture);
            triangle2.Position = new Vector2(400, 200);
            List<Vector2> trianglePoints2 = new List<Vector2>();
            trianglePoints2.Add(new Vector2(0, -25));
            trianglePoints2.Add(new Vector2(-50, 25));
            trianglePoints2.Add(new Vector2(50, 25));
            MJPhysicsBody triangle2Body = MJPhysicsBody.PolygonPathMJPhysicsBody(trianglePoints2);
            triangle2Body.Bitmask = Bitmasks.WALL;
            triangle2Body.CollisionMask = Bitmasks.WALL;
            triangle2.AttachPhysicsBody(triangle2Body);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            triangle1.Draw(spriteBatch);
            triangle2.Draw(spriteBatch);
        }

        public override void LoadContent()
        {
            paddleTexture = LoadTexture2D("Paddle");
            ballTexture = LoadTexture2D("ball");
            triangleTexture = LoadTexture2D("triangle");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            triangle1.Update(gameTime);
            triangle2.Update(gameTime);
            triangle1.PhysicsBody.Update(gameTime);
            triangle2.PhysicsBody.Update(gameTime);

            if (MJIntersection.PolygonsIntersect(triangle1.PhysicsBody, triangle2.PhysicsBody).Intersects)
            {
                Console.WriteLine("collision");
                triangle1.ColorTint = Color.Red;
            }
            else
            {
                Console.WriteLine("no-collision");
                triangle1.ColorTint = Color.White;
            }
        }
    }
}
