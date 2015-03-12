using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SuperPong.MJFrameWork;

namespace SuperPong
{
    public class PlayerCreator
    {
        protected static int STATIC_MASS = 1000000;
        protected Texture2D player1Texture, player2Texture;

        public PlayerCreator()
        {

        }

        public Player CreatePlayer1()
        {
            MJSprite paddleLeftSprite = new MJSprite(player1Texture);   //Points right
            paddleLeftSprite.Name = "PaddleLeft";
            paddleLeftSprite.origin = new Vector2(0.5f, 0.5f);
            paddleLeftSprite.AttachPhysicsBody(MJPhysicsBody.PolygonPathMJPhysicsBody(generatePlayer1Shape()));
            paddleLeftSprite.PhysicsBody.Mass = STATIC_MASS;
            paddleLeftSprite.PhysicsBody.Bitmask = Bitmasks.PADDLE;
            paddleLeftSprite.PhysicsBody.CollisionMask = Bitmasks.BALL;
            paddleLeftSprite.PhysicsBody.IntersectionMask = Bitmasks.POWERUP;
            paddleLeftSprite.Position = new Vector2(100, 500);
            Player leftPlayer = new Player(paddleLeftSprite);
            leftPlayer.Name = "PaddleLeft";
            return leftPlayer;
        }

        private List<Vector2> generatePlayer1Shape()
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

        public Player CreatePlayer2()
        {
            MJSprite paddleRight = new MJSprite(player2Texture);  //Points left
            paddleRight.Name = "PaddleRight";
            paddleRight.origin = new Vector2(0.5f, 0.5f);
            paddleRight.SEffects = SpriteEffects.FlipHorizontally;
            paddleRight.AttachPhysicsBody(MJPhysicsBody.PolygonPathMJPhysicsBody(generatePlayer2Shape()));
            paddleRight.PhysicsBody.Mass = STATIC_MASS;
            paddleRight.PhysicsBody.Bitmask = Bitmasks.PADDLE;
            paddleRight.PhysicsBody.CollisionMask = Bitmasks.BALL;
            paddleRight.PhysicsBody.IntersectionMask = Bitmasks.POWERUP;
            paddleRight.Position = new Vector2(1800, 550);
            Player rightPlayer = new Player(paddleRight);
            rightPlayer.Name = "PaddleRight";
            return rightPlayer;
        }

        private List<Vector2> generatePlayer2Shape()
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

        public void LoadTextures(Texture2D player1Texture, Texture2D player2Texture)
        {
            this.player1Texture = player1Texture;
            this.player2Texture = player2Texture;
        }
    }
}
