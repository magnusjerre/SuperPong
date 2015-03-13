﻿using Microsoft.Xna.Framework;
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
        protected Vector2 leftPosition, rightPosition;

        public PlayerCreator(Vector2 leftPosition, Vector2 rightPosition)
        {
            this.leftPosition = leftPosition;
            this.rightPosition = rightPosition;
        }

        public Player CreatePlayer1()
        {
            MJSprite paddleLeftSprite = new MJSprite(player1Texture);   //Points right
            paddleLeftSprite.Name = "PaddleLeft";
            paddleLeftSprite.origin = new Vector2(0.5f, 0.5f);
            
            MJPhysicsBody body = MJPhysicsBody.PolygonPathMJPhysicsBody(generatePlayer1Shape());
            body.Mass = STATIC_MASS;
            body.Bitmask = Bitmasks.PADDLE;
            body.CollisionMask = Bitmasks.BALL;
            body.IntersectionMask = Bitmasks.POWERUP;
            
            Player leftPlayer = new Player(paddleLeftSprite, body, leftPosition);
            leftPlayer.Name = "PaddleLeft";
            leftPlayer.Position = leftPosition;
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
            MJPhysicsBody body = MJPhysicsBody.PolygonPathMJPhysicsBody(generatePlayer2Shape());
            body.Mass = STATIC_MASS;
            body.Bitmask = Bitmasks.PADDLE;
            body.CollisionMask = Bitmasks.BALL;
            body.IntersectionMask = Bitmasks.POWERUP;
            
            Player rightPlayer = new Player(paddleRight, body, rightPosition);
            rightPlayer.Name = "PaddleRight";
            rightPlayer.Position = rightPosition;
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
