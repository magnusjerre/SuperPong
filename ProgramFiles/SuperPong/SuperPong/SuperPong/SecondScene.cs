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
        Vector2 point = new Vector2(300, 200);
        int i = 0;
        int millisecondsPress = 200;
        int elapsed = 0;
        Boolean pressed = false;
        Boolean collided = false;
        
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
            ball1 = new MJSprite(ballT);
            ball1.Position = new Vector2(400, 200 - ball1.Size.X / 2);

            rectangle1 = new MJSprite(rectangleT);
            rectangle1.Position = new Vector2(200, 200);

            MJPhysicsBody ball1Body = MJPhysicsBody.CircularMJPhysicsBody(ball1.Size.X / 2);
            ball1Body.Bitmask = 1;
            ball1.AttachPhysicsBody(ball1Body);

            MJPhysicsBody rect1Body = MJPhysicsBody.RectangularMJPhysicsBody(rectangle1.Size, new Vector2(0.5f, 0.5f));
            rect1Body.Bitmask = 2;
            rectangle1.AttachPhysicsBody(rect1Body);

            ball1Body.CollisionMask = rect1Body.Bitmask;

            ball1Body.Velocity = new Vector2(-30, 10);         

            AddChild(ball1);
            AddChild(rectangle1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            /*if (!pressed) {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    pressed = true;
                    Vector2 pos = ball1.absoluteCoordinateSystem.Position;
                    MJPhysicsManager.ApplyForce(new Vector2(1000, 0), ball1.PhysicsBody);
                    MJPhysicsManager.ApplyRotation(new Vector2(100, 0), new Vector2(300, 270), ball1.PhysicsBody);
                }

                if (Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    pressed = true;
                    Vector2 pos = ball1.absoluteCoordinateSystem.Position;
                    MJPhysicsManager.ApplyForce(new Vector2(-1000, 0), ball1.PhysicsBody);
                }
            }
            else
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsed > millisecondsPress)
                {
                    elapsed = 0;
                    pressed = false;
                }
            }*/

            if (!collided && ball1.PhysicsBody.ShouldCheckForCollision(rectangle1.PhysicsBody.Bitmask))
            {
                int collisionResult = MJCollision.Intersects(ball1.PhysicsBody, rectangle1.PhysicsBody);
                if (collisionResult > -1)
                {
                    Vector2 unitNormal = rectangle1.PhysicsBody.PolygonPathNormals[collisionResult];
                    Vector2 unitTangent = new Vector2(-unitNormal.Y, unitNormal.X);
                    MJPhysicsManager.BounceObjects(ball1.PhysicsBody, rectangle1.PhysicsBody, unitNormal, unitTangent);
                    collided = true;
                }

            }

        }
    }
}
