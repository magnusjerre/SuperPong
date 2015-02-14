using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperPong.MJFrameWork;
using Microsoft.Xna.Framework;

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

        [TestMethod]
        public void TestOverlapsHorizontal()
        {
            float delta = 0.2f;
            Assert.IsTrue(MJCollision.MJHorizontalOverlap(f1, f2, g1, g2, delta));
            Assert.IsTrue(MJCollision.MJHorizontalOverlap(g1, g2, f1, f2, delta));
            Assert.IsTrue(MJCollision.MJHorizontalOverlap(g1, g2, g2, g1, delta));
            Assert.IsFalse(MJCollision.MJHorizontalOverlap(f1, f2, h1, h2, delta));
            Assert.IsFalse(MJCollision.MJHorizontalOverlap(f1, f2, n1, n2, delta));
            Assert.IsFalse(MJCollision.MJHorizontalOverlap(b1, b2, a1, a2, delta));
        }

        [TestMethod]
        public void TestOverlapsVertical()
        {
            float delta = 0.2f;
            Assert.IsTrue(MJCollision.MJVerticalOverlap(e1, e2, o1, o2, delta));
            Assert.IsTrue(MJCollision.MJVerticalOverlap(e1, e2, e1, e2, delta));
            Assert.IsTrue(MJCollision.MJVerticalOverlap(e1, e2, e2, e1, delta));
            Assert.IsTrue(MJCollision.MJVerticalOverlap(e1, e2, u1, u2, delta));
            Assert.IsTrue(MJCollision.MJVerticalOverlap(u1, u2, e1, e2, delta));
            Assert.IsFalse(MJCollision.MJVerticalOverlap(v1, v2, w1, w2, delta));
            Assert.IsFalse(MJCollision.MJVerticalOverlap(v1, v2, x1, x2, delta));
            Assert.IsFalse(MJCollision.MJVerticalOverlap(a1, a2, b1, b2, delta));
        }

        [TestMethod]
        public void TestOverlapsSlanted()
        {
            float delta = 0.2f;
            Assert.IsTrue(MJCollision.slantedLinesOverlaps(aa1, aa2, bb1, bb2, delta));
            Assert.IsTrue(MJCollision.slantedLinesOverlaps(aa1, aa2, aa1, aa2, delta));
            Assert.IsTrue(MJCollision.slantedLinesOverlaps(aa1, aa2, cc1, cc2, delta));
            Assert.IsTrue(MJCollision.slantedLinesOverlaps(cc1, cc2, aa1, aa2, delta));

            Assert.IsFalse(MJCollision.slantedLinesOverlaps(p1, p2, q1, q2, delta));
            Assert.IsFalse(MJCollision.slantedLinesOverlaps(p1, p2, d1, d2, delta));
            Assert.IsFalse(MJCollision.slantedLinesOverlaps(p1, p2, a1, a2, delta));
            Assert.IsFalse(MJCollision.slantedLinesOverlaps(p1, p2, b1, b2, delta));
            Assert.IsFalse(MJCollision.slantedLinesOverlaps(dd1, dd2, cc1, cc2, delta));
        }


    }
}
