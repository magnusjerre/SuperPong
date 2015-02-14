using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperPong.MJFrameWork;
using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace UnitTestSuperPong
{
    [TestClass]
    public class UnitTestMJCollision
    {
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
        Vector2 u1 = new Vector2(350, 200);
        Vector2 u2 = new Vector2(350, 250);
        Vector2 v1 = new Vector2(600, 550);
        Vector2 v2 = new Vector2(600, 650);
        Vector2 w1 = new Vector2(600, 700);
        Vector2 w2 = new Vector2(600, 750);
        Vector2 x1 = new Vector2(750, 600);
        Vector2 x2 = new Vector2(750, 650);
        Vector2 aa1 = new Vector2(100, 100);
        Vector2 aa2 = new Vector2(200, 200);
        Vector2 bb1 = new Vector2(150, 150);
        Vector2 bb2 = new Vector2(250, 250);
        Vector2 cc1 = new Vector2(50, 50);
        Vector2 cc2 = new Vector2(300, 300);
        Vector2 dd1 = new Vector2(50, 250);
        Vector2 dd2 = new Vector2(400, 250);
        Vector2 ee1 = new Vector2(350, 250);
        Vector2 ee2 = new Vector2(500, 250);
        Vector2 ff1 = new Vector2(100, 50);
        Vector2 ff2 = new Vector2(100, 200);
        Vector2 gg1 = new Vector2(300, 50);
        Vector2 gg2 = new Vector2(400, 150);

        [TestMethod]
        public void TestLineIsHorizontal()
        {
            float delta = 0.2f;
            Assert.IsTrue(MJCollision.LineIsHorizontal(b1, b2, delta));
            Assert.IsFalse(MJCollision.LineIsHorizontal(a1, a2, delta));
            Assert.IsFalse(MJCollision.LineIsHorizontal(d1, d2, delta));
            delta = 105f;
            Assert.IsTrue(MJCollision.LineIsHorizontal(d1, d2, delta));
        }

        [TestMethod]
        public void TestLineIsVertical()
        {
            float delta = 0.2f;
            Assert.IsTrue(MJCollision.LineIsVertical(a1, a2, delta));
            Assert.IsFalse(MJCollision.LineIsVertical(b1, b2, delta));
            Assert.IsFalse(MJCollision.LineIsVertical(d1, d2, delta));
            delta = 105f;
            Assert.IsTrue(MJCollision.LineIsVertical(d1, d2, delta));
        }
    
        [TestMethod]
        public void TestLinesIntersect()
        {
            float delta = 0.2f;
            //Horizontal with horizontal intersections
            Assert.IsTrue(MJCollision.LinesIntersect(f1, f2, f1, f2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(f1, f2, g1, g2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(g1, g2, f1, f2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(dd1, dd2, ee1, ee2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(ee1, ee2, dd1, dd2, delta));
            //Horizontal with horizontal non-intersections
            Assert.IsFalse(MJCollision.LinesIntersect(f1, f2, n1, n2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(f1, f2, h1, h2, delta));

            //Horizontal with vertical intersections
            Assert.IsTrue(MJCollision.LinesIntersect(b1, b2, a1, a2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(a1, a2, b1, b2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(c1, c2, a1, a2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(a1, a2, c1, c2, delta));
            //Horizontal with vertical non-intersections
            Assert.IsFalse(MJCollision.LinesIntersect(f1, f2, a1, a2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(a1, a2, f1, f2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(f1, f2, v1, v2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(v1, v2, f1, f2, delta));

            //Horizontal with slanted intersections
            Assert.IsTrue(MJCollision.LinesIntersect(b1, b2, d1, d2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(d1, d2, b1, b2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(i1, i2, j1, j2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(j1, j2, i1, i2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(dd1, dd2, cc1, cc2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(cc1, cc2, dd1, dd2, delta));
            //Horizontal with slanted non-intersections
            Assert.IsFalse(MJCollision.LinesIntersect(c1, c2, d1, d2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(d1, d2, c1, c2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(l1, l2, f1, f2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(f1, f2, l1, l2, delta));

            //Vertical with vertical intersections
            Assert.IsTrue(MJCollision.LinesIntersect(e1, e2, o1, o2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(o1, o2, e1, e2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(e1, e2, u1, u2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(u1, u2, e1, e2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(u1, u2, o1, o2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(o1, o2, u1, u2, delta));
            //Vertical with vertical non-intersections
            Assert.IsFalse(MJCollision.LinesIntersect(v1, v2, w1, w2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(w1, w2, v1, v1, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(v1, v2, x1, x2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(x1, x2, v1, v2, delta));

            //Vertical with slanted intersections
            Assert.IsTrue(MJCollision.LinesIntersect(ff1, ff2, cc1, cc2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(cc1, cc2, ff1, ff2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(ff1, ff2, aa1, aa2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(aa1, aa2, ff1, ff2, delta));
            //Vertical with slanted non-intersections
            Assert.IsFalse(MJCollision.LinesIntersect(ff1, ff2, bb1, bb2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(bb1, bb2, ff1, ff2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(a1, a2, d1, d2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(d1, d2, a1, a2, delta));

            //Slanted with slanted intersections
            Assert.IsTrue(MJCollision.LinesIntersect(m1, m2, l1, l2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(l1, l2, m1, m2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(l1, l2, i1, i2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(l1, l2, i1, i2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(aa1, aa2, bb1, bb2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(bb1, bb2, aa1, aa2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(aa1, aa2, cc1, cc2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(cc1, cc2, aa1, aa2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(t1, t2, s1, s2, delta));
            Assert.IsTrue(MJCollision.LinesIntersect(s1, s2, t1, t2, delta));

            //Slanted with slanted non-intersections
            Assert.IsFalse(MJCollision.LinesIntersect(p1, p2, q1, q2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(q1, q2, p1, p2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(p1, p2, t1, t2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(gg1, gg2, cc1, cc2, delta));
            Assert.IsFalse(MJCollision.LinesIntersect(cc1, cc2, gg1, gg2, delta));
        }

        [TestMethod]
        public void TestRectanglesIntersect()
        {
            MJRectangle rectangle1 = new MJRectangle(0, 0, 100, 100);
            MJRectangle rectangle2 = new MJRectangle(50, 50, 200, 100);
            MJRectangle rectangle3 = new MJRectangle(0, 0, 250, 200);
            MJRectangle rectangle4 = new MJRectangle(300, 50, 450, 150);

            Assert.IsTrue(MJCollision.RectanglesIntersect(rectangle1, rectangle1));
            Assert.IsTrue(MJCollision.RectanglesIntersect(rectangle1, rectangle2));
            Assert.IsTrue(MJCollision.RectanglesIntersect(rectangle2, rectangle1));
            Assert.IsTrue(MJCollision.RectanglesIntersect(rectangle2, rectangle3));
            Assert.IsTrue(MJCollision.RectanglesIntersect(rectangle3, rectangle2));
            Assert.IsFalse(MJCollision.RectanglesIntersect(rectangle1, rectangle4));
            Assert.IsFalse(MJCollision.RectanglesIntersect(rectangle4, rectangle1));
        }

        [TestMethod]
        public void TestCirclesIntersect()
        {
            float radius1 = 100;
            float radius2 = 50;
            Vector2 pos1 = new Vector2(200, 200);
            Vector2 pos2 = new Vector2(300, 300);

            Assert.IsTrue(MJCollision.CirclesIntersect(radius1, pos1, radius2, pos2));
            Assert.IsTrue(MJCollision.CirclesIntersect(radius2, pos2, radius1, pos1));
            Assert.IsFalse(MJCollision.CirclesIntersect(radius2, pos1, radius2, pos2));
        }

        [TestMethod]
        public void TestLineIntersectsCircle()
        {
            float radius1 = 100;
            float radius2 = 50;
            Vector2 point1 = new Vector2(100, 150);
            Vector2 point2 = new Vector2(250, 50);

            Vector2 h1 = new Vector2(100, 100);
            Vector2 h2 = new Vector2(200, 100);

            Vector2 s1 = new Vector2(150, 200);
            Vector2 s2 = new Vector2(400, 350);

            Assert.IsTrue(MJCollision.LineIntersectsCircle(h1, h2, radius1, point1));
            Assert.IsTrue(MJCollision.LineIntersectsCircle(s1, s2, radius1, point1));
            Assert.IsFalse(MJCollision.LineIntersectsCircle(h1, h2, radius2, point2));
            Assert.IsFalse(MJCollision.LineIntersectsCircle(s1, s2, radius2, point1));
        }

        [TestMethod]
        public void TestPointInsideCircle()
        {
            float radius = 100;
            Vector2 pos = new Vector2(200, 200);
            Vector2 point1 = new Vector2(250, 250);
            Vector2 point2 = new Vector2(200, 300);
            Vector2 point3 = new Vector2(500, 400);

            Assert.IsTrue(MJCollision.PointInsideCircle(radius, pos, point1));
            Assert.IsTrue(MJCollision.PointInsideCircle(radius, pos, point2));
            Assert.IsFalse(MJCollision.PointInsideCircle(radius, pos, point3));
        }

        [TestMethod]
        public void TestPointInsiderectangle()
        {
            MJRectangle rectangle = new MJRectangle(50, 50, 200, 150);
            Vector2 point1 = new Vector2(180, 140);
            Vector2 point2 = new Vector2(200, 150);
            Vector2 point3 = new Vector2(300, 100);

            Assert.IsTrue(MJCollision.PointInsideRectangle(rectangle, point1));
            Assert.IsTrue(MJCollision.PointInsideRectangle(rectangle, point2));
            Assert.IsFalse(MJCollision.PointInsideRectangle(rectangle, point3));
        }

        [TestMethod]
        public void TestPointOnLine()
        {
            float delta = 0.1f;
            Vector2 h1 = new Vector2(400, 600);
            Vector2 h2 = new Vector2(550, 600);
            Vector2 v1 = new Vector2(700, 450);
            Vector2 v2 = new Vector2(700, 600);
            Vector2 s1 = new Vector2(800, 550);
            Vector2 s2 = new Vector2(900, 450);

            Vector2 point1 = new Vector2(500, 600);
            Vector2 point2 = new Vector2(700, 500);
            Vector2 point3 = new Vector2(850, 500);
            Vector2 h1o = new Vector2(399, 600);
            Vector2 v1o = new Vector2(699, 450);
            Vector2 s1o = new Vector2(799, 550);

            Assert.IsTrue(MJCollision.PointOnLine(h1, h2, h1, delta));
            Assert.IsTrue(MJCollision.PointOnLine(h1, h2, point1, delta));
            Assert.IsTrue(MJCollision.PointOnLine(v1, v2, v1, delta));
            Assert.IsTrue(MJCollision.PointOnLine(v1, v2, point2, delta));
            Assert.IsTrue(MJCollision.PointOnLine(s1, s2, s1, delta));
            Assert.IsTrue(MJCollision.PointOnLine(s1, s2, point3, delta));

            Assert.IsFalse(MJCollision.PointOnLine(h1, h2, point2, delta));
            Assert.IsFalse(MJCollision.PointOnLine(h1, h2, h1o, delta));
            Assert.IsFalse(MJCollision.PointOnLine(v1, v2, point1, delta));
            Assert.IsFalse(MJCollision.PointOnLine(v1, v2, v1o, delta));
            Assert.IsFalse(MJCollision.PointOnLine(s1, s2, point2, delta));
            Assert.IsFalse(MJCollision.PointOnLine(s1, s2, s1o, delta));
        }

        [TestMethod]
        public void TestPointinsidePolygon()
        {
            MJRectangle boundingBox = new MJRectangle(150, 200, 400, 400);
            List<Vector2> path = new List<Vector2>();
            path.Add(new Vector2(200, 200));
            path.Add(new Vector2(400, 250));
            path.Add(new Vector2(350, 350));
            path.Add(new Vector2(250, 400));
            path.Add(new Vector2(150, 300));

            Vector2 point1 = new Vector2(300, 300); //Inside
            Vector2 point2 = new Vector2(400, 250); //Inside
            Vector2 point3 = new Vector2(500, 500); //Outside
            Vector2 point4 = new Vector2(401, 250); //Outside
            Vector2 point5 = new Vector2(100, 300); //Outside
            Vector2 point6 = new Vector2(200, 380); //Outside
            Vector2 point7 = new Vector2(200, 400); //Usbude
            
            Assert.IsTrue(MJCollision.PointInsidePolygon(boundingBox, path, point1));
            Assert.IsTrue(MJCollision.PointInsidePolygon(boundingBox, path, point2));
            Assert.IsFalse(MJCollision.PointInsidePolygon(boundingBox, path, point3));
            Assert.IsFalse(MJCollision.PointInsidePolygon(boundingBox, path, point4));
            Assert.IsFalse(MJCollision.PointInsidePolygon(boundingBox, path, point5));
            Assert.IsFalse(MJCollision.PointInsidePolygon(boundingBox, path, point6));
            Assert.IsTrue(MJCollision.PointInsidePolygon(boundingBox, path, point7));
        }

        [TestMethod]
        public void TestCollision()
        {
            MJNode node1 = new MJNode();
            node1.Position = new Vector2(250, 200);
            MJPhysicsBody rectangleBody1 = MJPhysicsBody.RectangularMJPhysicsBody(new Vector2(300, 200), new Vector2(0.5f, 0.5f));
            node1.AttachPhysicsBody(rectangleBody1);


            MJNode node2 = new MJNode();
            node2.Position = new Vector2(300, 150);
            List<Vector3> path = new List<Vector3>();
            path.Add(new Vector3(150, 0, 0));
            path.Add(new Vector3(300, 50, 0));
            path.Add(new Vector3(300, 200, 0));
            path.Add(new Vector3(150, 250, 0));
            path.Add(new Vector3(0, 200, 0));
            MJPhysicsBody polygonBody = MJPhysicsBody.PolygonPathMJPhysicsBody(path);
            node2.AttachPhysicsBody(polygonBody);

            MJNode circle1 = new MJNode();
            circle1.Position = new Vector2(650, 250);
            MJPhysicsBody circle1Body = MJPhysicsBody.CircularMJPhysicsBody(100);
            circle1.AttachPhysicsBody(circle1Body);

            MJNode circle2 = new MJNode();
            circle2.Position = new Vector2(200, 250);
            MJPhysicsBody circle2Body = MJPhysicsBody.CircularMJPhysicsBody(50);
            circle2.AttachPhysicsBody(circle2Body);

            GameTime gameTime = new GameTime(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 0, 0, 17));
            node1.Update(gameTime);
            node2.Update(gameTime);
            circle1.Update(gameTime);
            circle2.Update(gameTime);

            //Intersects with self
            Assert.IsTrue(MJCollision.Intersects(rectangleBody1, rectangleBody1));
            Assert.IsTrue(MJCollision.Intersects(polygonBody, polygonBody));
            Assert.IsTrue(MJCollision.Intersects(circle1Body, circle1Body));

            Assert.IsTrue(MJCollision.Intersects(rectangleBody1, polygonBody));
            Assert.IsTrue(MJCollision.Intersects(polygonBody, rectangleBody1));
            
            Assert.IsTrue(MJCollision.Intersects(circle2Body, rectangleBody1));
            Assert.IsTrue(MJCollision.Intersects(rectangleBody1, circle2Body));

            Assert.IsTrue(MJCollision.Intersects(polygonBody, circle1Body));
            Assert.IsTrue(MJCollision.Intersects(circle1Body, polygonBody));

            Assert.IsFalse(MJCollision.Intersects(circle1Body, rectangleBody1));
            Assert.IsFalse(MJCollision.Intersects(rectangleBody1, circle1Body));

            node2.Position = new Vector2(350, 200);
            node2.Update(gameTime);

            Assert.IsFalse(MJCollision.Intersects(rectangleBody1, polygonBody));
            Assert.IsFalse(MJCollision.Intersects(polygonBody, rectangleBody1));


        }
    
    }
}
