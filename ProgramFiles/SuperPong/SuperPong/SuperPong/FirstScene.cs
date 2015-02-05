using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SuperPong.MJFrameWork;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperPong
{
    public class FirstScene : MJScene
    {

        Texture2D ballTexture;
        MJPhysicsBody body, body2;

        public FirstScene(ContentManager content)
            : base(content, "FirstScene")
        {
            
        }

        public override void LoadContent()
        {
            hasLoadedContent = true;
            ballTexture = content.Load<Texture2D>("ball");
            if (!HasChildNamed("ball"))
            {
                float speed = 1;
                MJSprite ball = new MJSprite(ballTexture);
                ball.Name = "ball";
                ball.Position = new Vector2(100, 100);
                body = MJPhysicsBody.CircularMJPhysicsBody(
                    ball.Size.X / 2.0f);
                //body.Velocity = new Vector2(speed, speed);
                //body.Acceleration = new Vector2(-5, 0);
                ball.AttachPhysicsBody(body);
                AddChild(ball);

                /*
                MJSprite ball2 = new MJSprite(ballTexture);
                ball2.Name = "ball2";
                ball2.ColorTint = Color.Green;
                ball2.Position = new Vector2(100 + ball.Size.X / 2,
                    100 + ball.Size.X);
                body2 = MJPhysicsBody.CircularMJPhysicsBody(
                    ball2.Size.X / 2.0f);
                body2.Velocity = new Vector2(-speed, -speed);
                AddChild(ball2);
                ball2.AttachPhysicsBody(body2);
                Console.WriteLine("NumberOfChildren: " + Children.Count);
                */


            }
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

           if (body.PointInsideBody(new Vector2(body.Parent.absoluteCoordinateSystem.Position.X + body.Radius, body.Parent.absoluteCoordinateSystem.Position.Y))) {
                Console.WriteLine("Inside");
            } else {
                Console.WriteLine("Outside");
            }
        }

    }
}
