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

            for (int i = 0; i < polygonBody.PolygonPathTransformed.Count; i++)
            {
                int next = (i + 1) % polygonBody.PolygonPathTransformed.Count;
                Vector2 a1 = polygonBody.PolygonPathTransformed[i];
                Vector2 a2 = polygonBody.PolygonPathTransformed[next];

                if (LineIntersectsCircle(a1, a2, circleBody.Radius, circleBody.Parent.absoluteCoordinateSystem.Position))
                    return new MJIntersects(true, polygonBody.PolygonPathNormals[i]);
            }

            return new MJIntersects(false, new Vector2(0, 0));
        }

        public static Boolean LineIntersectsCircle(Vector2 a1, Vector2 a2, float radius, Vector2 position)
        {

            if (PointInsideCircle(a1, radius, position) || PointInsideCircle(a2, radius, position))
                return true;

            if (PointCannotBeOnLine(a1, a2, position, radius))
                return false;

            Vector2 pointToA = a1 - position;
            Vector2 direction = a2 - a1;
            Vector2 normal = new Vector2(-direction.Y, direction.X);
            Vector2 unitNormal = normal / normal.Length();
            float distanceFromLine = Abs(pointToA.X * unitNormal.X + pointToA.Y * unitNormal.Y);
            if (distanceFromLine < radius)
                return true;
            
            return false;
        }

        private static Boolean PointCannotBeOnLine(Vector2 a1, Vector2 a2, Vector2 point, float radius)
        {
            float minX = Min(a1.X, a2.X);
            float maxX = Max(a1.X, a2.X);

            float minY = Min(a1.Y, a2.Y);
            float maxY = Max(a1.Y, a2.Y);

            if (point.X < minX - radius)
                return true;
            if (point.X > maxX + radius)
                return true;
            if (point.Y < minY - radius)
                return true;
            if (point.Y > maxY + radius)
                return true;
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
