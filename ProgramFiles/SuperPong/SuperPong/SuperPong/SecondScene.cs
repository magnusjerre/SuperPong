using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using SuperPong.MJFrameWork;

namespace SuperPong
{
    class SecondScene : MJScene
    {

        Texture2D circleT, rectangleT, polygonT, ballT, rectangleSmallT;
        MJSprite circle1, circle2, rectangle1, rectangle2, polygon1, polygon2, ball1, ball2, ball3, rectangleSmall1;
        
        public SecondScene(ContentManager content)
            : base(content, "SecondScene")
        {
            
        }

        public override void LoadContent()
        {
            circleT = content.Load<Texture2D>("circle");
            rectangleT = content.Load<Texture2D>("rectangle-tall");
            polygonT = content.Load<Texture2D>("polygon");
            ballT = content.Load<Texture2D>("ball");
            rectangleSmallT = content.Load<Texture2D>("rectangle-small");
        }

        public override void Initialize()
        {
            physicsManager = new MJPhysicsManager();
            physicsManager.Listener = this;

            ball1 = new MJSprite(ballT);
            ball1.Position = new Vector2(200, 200);

            ball2 = new MJSprite(ballT);
            ball2.Position = new Vector2(100, 100);

            MJPhysicsBody ball1Body = MJPhysicsBody.CircularMJPhysicsBody(ball1.Size.X / 2);
            ball1Body.Bitmask = 1;
            ball1Body.CollisionMask = 1;
            ball1.AttachPhysicsBody(ball1Body);

            MJPhysicsBody ball2Body = MJPhysicsBody.CircularMJPhysicsBody(ball2.Size.X / 2);
            ball2Body.Bitmask = 1;
            ball2Body.CollisionMask = 1;
            ball2.AttachPhysicsBody(ball2Body);

            ball1Body.Velocity = new Vector2(-30, -30);

            physicsManager.allBodies.Add(ball1Body);
            physicsManager.allBodies.Add(ball2Body);

            AddChild(ball1);
            AddChild(ball2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
