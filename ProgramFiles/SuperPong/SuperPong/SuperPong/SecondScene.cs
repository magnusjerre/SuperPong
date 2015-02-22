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

        Texture2D circleT, rectangleT, polygonT, ballT, rectangleSmallT, shapeT;
        MJSprite circle1, circle2, rectangle1, rectangle2, polygon1, polygon2, ball1, ball2, ball3, rectangleSmall1, shape;
        
        public SecondScene(ContentManager content)
            : base(content, "SecondScene")
        {
            
        }

        public override void LoadContent()
        {
            circleT = LoadTexture2D("circle");
            rectangleT = LoadTexture2D("rectangle-tall");
            polygonT = LoadTexture2D("polygon");
            ballT = LoadTexture2D("ball");
            rectangleSmallT = LoadTexture2D("rectangle-small");
            shapeT = LoadTexture2D("shape");
        }

        public override void Initialize()
        {
            AttachPhysicsManager(MJPhysicsManager.getInstance());

            ball1 = new MJSprite(ballT);
            ball1.Position = new Vector2(180, 200);

            
            shape = new MJSprite(shapeT);
            shape.Origin = new Vector2(0, 0);
            List<Vector2> path = new List<Vector2>();
            path.Add(new Vector2(0,0));
            path.Add(new Vector2(0, 50));
            path.Add(new Vector2(50, 100));
            path.Add(new Vector2(150, 100));
            path.Add(new Vector2(200, 50));
            path.Add(new Vector2(200, 0));
            path.Add(new Vector2(150, 50));
            path.Add(new Vector2(50, 50));
            MJPhysicsBody shapeBody = MJPhysicsBody.PolygonPathMJPhysicsBody(path);
            shape.AttachPhysicsBody(shapeBody);
            shapeBody.Mass = 10000000;
            shapeBody.Bitmask = 1;
            shapeBody.CollisionMask = 1;
            shape.Position = new Vector2(400, 200);
            shape.Alpha = 0.5f;
            shape.Rotation = 1.57f;
            
            MJPhysicsBody ball1Body = MJPhysicsBody.CircularMJPhysicsBody(ball1.Size.X / 2);
            ball1Body.Bitmask = 1;
            ball1Body.CollisionMask = 1;
            ball1.AttachPhysicsBody(ball1Body);

            ball1Body.Velocity = new Vector2(60, 30);

            AddChild(ball1);
            AddChild(shape);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
