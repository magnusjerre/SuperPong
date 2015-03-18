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

        public Vector2 AccelerationFromForce { get; set; }

        public Matrix TransformationMatrix { get; set; }

        public Boolean ToBeRemoved { get; set; }
        /*
         * <summary>
         * The axis-aligned bounding box is the rectangular box created using
         * the extreme values of the tighter physics body. It will always be 
         * aligned with the major coordinate system.
         * </summary>
         */
        public MJRectangle AxisAlignedBoundingBox { get; set; }
        
        /*
         * <summary>
         * A list containing each point that defines the polygon box.
         * </summary>
         */
        public List<Vector2> PolygonPath { get; set; }
        public List<Vector2> PolygonPathTransformed { get; set; }
        public List<Vector2> PolygonPathNormals { get; set; }

        public float RotationalAcceleration { get; set; }
        public float RotationalSpeed { get; set; }

        public uint CollisionMask { get; set; }
        public uint IntersectionMask { get; set; }
        public uint Bitmask { get; set; }

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
            foreach (Vector2 v in body.PolygonPath)
            {
                body.PolygonPathTransformed.Add(new Vector2(v.X, v.Y));
            }
            body.CalculateAxisAlignedBoundingBox();
            body.UpdatePolygons();
            return body;
        }

        public static MJPhysicsBody PolygonPathMJPhysicsBody(
            List<Vector2> path)
        {
            MJPhysicsBody body = new MJPhysicsBody();
            body.PolygonPath = path;
            
            foreach (Vector2 v in path)
            {
                body.PolygonPathTransformed.Add(new Vector2(v.X, v.Y));
            }
            body.CalculateAxisAlignedBoundingBox();
            body.UpdatePolygons();
            return body;
        }

        private MJPhysicsBody()
        {
            Parent = null;
            Radius = -1.0f;
            Mass = 1.0f;
            Velocity = new Vector2(0, 0);
            Acceleration = new Vector2(0, 0);
            AccelerationFromForce = new Vector2();
            IsStatic = false;
            AxisAlignedBoundingBox = new MJRectangle(0, 0, 0, 0);
            PolygonPath = new List<Vector2>();
            PolygonPathTransformed = new List<Vector2>();
            PolygonPathNormals = new List<Vector2>();

            Bitmask = 0;
            CollisionMask = 0;
            IntersectionMask = 0;
            ToBeRemoved = false;
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
         */
        protected List<Vector2> RectangularBoundingBox(Vector2 size, 
            Vector2 origin)
        {
            float minX = -origin.X * size.X;
            float minY = -origin.Y * size.Y;
            float maxX = size.X - origin.X * size.X;
            float maxY = size.Y - origin.Y * size.Y;
            Vector2 upperLeft = new Vector2(minX, minY);
            Vector2 upperRight = new Vector2(maxX, minY);
            Vector2 lowerRight = new Vector2(maxX, maxY);
            Vector2 lowerLeft = new Vector2(minX, maxY);

            List<Vector2> polygonPath = new List<Vector2>();
            polygonPath.Add(upperLeft);
            polygonPath.Add(lowerLeft);
            polygonPath.Add(lowerRight);
            polygonPath.Add(upperRight);
            
            return polygonPath;
        }

        protected void CalculateAxisAlignedBoundingBox()
        {
            if (PolygonPath.Count > 1)
            {
                float minX = PolygonPathTransformed[0].X;
                float minY = PolygonPathTransformed[0].Y;
                float maxX = PolygonPathTransformed[1].X;
                float maxY = PolygonPathTransformed[1].Y;

                foreach (Vector2 point in PolygonPathTransformed)
                {
                    if (point.X < minX)
                        minX = point.X;
                    else if (point.X > maxX)
                        maxX = point.X;
                    
                    if (point.Y < minY)
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
            this.Parent = null;
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            float dt = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            Vector2 velocityFromAcceleration = (Acceleration + AccelerationFromForce) * dt;
            AccelerationFromForce = new Vector2();
            Velocity += velocityFromAcceleration;
            Parent.Position += Velocity * dt;

            float rotSpeedFromAccel = RotationalAcceleration * dt;
            RotationalAcceleration = 0;
            RotationalSpeed += rotSpeedFromAccel;
            float deltaRot = RotationalSpeed * dt;
            Parent.Rotation += deltaRot;

            if (Radius == -1)
            {
                UpdateMatrix();
                UpdatePolygons();
                CalculateAxisAlignedBoundingBox();
            }
        }

        public void UpdatePolygons()
        {
            PolygonPathTransformed.Clear();
            foreach (Vector2 v in PolygonPath)
            {
                Vector2 copy = new Vector2(v.X, v.Y);
                Vector2 transformed = Vector2.Transform(copy, TransformationMatrix);
                PolygonPathTransformed.Add(transformed);

            }
            CalculateNormals();
        }

        private void CalculateNormals()
        {
            PolygonPathNormals.Clear();
            for (int i = 0; i < PolygonPathTransformed.Count; i++)
            {
                int next = (i + 1) % PolygonPathTransformed.Count;
                Vector2 line = PolygonPathTransformed[next] - PolygonPathTransformed[i];
                Vector2 normal = new Vector2(-line.Y, line.X);
                Vector2 unitNormal = normal / normal.Length();
                PolygonPathNormals.Add(unitNormal);
            }
        }

        public void UpdateMatrix()
        {
            TransformationMatrix = Matrix.Identity;
            Vector2 position = Parent.absoluteCoordinateSystem.Position;

            Matrix translationMatrix = Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0));
            Matrix rotationMatrix = Matrix.CreateRotationZ(Parent.absoluteCoordinateSystem.Rotation);
            TransformationMatrix = rotationMatrix * translationMatrix;
        }

        public Boolean ShouldCheckForCollision(uint otherBitmask)
        {
            return (otherBitmask & CollisionMask) > 0;
        }

        public Boolean ShouldCheckForIntersection(uint otherBitmask)
        {
            return (otherBitmask & IntersectionMask) > 0;
        }

        public bool IsCircleBody()
        {
            return Radius > 0;
        }

        public bool IsPolygonBody()
        {
            return PolygonPath.Count > 0;
        }
    } 

}