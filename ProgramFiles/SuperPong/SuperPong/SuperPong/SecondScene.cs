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

            Vector2 a1 = new Vector2(100, 50);
            Vector2 a2 = new Vector2(100, 200);
            Vector2 b1 = new Vector2(100, 100);
            Vector2 b2 = new Vector2(300, 100);
            Vector2 c1 = new Vector2(50, 150);
            Vector2 c2 = new Vector2(200, 150);
            Vector2 d1 = new Vector2(250, 150);
            Vector2 d2 = new Vector2(350, 50);
            Vector2 e1 = new Vector2(350, 150);
            Vector2 e2 = new Vector2(350, 300);
            Vector2 f1 = new Vector2(450, 150);
            Vector2 f2 = new Vector2(700, 150);
            Vector2 g1 = new Vector2(500, 150);
            Vector2 g2 = new Vector2(600, 150);
            Vector2 h1 = new Vector2(500, 200);
            Vector2 h2 = new Vector2(650, 200);
            Vector2 i1 = new Vector2(350, 400);
            Vector2 i2 = new Vector2(400, 300);
            Vector2 j1 = new Vector2(350, 400);
            Vector2 j2 = new Vector2(550, 400);
            Vector2 k1 = new Vector2(300, 450);
            Vector2 k2 = new Vector2(500, 450);
            Vector2 l1 = new Vector2(150, 500);
            Vector2 l2 = new Vector2(350, 400);
            Vector2 m1 = new Vector2(150, 350);
            Vector2 m2 = new Vector2(200, 700);
            Vector2 n1 = new Vector2(750, 150);
            Vector2 n2 = new Vector2(800, 150);
            Vector2 o1 = new Vector2(350, 250);
            Vector2 o2 = new Vector2(350, 350);

            Vector2 p1 = new Vector2(600, 300);
            Vector2 p2 = new Vector2(700, 400);
            Vector2 q1 = new Vector2(750, 450);
            Vector2 q2 = new Vector2(850, 550);
            Vector2 r1 = new Vector2(650, 350);
            Vector2 r2 = new Vector2(800, 500);
            Vector2 s1 = new Vector2(900, 200);
            Vector2 s2 = new Vector2(850, 250);
            Vector2 t1 = new Vector2(850, 250);
            Vector2 t2 = new Vector2(800, 300);
            
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

        }
    }
}
