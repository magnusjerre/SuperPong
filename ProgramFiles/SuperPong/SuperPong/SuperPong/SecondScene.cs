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
            ball1.Position = new Vector2(300, 200 - ball1.Size.X / 2);

            ball2 = new MJSprite(ballT);
            ball2.Position = new Vector2(300, 200 + ball2.Size.X / 2);
            ball2.ColorTint = Color.Green;

            ball3 = new MJSprite(ballT);
            ball3.Position = new Vector2(500, 200);
            ball3.ColorTint = Color.Yellow;

            MJPhysicsBody ball1Body = MJPhysicsBody.CircularMJPhysicsBody(ball1.Size.X / 2);
            ball1.AttachPhysicsBody(ball1Body);

//            ball1.Update(new GameTime(new TimeSpan(0, 0, 0, 1), new TimeSpan(0, 0, 0, 0, 17)));

            MJPhysicsBody ball2Body = MJPhysicsBody.CircularMJPhysicsBody(ball2.Size.X / 2);
            ball2.AttachPhysicsBody(ball2Body);

            MJPhysicsBody ball3Body = MJPhysicsBody.CircularMJPhysicsBody(ball3.Size.X / 2);
            ball3.AttachPhysicsBody(ball3Body);


            //ball1Body.Velocity = new Vector2(10, 0);
            //ball2Body.Velocity = new Vector2(-10, -10f);
            ball3Body.Velocity = new Vector2(-30, 0);
            

            AddChild(ball1);
            AddChild(ball2);
            AddChild(ball3);
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

            if (!collided) {
                if (MJCollision.Intersects(ball1.PhysicsBody, ball2.PhysicsBody))
                {
                    //collided = true;
                    Vector2 normalVector = ball1.Position - ball2.Position;
                    Vector2 unitNormal = normalVector / normalVector.Length();
                    Vector2 unitTangent = new Vector2(-unitNormal.Y, unitNormal.X);
                    Console.WriteLine("unitNormal: " + unitNormal);
                    MJPhysicsManager.BounceObjects(ball1.PhysicsBody, ball2.PhysicsBody, unitNormal, unitTangent);
                }

                if (MJCollision.Intersects(ball1.PhysicsBody, ball3.PhysicsBody))
                {
                    //collided = true;
                    Vector2 normalVector = ball1.Position - ball3.Position;
                    Vector2 unitNormal = normalVector / normalVector.Length();
                    Vector2 unitTangent = new Vector2(-unitNormal.Y, unitNormal.X);
                    Console.WriteLine("unitNormal: " + unitNormal);
                    MJPhysicsManager.BounceObjects(ball1.PhysicsBody, ball3.PhysicsBody, unitNormal, unitTangent);
                }
                if (MJCollision.Intersects(ball3.PhysicsBody, ball2.PhysicsBody))
                {
                    //collided = true;
                    Vector2 normalVector = ball3.Position - ball2.Position;
                    Vector2 unitNormal = normalVector / normalVector.Length();
                    Vector2 unitTangent = new Vector2(-unitNormal.Y, unitNormal.X);
                    Console.WriteLine("unitNormal: " + unitNormal);
                    MJPhysicsManager.BounceObjects(ball3.PhysicsBody, ball2.PhysicsBody, unitNormal, unitTangent);
                }
            } else {
                if (!MJCollision.Intersects(ball1.PhysicsBody, ball2.PhysicsBody)) {
                    collided = false;
                }
                if (!MJCollision.Intersects(ball1.PhysicsBody, ball3.PhysicsBody))
                {
                    collided = false;
                }
                if (!MJCollision.Intersects(ball3.PhysicsBody, ball2.PhysicsBody))
                {
                    collided = false;
                }
            }
                

        }
    }
}
