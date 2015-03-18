using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork
{
    public class MJSat
    {
        public static MJIntersection Collides(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            if (body1.IsCircleBody() && body2.IsCircleBody())
                return CirclesCollide(body1, body2);
            else if (body1.IsCircleBody() || body2.IsCircleBody())
                return PolygonAndCircleCollide(body1, body2);
            else
                return PolygonsCollide(body1, body2);
        }

        public static MJIntersection CirclesCollide(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            Vector2 body1Center = body1.Parent.absoluteCoordinateSystem.Position;
            Vector2 body2Center = body2.Parent.absoluteCoordinateSystem.Position;
            Vector2 distanceBetweenCenters = body2Center - body1Center;

            float maxDistanceBetweenCenters = body1.Radius + body2.Radius;
            if (distanceBetweenCenters.LengthSquared() > maxDistanceBetweenCenters * maxDistanceBetweenCenters)
                return new MJIntersection(false, new Vector2(), 0f, body1, body2);

            Vector2 unitDistance = distanceBetweenCenters / distanceBetweenCenters.Length();
            float mtv = maxDistanceBetweenCenters - distanceBetweenCenters.Length();

            return new MJIntersection(true, unitDistance, -mtv, body1, body2);
        }

        public static MJIntersection PolygonAndCircleCollide(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            MJPhysicsBody circleBody = body1.IsCircleBody() ? body1 : body2;
            MJPhysicsBody polygonBody = body1.IsPolygonBody() ? body1 : body2;
            //if (PolygonAndCircleCannotCollide(circleBody, polygonBody))
            //    return new MJIntersection(false, Vector2.Zero, 0f, body1, body2);

            Vector2 circleCenter = circleBody.Parent.absoluteCoordinateSystem.Position;

            float mtv = float.MaxValue;
            Vector2 resultNormal = new Vector2();
            Boolean collision = false;

            List<Vector2> allNormals = new List<Vector2>();
            allNormals.AddRange(polygonBody.PolygonPathNormals);
            allNormals.Add(GetCircleNormal(circleBody, polygonBody));

            foreach (Vector2 normal in allNormals)
            {
                //Calculate the min and max projection of body1 onto the normal
                float polygonMinProj = float.MaxValue, polygonMaxProj = float.MinValue;
                foreach (Vector2 point in polygonBody.PolygonPathTransformed)
                {
                    float projection = point.X * normal.X + point.Y * normal.Y;
                    if (projection < polygonMinProj)
                        polygonMinProj = projection;
                    if (projection > polygonMaxProj)
                        polygonMaxProj = projection;
                }

                //Calculate the min and max projection of the circle onto the normal
                float circleCenterProjection = circleCenter.X * normal.X + circleCenter.Y * normal.Y;
                float circleMinProj = circleCenterProjection - circleBody.Radius, circleMaxProj = circleCenterProjection + circleBody.Radius;

                float overlap = Overlap(polygonMinProj, polygonMaxProj, circleMinProj, circleMaxProj);
                if (overlap < 0)
                    return new MJIntersection(false, new Vector2(), 0, body1, body2);
                else
                {
                    if (overlap < mtv)
                    {
                        mtv = overlap;
                        resultNormal = normal;
                        collision = true;
                    }
                }

            }

            return new MJIntersection(collision, resultNormal, mtv, body1, body2);
        }

        public static MJIntersection PolygonsCollide(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            if (PolygonsCannotCollide(body1, body2))
                return new MJIntersection(false, Vector2.Zero, 0f, body1, body2);

            float mtv = float.MaxValue;
            Vector2 resultNormal = new Vector2();
            Boolean collision = false;

            List<Vector2> allNormals = new List<Vector2>();
            allNormals.AddRange(body1.PolygonPathNormals);
            allNormals.AddRange(body2.PolygonPathNormals);

            foreach (Vector2 normal in allNormals)
            {
                //Calculate the min and max projection of body1 onto the normal
                float body1MinProj = float.MaxValue, body1MaxProj = float.MinValue;
                foreach (Vector2 point in body1.PolygonPathTransformed)
                {
                    float projection = point.X * normal.X + point.Y * normal.Y;
                    if (projection < body1MinProj)
                        body1MinProj = projection;
                    if (projection > body1MaxProj)
                        body1MaxProj = projection;
                }

                //Calculate the min and max projection of body2 onto the normal
                float body2MinProj = float.MaxValue, body2MaxProj = float.MinValue;
                foreach (Vector2 point in body1.PolygonPathTransformed)
                {
                    float projection = point.X * normal.X + point.Y * normal.Y;
                    if (projection < body2MinProj)
                        body2MinProj = projection;
                    if (projection > body2MaxProj)
                        body2MaxProj = projection;
                }

                //Calculate the overlap of the bodies onto the projection
                float overlap = Overlap(body1MinProj, body1MaxProj, body2MinProj, body2MaxProj);
                if (overlap < 0)
                    return new MJIntersection(false, new Vector2(), 0, body1, body2);
                else
                {
                    if (overlap < mtv)
                    {
                        mtv = overlap;
                        resultNormal = normal;
                        collision = true;
                    }
                }
            }

            return new MJIntersection(collision, resultNormal, mtv, body1, body2);
        }

        private static Boolean PolygonAndCircleCannotCollide(MJPhysicsBody circleBody, MJPhysicsBody polygonBody)
        {
            Vector2 circleCenter = circleBody.Parent.absoluteCoordinateSystem.Position;
            if (polygonBody.AxisAlignedBoundingBox.MaxX < circleCenter.X - circleBody.Radius)
                return true;
            if (polygonBody.AxisAlignedBoundingBox.MinX > circleCenter.X + circleBody.Radius)
                return true;
            if (polygonBody.AxisAlignedBoundingBox.MaxY < circleCenter.Y - circleBody.Radius)
                return true;
            if (polygonBody.AxisAlignedBoundingBox.MinY > circleCenter.Y + circleBody.Radius)
                return true;
            return false;
        }
        
        private static Boolean PolygonsCannotCollide(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            if (body1.AxisAlignedBoundingBox.MaxX < body2.AxisAlignedBoundingBox.MinX)
                return true;
            if (body1.AxisAlignedBoundingBox.MinX > body2.AxisAlignedBoundingBox.MaxX)
                return true;
            if (body1.AxisAlignedBoundingBox.MaxY < body2.AxisAlignedBoundingBox.MinY)
                return true;
            if (body1.AxisAlignedBoundingBox.MinY > body2.AxisAlignedBoundingBox.MaxY)
                return true;
            return false;
        }

        private static float Overlap(float min1, float max1, float min2, float max2)
        {
            if (min2 > max1)
                return -1f;
            if (min1 > max2)
                return -1f;

            if (max2 > max1)
                return Abs(min2 - max1);

            return Abs(max1 - min2);
        }

        private static Vector2 GetCircleNormal(MJPhysicsBody circle, MJPhysicsBody polygon)
        {
            Vector2 normal = new Vector2((float)Math.Sqrt(float.MaxValue - 1), 0);
            Vector2 circleCenter = circle.Parent.absoluteCoordinateSystem.Position;

            foreach (Vector2 point in polygon.PolygonPathTransformed)
            {
                Vector2 distance = circleCenter - point;
                if (distance.LengthSquared() < normal.LengthSquared())
                {
                    normal = distance;
                }
            }

            return normal / normal.Length();
        }

        private static float Abs(float number)
        {
            if (number < 0)
                return -number;
            return number;
        }
    }
}
