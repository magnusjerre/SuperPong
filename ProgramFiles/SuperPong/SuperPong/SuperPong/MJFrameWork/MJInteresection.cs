using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork
{
    public class MJInteresection
    {
        public static MJIntersects Collides(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            if (BothBodiesAreCircles(body1, body2))
                return CirclesIntersect(body1, body2);
            if (OneBodyIsACircle(body1, body2))
                return CircleIntersectsPolygon(body1, body2);
            return new MJIntersects(false, new Vector2(0, 0));
        }

        private static Boolean BothBodiesAreCircles(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            return body1.Radius > -1 && body2.Radius > -1;
        }

        private static Boolean OneBodyIsACircle(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            if (body1.Radius != -1 && body2.Radius == -1)
                return true;
            if (body2.Radius != -1 && body1.Radius == -1)
                return true;
            return false;
        }

        private static MJIntersects CirclesIntersect(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            Vector2 body1Position = body1.Parent.absoluteCoordinateSystem.Position;
            Vector2 body2Position = body2.Parent.absoluteCoordinateSystem.Position;

            float dx = body2Position.X - body1Position.X;
            float dy = body2Position.Y - body1Position.Y;
            float distance = dx * dx + dy * dy;
            float maxDistance = (body1.Radius + body2.Radius) * (body1.Radius + body2.Radius);

            if (distance < maxDistance)
            {
                Vector2 normal = new Vector2(dx, dy);
                if (normal.Length() == 0)
                    return new MJIntersects(true, new Vector2(0, 0));
                
                Vector2 unitNormal = normal / normal.Length();
                return new MJIntersects(true, unitNormal);
            }

            return new MJIntersects(false, new Vector2(0, 0));
        }

        private static MJIntersects CircleIntersectsPolygon(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            MJPhysicsBody circleBody = body1.Radius != -1 ? body1 : body2;
            MJPhysicsBody polygonBody = body1.Radius != -1 ? body2 : body1;

            if (!CircleIntersectsRectangle(circleBody.Radius, circleBody.Parent.absoluteCoordinateSystem.Position, polygonBody.AxisAlignedBoundingBox))
                return new MJIntersects(false, new Vector2(0, 0));

            for (int i = 0; i < polygonBody.PolygonPathTransformed.Count; i++)
            {
                int next = (i + 1) % polygonBody.PolygonPathTransformed.Count;
                Vector2 a1 = polygonBody.PolygonPathTransformed[i];
                Vector2 a2 = polygonBody.PolygonPathTransformed[next];

                if (LineIntersectsCircle(a1, a2, circleBody.Radius, circleBody.Parent.absoluteCoordinateSystem.Position))
                    return new MJIntersects(true, polygonBody.PolygonPathNormals[i]);
            }

            foreach (Vector2 point in polygonBody.PolygonPathTransformed)
            {
                Vector2 circlePos = circleBody.Parent.absoluteCoordinateSystem.Position;
                if (PointInsideCircle(point, circleBody.Radius, circlePos)) 
                {
                    Vector2 normal = circlePos - point;
                    Vector2 unitNormal = normal / normal.Length();
                    return new MJIntersects(true, unitNormal);
                }

            }

            return new MJIntersects(false, new Vector2(0, 0));
        }

        public static Boolean LineIntersectsCircle(Vector2 a1, Vector2 a2, float radius, Vector2 position)
        {
            Vector2 CA = a1 - position;
            Vector2 directionA1A2 = a2 - a1;
            Vector2 unitDirection = directionA1A2 / directionA1A2.Length();
            Vector2 normal = new Vector2(-unitDirection.Y, unitDirection.X);
            float height = Abs(CA.X * normal.X + CA.Y * normal.Y);
            if (height < radius)
            {
                Vector2 AC = CA * -1;
                float length = (AC.X * directionA1A2.X + AC.Y * directionA1A2.Y) / directionA1A2.Length();
                return 0 < length && length < directionA1A2.Length();
            }

            return false;
        }

        public static Boolean PointInsideCircle(Vector2 point, float radius, Vector2 position)
        {
            float dx = point.X - position.X;
            float dy = point.Y - position.Y;
            float distance = dx * dx + dy * dy;
            float maxDistance = radius * radius;
            if (distance < maxDistance)
                return true;
            return false;
        }

        public static Boolean CircleIntersectsRectangle(float radius, Vector2 position, MJRectangle rectangle)
        {

            if (position.X < rectangle.MinX - radius)
                return false;
            if (position.X > rectangle.MaxX + radius)
                return false;
            if (position.Y < rectangle.MinY - radius)
                return false;
            if (position.Y > rectangle.MaxY + radius)
                return false;


            return true;
        }

        public static Boolean RectanglesIntersect(MJRectangle first, MJRectangle second)
        {
            if (second.MaxX < first.MinX)
                return false;
            if (second.MinX > first.MaxX)
                return false;
            if (second.MaxY < first.MinY)
                return false;
            if (second.MinY > first.MaxY)
                return false;
            return true;
        }

        private static float Min(float a, float b) {
            return a < b ? a : b;
        }

        private static float Max(float a, float b)
        {
            return a > b ? a : b;
        }

        private static float Abs(float a) {
            return a < 0 ? -a : a;
    }

    }
}
