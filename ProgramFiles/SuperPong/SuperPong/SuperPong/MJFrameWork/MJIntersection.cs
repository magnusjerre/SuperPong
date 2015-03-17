using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork
{
    public class MJIntersection
    {

        public static MJIntersects Intersects(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            if (BothBodiesAreCircles(body1, body2))
                return CirclesIntersect(body1, body2);
            if (OneBodyIsACircle(body1, body2))
                return PolygonAndCircleIntersects(body1, body2);
            else
                return PolygonsIntersect(body1, body2);
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

        public static MJIntersects PolygonAndCircleIntersects(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            MJPhysicsBody pathBody = body1.Radius < 0 ? body1 : body2;
            MJPhysicsBody circleBody = body1.Radius > 0 ? body1 : body2;

            if (!CircleIntersectsRectangle(circleBody.Radius, circleBody.Parent.absoluteCoordinateSystem.Position, pathBody.AxisAlignedBoundingBox))
                return new MJIntersects(false, new Vector2(0, 0));

            Vector2 circleNormal = GetClosesNormalToCircle(pathBody, circleBody);

            float mtv = float.MaxValue;
            Vector2 translationNormal = new Vector2();
            Boolean intersects = false;

            List<Vector2> allNormals = new List<Vector2>();
            allNormals.AddRange(pathBody.PolygonPathNormals);
            allNormals.Add(circleNormal);

            foreach (Vector2 normal in pathBody.PolygonPathNormals)
            {
                float body1MinProjection = float.MaxValue, body1MaxPojection = float.MinValue;
                float body2MinProjection = float.MaxValue, body2MaxPojection = float.MinValue;

                foreach (Vector2 point in pathBody.PolygonPathTransformed)
                {
                    float projection = normal.X * point.X + normal.Y * point.Y;
                    if (projection < body1MinProjection)
                    {
                        body1MinProjection = projection;
                    }
                    if (projection > body1MaxPojection)
                    {
                        body1MaxPojection = projection;
                    }
                }

                float projection2 = circleBody.Parent.absoluteCoordinateSystem.Position.X * normal.X + circleBody.Parent.absoluteCoordinateSystem.Position.Y * normal.Y;
                body2MinProjection = projection2 - circleBody.Radius;
                body2MaxPojection = projection2 + circleBody.Radius;

                float overlap = ProjectionOverlap(body1MinProjection, body1MaxPojection, body2MinProjection, body2MaxPojection);
                if (overlap < 0)
                    return new MJIntersects(false, new Vector2());
                if (overlap > 0)
                {
                    intersects = true;
                    if (overlap < mtv)
                    {
                        mtv = overlap;
                        translationNormal = normal;
                    }
                }
            }

            return new MJIntersects(intersects, translationNormal);

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

        private static Vector2 GetClosesNormalToCircle(MJPhysicsBody pathBody, MJPhysicsBody circleBody)
        {
            Vector2 closestPoint = new Vector2(float.MaxValue, 0);
            Vector2 circleCenter = circleBody.Parent.absoluteCoordinateSystem.Position;
            foreach (Vector2 point in pathBody.PolygonPathTransformed)
            {
                Vector2 distance = circleCenter - point;
                if (distance.LengthSquared() < closestPoint.LengthSquared())
                {
                    closestPoint = distance;
                }
            }

            Vector2 theDistance = circleCenter - closestPoint;
            Vector2 unitDistance = theDistance / theDistance.Length();

            return unitDistance;
        }

        public static MJIntersects PolygonsIntersect(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            float mtv = float.MaxValue;
            Vector2 translationNormal = new Vector2();
            Boolean intersects = false;
            if (!RectanglesIntersect(body1.AxisAlignedBoundingBox, body2.AxisAlignedBoundingBox))
                return new MJIntersects(false, new Vector2());

            foreach (Vector2 normal in body1.PolygonPathNormals)
            {

                float body1MinProjection = float.MaxValue, body1MaxPojection = float.MinValue;
                float body2MinProjection = float.MaxValue, body2MaxPojection = float.MinValue;

                foreach (Vector2 points in body1.PolygonPathTransformed)
                {
                    float projection = normal.X * points.X + normal.Y * points.Y;
                    if (projection < body1MinProjection)
                    {
                        body1MinProjection = projection;
                    }
                    if (projection > body1MaxPojection)
                    {
                        body1MaxPojection = projection;
                    }
                }

                foreach (Vector2 points in body2.PolygonPathTransformed)
                {
                    float projection = normal.X * points.X + normal.Y * normal.Y;
                    if (projection < body2MinProjection)
                    {
                        body2MinProjection = projection;
                    }
                    if (projection > body2MaxPojection)
                    {
                        body2MaxPojection = projection;
                    }
                }

                float overlap = ProjectionOverlap(body1MinProjection, body1MaxPojection, body2MinProjection, body2MaxPojection);
                if (overlap < 0)
                    return new MJIntersects(false, new Vector2());
                if (overlap > 0)
                {
                    intersects = true;
                    if (overlap < mtv)
                    {
                        mtv = overlap;
                        translationNormal = normal; 
                    }
                }

            }

            foreach (Vector2 normal in body2.PolygonPathNormals)
            {

                float body1MinProjection = float.MaxValue, body1MaxPojection = float.MinValue;
                float body2MinProjection = float.MaxValue, body2MaxPojection = float.MinValue;

                foreach (Vector2 points in body1.PolygonPathTransformed)
                {
                    float projection = normal.X * points.X + normal.Y * points.Y;
                    if (projection < body1MinProjection)
                    {
                        body1MinProjection = projection;
                    }
                    if (projection > body1MaxPojection)
                    {
                        body1MaxPojection = projection;
                    }
                }

                foreach (Vector2 points in body2.PolygonPathTransformed)
                {
                    float projection = normal.X * points.X + normal.Y * normal.Y;
                    if (projection < body2MinProjection)
                    {
                        body2MinProjection = projection;
                    }
                    if (projection > body2MaxPojection)
                    {
                        body2MaxPojection = projection;
                    }
                }

                float overlap = ProjectionOverlap(body1MinProjection, body1MaxPojection, body2MinProjection, body2MaxPojection);
                if (overlap < 0)
                    return new MJIntersects(false, new Vector2());
                if (overlap > 0)
                {
                    intersects = true;
                    if (overlap < mtv)
                    {
                        mtv = overlap;
                        translationNormal = normal;
                    }
                }

            }

            return new MJIntersects(intersects, translationNormal);
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

        private static float ProjectionOverlap(float b1min, float b1max, float b2min, float b2max)
        {
            if (b1min > b2max)
                return -1;
            if (b2min > b1max)
                return -1;
            if (b1max < b2min)
                return -1;
            if (b2max < b1min)
                return -1;

            float rightmostMin = b1min < b2min ? b1min : b2min;
            float leftmostMax = b1max < b2max ? b1max : b2max;

            return leftmostMax - rightmostMin;
        }

    }
}
