﻿using System;
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
        public MJRectangle AxisAlignedBoundingBox { get; set; }
        
        /*
         * <summary>
         * A list containing each point that defines the polygon box.
         * </summary>
         */
        public List<Vector3> PolygonPath { get; set; }

        public List<Vector3> PolygonPathTransformed { get; set; }

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
            Console.WriteLine("Creating the initial bounding box");
            for (int i = 0; i < body.PolygonPath.Count; i++)
            {
                Vector3 v = body.PolygonPath[i];
                Console.WriteLine("v" + i + ": " + v);
            } Console.WriteLine("Finished creating the initial bounding box");
            foreach (Vector3 v in body.PolygonPath)
            {
                body.PolygonPathTransformed.Add(new Vector3(v.X, v.Y, v.Z));
            }
            body.CalculateAxisAlignedBoundingBox();
            Console.WriteLine("AABB: " + body.AxisAlignedBoundingBox);
            return body;
        }

        public static MJPhysicsBody PolygonPathMJPhysicsBody(
            List<Vector3> path)
        {
            MJPhysicsBody body = new MJPhysicsBody();
            body.PolygonPath = path;
            Console.WriteLine("Creating the initial bounding box");
            for (int i = 0; i < body.PolygonPath.Count; i++)
            {
                Vector3 v = body.PolygonPath[i];
                Console.WriteLine("v" + i + ": " + v);
            } Console.WriteLine("Finished creating the initial bounding box");
            foreach (Vector3 v in path)
            {
                body.PolygonPathTransformed.Add(new Vector3(v.X, v.Y, v.Z));
            }
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
            PolygonPathTransformed = new List<Vector3>();
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
                float minX = PolygonPathTransformed[0].X;
                float minY = PolygonPathTransformed[0].Y;
                float maxX = PolygonPathTransformed[1].X;
                float maxY = PolygonPathTransformed[1].Y;

                foreach (Vector3 point in PolygonPathTransformed)
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

            PolygonPathTransformed.Clear();
            UpdateMatrix();
            UpdatePolygons();
            CalculateAxisAlignedBoundingBox();
        }

        private void UpdatePolygons()
        {
            foreach (Vector3 v in PolygonPath)
            {
                Vector3 copy = new Vector3(v.X, v.Y, v.Z);
                PolygonPathTransformed.Add(Vector3.Transform(copy, TransformationMatrix));
            }
        }

        public void UpdateMatrix()
        {
            TransformationMatrix = Matrix.Identity;

            //float rotation = Parent.absoluteCoordinateSystem.Rotation;
            Vector2 position = Parent.absoluteCoordinateSystem.Position;

            //Matrix rotationMatrix = Matrix.CreateRotationZ((float)(Math.PI));

            Matrix translationMatrix = Matrix.CreateTranslation(
                new Vector3(position.X, position.Y, 0));
            TransformationMatrix = translationMatrix;

            //TransformationMatrix = Matrix.Multiply(translationMatrix, rotationMatrix);
        }
    } 

}