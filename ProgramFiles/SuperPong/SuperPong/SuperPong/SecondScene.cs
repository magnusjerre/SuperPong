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

        Texture2D circleT, rectangleT, polygonT, ballT, rectangleSmallT;
        MJSprite circle1, circle2, rectangle1, rectangle2, polygon1, polygon2, ball1, rectangleSmall1;
        Vector2 point = new Vector2(300, 250);
        
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

            //circle1 = new MJSprite(circleT);
            rectangle1 = new MJSprite(rectangleT);
            rectangleSmall1 = new MJSprite(rectangleSmallT);
            rectangleSmall1.Alpha = 0.5f;
            //polygon1 = new MJSprite(polygonT);
            //ball1 = new MJSprite(ballT);
            //polygon1.ColorTint = Color.Gray;
            //polygon1.Alpha = 0.5f;
            
            //circle1.Position = new Vector2(450, 120);
            rectangle1.Position = new Vector2(300, 200);
            rectangleSmall1.Position = point;
            //rectangle1.Position = new Vector2(300, 250);
            //polygon1.Position = new Vector2(450, 300);
            //ball1.Position = point;

            //MJPhysicsBody circle1Body = MJPhysicsBody.CircularMJPhysicsBody(circle1.Size.X / 2);
            //circle1Body.Velocity = new Vector2(0, 20);
            //circle1.AttachPhysicsBody(circle1Body);


            MJPhysicsBody rectangle1Body = MJPhysicsBody.RectangularMJPhysicsBody(rectangle1.Size, new Vector2(0.5f, 0.5f));
            //rectangle1Body.Velocity = new Vector2(10, 10);
            rectangle1.AttachPhysicsBody(rectangle1Body);

            MJPhysicsBody rectSmallBody = MJPhysicsBody.RectangularMJPhysicsBody(rectangleSmall1.Size, new Vector2(0.5f, 0.5f));
            //rectSmallBody.Velocity = new Vector2(20, 0);
            rectangleSmall1.AttachPhysicsBody(rectSmallBody);

            List<Vector3> polygonPoints = new List<Vector3>();
            polygonPoints.Add(new Vector3(150, 0, 0));
            polygonPoints.Add(new Vector3(150, 100, 0));
            polygonPoints.Add(new Vector3(-150, 100, 0));
            polygonPoints.Add(new Vector3(-150, 0, 0));
            polygonPoints.Add(new Vector3(-75, -100, 0));
            polygonPoints.Add(new Vector3(75, -100, 0));

            //MJPhysicsBody polygon1Body = MJPhysicsBody.PolygonPathMJPhysicsBody(polygonPoints);
            //polygon1.AttachPhysicsBody(polygon1Body);
            

            //AddChild(circle1);
            AddChild(rectangle1);
            AddChild(rectangleSmall1);
            //AddChild(polygon1);
            //AddChild(ball1);
            
        }

        public override void Update(GameTime gameTime)
        {
            
            
            base.Update(gameTime);
            /*
            Console.WriteLine("Rectangle1");
            int i = 0;
            foreach (Vector3 v in rectangle1.PhysicsBody.PolygonPathTransformed)
            {
                Console.WriteLine("i: " + i + ", X: " + v.X + ", Y: " + v.Y + ", v.Z: " + v.Z);
                i++;
            }

            Console.WriteLine("Polgon1");
            i = 0;
            foreach (Vector3 v in polygon1.PhysicsBody.PolygonPathTransformed)
            {
                Console.WriteLine("i: " + i + ", X: " + v.X + ", Y: " + v.Y + ", v.Z: " + v.Z);
                i++;
            }*/
            /*
            if (rectangle1.PhysicsBody.PointInsidePol(point))
            {
                rectangle1.ColorTint = Color.Red;
            }*/
            /* 
            float x = 301;// +polygon1.Size.X / 2;
            float y = 400;// +polygon1.Size.Y / 2;
            if (polygon1.PhysicsBody.PointInsidePol(new Vector2(x,y)))
            {
                polygon1.ColorTint = Color.Green;
            }*/
            
            /*
            foreach (Vector3 point in polygon1.PhysicsBody.PolygonPathTransformed) {
                Vector2 p = new Vector2(point.X, point.Y);
                if (rectangle1.PhysicsBody.PointInsidePol(p)) {
                    Console.WriteLine("cake");
                    polygon1.ColorTint = Color.Green;
                    break;
                }
            }*/
            /*
            if (rectangleSmall1.PhysicsBody.Collides(polygon1.PhysicsBody))
            {
                polygon1.ColorTint = Color.Green;
            }
            else
            {
                polygon1.ColorTint = Color.Gray;
            }*/
            /*
            if (rectangleSmall1.PhysicsBody.Collides(rectangle1.PhysicsBody))
            {
                rectangleSmall1.Alpha = 1.0f;
            }
            else
            {
                //rectangleSmall1.Alpha = 0.5f;
            }*/


            Vector2 a1 = new Vector2(100, 100);
            Vector2 a2 = new Vector2(100, 150);
            Vector2 b1 = new Vector2(90, 140);
            Vector2 b2 = new Vector2(105, 180);
            if (rectangleSmall1.PhysicsBody.MJLinesCrossVertical(a1, a2, b1, b2, 0.5f))
                Console.WriteLine("cross");

        }
    }
}
