using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SuperPong.MJFrameWork
{
    public class MJPhysicsBody : MJDetachable, MJUpdate
    {
        /*
         * <summary>
         * The parent is the MJNode this physicsbody is connected to.
         * </summary>
         * 
         */
        public MJNode Parent { get; set; }

        /*
         * <summary>
         * This is the W-component associated with homogenous coordinates.
         * </summary>
         */
        public float W = 1.0f;

        /*
         *<summary>
         * The radius specifies the size of the body using a radius. 
         * Ideal for circular bodies. Default radius is -1, which means
         * that the body is not a circle.
         * </summary>
         */
        public float Radius { get; set; }

        public float Mass { get; set; }

        public Boolean IsStatic { get; set; }

        public Vector2 Velocity { get; set; }

        public Vector2 Acceleration { get; set; }

        public Matrix TransformationMatrix { get; set; }

        /*
         * <summary>
         * The axis-aligned bounding box is the rectangular box created using
         * the extreme values of the tighter physics body. It will always be 
         * aligned with the major coordinate system.
         * </summary>
         */
        protected MJRectangle AxisAlignedBoundingBox { get; set; }
        
        /*
         * <summary>
         * A list containing each point that defines the polygon box.
         * </summary>
         */
        protected List<Vector3> PolygonPath { get; set; }

        public static MJPhysicsBody CircularMJPhysicsBody(float radius) 
        {
            MJPhysicsBody body = new MJPhysicsBody();
            body.Radius = radius;
            return body;
        }

        public static MJPhysicsBody RectangularMJPhysicsBody(Vector2 size,
            Vector2 origin)
        {
            MJPhysicsBody body = new MJPhysicsBody();
            body.PolygonPath = body.RectangularBoundingBox(size, origin);
            body.CalculateAxisAlignedBoundingBox();
            return body;
        }

        public static MJPhysicsBody PolygonPathMJPhysicsBody(
            List<Vector3> path)
        {
            MJPhysicsBody body = new MJPhysicsBody();
            body.PolygonPath = path;
            body.CalculateAxisAlignedBoundingBox();
            return body;
        }

        private MJPhysicsBody()
        {
            Parent = null;
            Radius = -1.0f;
            Mass = 1.0f;
            Velocity = new Vector2(0, 0);
            Acceleration = new Vector2(0, 0);
            IsStatic = false;
            AxisAlignedBoundingBox = new MJRectangle(0, 0, 0, 0);
            PolygonPath = new List<Vector3>();
        }

        /*
         * <summary>
         * Creates the polygon path representing a rectangular box with the 
         * given size as input and the offset based on the input origin. 
         * 
         * The first point starts in the upper left corner, the second point is
         * the upper right, the third the lower right and the fourth the lower
         * left.
         * </summar>
         * 
         */
        protected List<Vector3> RectangularBoundingBox(Vector2 size, 
            Vector2 origin)
        {
            float minX = -origin.X * size.X;
            float minY = -origin.Y * size.Y;
            float maxX = size.X - origin.X * size.X;
            float maxY = size.Y - origin.Y * size.Y;
            Vector3 upperLeft = new Vector3(minX, minY, W);
            Vector3 upperRight = new Vector3(maxX, minY, W);
            Vector3 lowerRight = new Vector3(maxX, maxY, W);
            Vector3 lowerLeft = new Vector3(minX, maxY, W);

            List<Vector3> polygonPath = new List<Vector3>();
            polygonPath.Add(upperLeft);
            polygonPath.Add(upperRight);
            polygonPath.Add(lowerRight);
            polygonPath.Add(lowerLeft);
            
            return polygonPath;
        }

        protected void CalculateAxisAlignedBoundingBox()
        {
            if (PolygonPath.Count > 1)
            {
                float minX = PolygonPath[0].X;
                float minY = PolygonPath[0].Y;
                float maxX = PolygonPath[1].X;
                float maxY = PolygonPath[1].Y;

                foreach (Vector3 point in PolygonPath)
                {
                    if (point.X < minX)
                        minX = point.X;
                    else if (point.X > maxX)
                        maxX = point.X;
                    else if (point.Y < minY)
                        minY = point.Y;
                    else if (point.Y > maxY)
                        maxY = point.Y;
                }

                AxisAlignedBoundingBox = new MJRectangle(minX, minY, maxX, 
                    maxY);
            }
        }

        public void DetachFromParent()
        {
            throw new NotImplementedException();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            float dt = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            Vector2 velocityFromAcceleration = Acceleration * dt;
            Velocity += velocityFromAcceleration;
            Parent.Position += Velocity * dt;
        }

        public void UpdateMatrix()
        {
            TransformationMatrix = Matrix.Identity;

            float rotation = Parent.absoluteCoordinateSystem.Rotation;
            Vector2 position = Parent.absoluteCoordinateSystem.Position;

            Matrix rotationMatrix = Matrix.CreateRotationZ(rotation);

            Matrix translationMatrix = Matrix.CreateTranslation(
                new Vector3(position.X, position.Y, 0));

            Matrix inversetranslationMatrix = Matrix.CreateTranslation(
                new Vector3(-position.X, -position.Y, 0));
        }

//------------------------- Collision detection ----------------------------\\

        public Boolean Collides(MJPhysicsBody other)
        {
            if (Radius > -1 && other.Radius > -1)
                return CollidesBothCircular(other);

            if (AxisAlignedIntersects(other.AxisAlignedBoundingBox))
            {
                return true;
            }
            return false;
        }

        private Boolean CollidesBothCircular(MJPhysicsBody other)
        {
            Vector2 pos1 = Parent.absoluteCoordinateSystem.Position;
            Vector2 pos2 = other.Parent.absoluteCoordinateSystem.Position;
            float dx = pos2.X - pos1.X;
            float dy = pos2.Y - pos1.Y;
            float distance = dx * dx + dy * dy;

            return distance < (Radius + other.Radius) * (Radius + other.Radius);
        }

        private Boolean AxisAlignedIntersects(MJRectangle other)
        {
            if (other.MaxX < AxisAlignedBoundingBox.MinX)
                return false;

            if (other.MinX > AxisAlignedBoundingBox.MaxX)
                return false;

            if (other.MaxY < AxisAlignedBoundingBox.MinY)
                return false;

            if (other.MinY > AxisAlignedBoundingBox.MaxY)
                return false;

            return true;
        }

        public Boolean PointInsideBody(Vector2 point)
        {
            if (Radius > -1)
                return PointInsideCircle(point);
            if (PointInsideAxisAlignedBoundingBox(point))
                return true;    //TODO: More implementation needed
            return false;
        }

        private Boolean PointInsideCircle(Vector2 point)
        {
            Vector2 pos = Parent.absoluteCoordinateSystem.Position;
            float dx = point.X - pos.X;
            float dy = point.Y - pos.Y;

            return dx * dx + dy * dy < Radius * Radius;
        }

        private Boolean PointInsideAxisAlignedBoundingBox(Vector2 point)
        {
            if (point.X < AxisAlignedBoundingBox.MinX)
                return false;
            if (point.X > AxisAlignedBoundingBox.MaxX)
                return false;
            if (point.Y < AxisAlignedBoundingBox.MinY)
                return false;
            if (point.Y > AxisAlignedBoundingBox.MaxY)
                return false;
            
            return true;
        }

        private Boolean PointInsidePolygon(Vector2 point)
        {
            Vector2 pointH1 = point;
            Vector2 pointH2 = new Vector2(AxisAlignedBoundingBox.MaxY, 
                point.Y);

            int counter = 0;

            for (int i = 0; i < PolygonPath.Count; i++)
            {
                Vector2 pointA1 = new Vector2(PolygonPath[i].X,
                    PolygonPath[i].Y);
                int next = (i + 1) % PolygonPath.Count;
                Vector2 pointA2 = new Vector2(PolygonPath[next].X,
                    PolygonPath[next].Y);
                if (LineCrossesHorizontal(pointH1, pointH2, pointA1, pointA2))
                    counter++;
            }

            return counter % 2 != 0;
        }

        private Boolean LinesCross(Vector2 pointA1, Vector2 pointA2,
            Vector2 pointB1, Vector2 pointB2)
        {
            //Ya = a*x + b
            //Yb = a*x + b
            float Aa = (pointA1.Y - pointA2.Y) / (pointA1.X - pointA2.X);
            float Ba = (pointB1.Y - pointB2.Y) / (pointB1.X - pointB2.X);
            float Ab = pointA1.Y - Aa * pointA1.X;
            float Bb = pointB1.Y - Ba * pointB1.X;
            
            //Ya = Yb -> x * (Aa - Ba) = (Bb - Ab)
            float x = (Bb - Ab) / (Aa - Ba);
            
            float max = Math.Max(pointA1.X, pointA2.X);
            float min = Math.Min(pointA1.X, pointA2.X);

            return x >= min && x <= max;
        }

        /*
         * <summary>
         * pointH1 and pointH2 are assumed to be the points of the horizontal 
         * line.
         * </summary>
         */
        private Boolean LineCrossesHorizontal(Vector2 pointH1, Vector2 pointH2,
            Vector2 pointA1, Vector2 pointA2)
        {
            if (pointA1.Y > pointH1.Y && pointA2.Y > pointH1.Y ||
                pointA1.Y < pointH1.Y && pointA2.Y < pointH1.Y)
            {
                return false;
            }

            //Ya = a*x + b
            float Aa = (pointA1.Y - pointA2.Y) / (pointA1.X - pointA2.X);
            float Ab = pointA1.Y - Aa * pointA1.X;
            
            //x = (Ya - b) / a
            float x = (pointH1.Y - Ab) / Aa;
            
            float max = Math.Max(pointH1.X, pointH2.X);
            float min = Math.Min(pointH1.X, pointH2.X);
            return x >= min && x <= max;
        }
    }

}