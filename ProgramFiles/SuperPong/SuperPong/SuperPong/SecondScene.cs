using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SuperPong.MJFrameWork;

namespace SuperPong
{
    class SecondScene : MJScene
    {

        Texture2D circleT, rectangleT, polygonT;
        MJSprite circle1, circle2, rectangle1, rectangle2, polygon1, polygon2;
        
        public SecondScene(ContentManager content)
            : base(content, "SecondScene")
        {
            
        }

        public override void LoadContent()
        {
            circleT = content.Load<Texture2D>("circle");
            rectangleT = content.Load<Texture2D>("rectangle");
            polygonT = content.Load<Texture2D>("polygon");
        }

        public override void Initialize()
        {

            circle1 = new MJSprite(circleT);
            //rectangle1 = new MJSprite(rectangleT);
            polygon1 = new MJSprite(polygonT);

            circle1.Position = new Vector2(450, 120);
            //rectangle1.Position = new Vector2(450, 320);
            polygon1.Position = new Vector2(450, 400);

            MJPhysicsBody circle1Body = MJPhysicsBody.CircularMJPhysicsBody(circle1.Size.X / 2);
            circle1Body.Velocity = new Vector2(0, 20);
            circle1.AttachPhysicsBody(circle1Body);


            //MJPhysicsBody rectangle1Body = MJPhysicsBody.RectangularMJPhysicsBody(rectangle1.Size, new Vector2(0.5f, 0.5f));
            //rectangle1.AttachPhysicsBody(rectangle1Body);

            List<Vector3> polygonPoints = new List<Vector3>();
            polygonPoints.Add(new Vector3(150, 0, 0));
            polygonPoints.Add(new Vector3(150, 100, 0));
            polygonPoints.Add(new Vector3(-150, 100, 0));
            polygonPoints.Add(new Vector3(-150, 0, 0));
            polygonPoints.Add(new Vector3(-75, -100, 0));
            polygonPoints.Add(new Vector3(75, -100, 0));

            MJPhysicsBody polygon1Body = MJPhysicsBody.PolygonPathMJPhysicsBody(polygonPoints);
            polygon1.AttachPhysicsBody(polygon1Body);

            AddChild(circle1);
            //AddChild(rectangle1);
            AddChild(polygon1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (circle1.PhysicsBody.Collides(polygon1.PhysicsBody))
            {
                polygon1.ColorTint = Color.Green;
            }
            else
            {
                polygon1.ColorTint = Color.White;
            }
        }
    }
}
