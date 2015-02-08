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
                PolygonPathTransformed.Add(Vector3.Transform(v, TransformationMatrix));
            }
        }

        public void UpdateMatrix()
        {
            TransformationMatrix = Matrix.Identity;

            float rotation = Parent.absoluteCoordinateSystem.Rotation;
            Vector2 position = Parent.absoluteCoordinateSystem.Position;

            Matrix rotationMatrix = Matrix.CreateRotationZ((float)(Math.PI));

            Matrix translationMatrix = Matrix.CreateTranslation(
                new Vector3(position.X, position.Y, 0));
            TransformationMatrix = translationMatrix;

            //TransformationMatrix = Matrix.Multiply(translationMatrix, rotationMatrix);
        }

//------------------------- Collision detection ----------------------------\\

        public Boolean Collides(MJPhysicsBody other)
        {
            //Both bodies are circles
            if (Radius > -1 && other.Radius > -1)
                return CollidesBothCircular(other);

            //This body is circle, the other is not
            if (Radius > -1)
            {
                if (CircleIntersectsAxisAlignedBoundingBox(
                    other.AxisAlignedBoundingBox))
                {
                    for (int i = 0; i < other.PolygonPathTransformed.Count; i++)
                    {
                        Vector3 current = other.PolygonPathTransformed[i];
                        Vector2 a1 = new Vector2(current.X, current.Y);
                        int next = (i + 1) % other.PolygonPathTransformed.Count;
                        Vector3 nextVector = other.PolygonPathTransformed[next];
                        Vector2 a2 = new Vector2(nextVector.X, nextVector.Y);
                        if (LineCrossesCircle(a1, a2))
                        {
                            Console.WriteLine("i: " + i);
                            return true;
                        }
                    }
                }
            }

            //Other body is circle, this is not
            if (other.Radius > -1)
            {
                if (other.CircleIntersectsAxisAlignedBoundingBox(AxisAlignedBoundingBox))
                {
                    for (int i = 0; i < PolygonPathTransformed.Count; i++)
                    {
                        Vector3 current = PolygonPathTransformed[i];
                        Vector2 a1 = new Vector2(current.X, current.Y);
                        int next = (i + 1) % PolygonPathTransformed.Count;
                        Vector3 nextVector = PolygonPathTransformed[next];
                        Vector2 a2 = new Vector2(nextVector.X, nextVector.Y);
                        if (other.LineCrossesCircle(a1, a2))
                            return true;
                    }
                }
            }

            //Both bodies are polygons
            if (AxisAlignedIntersects(other.AxisAlignedBoundingBox))
            {
                foreach (Vector3 point3 in other.PolygonPathTransformed)
                {
                    Vector2 point = new Vector2(point3.X, point3.Y);
                    if (PointInsidePol(point))
                        return true;
                    //if (PointInsideBody(point))
                    //    return true;
                }
                
                foreach (Vector3 point3 in PolygonPathTransformed)
                {
                    Vector2 point = new Vector2(point3.X, point3.Y);
                    if (other.PointInsidePol(point))
                        return true;
                }
                //This needs to be fixed
                for (int i = 0; i < PolygonPathTransformed.Count; i++)
                {
                    Vector3 current = PolygonPathTransformed[i];
                    Vector2 a1 = new Vector2(current.X, current.Y);
                    int next = (i + 1) % PolygonPathTransformed.Count;
                    Vector3 nextVector = PolygonPathTransformed[next];
                    Vector2 a2 = new Vector2(nextVector.X, nextVector.Y);

                    for (int j = 0; j < other.PolygonPathTransformed.Count; j++)
                    {
                        Vector3 jCurrent = other.PolygonPathTransformed[j];
                        Vector2 b1 = new Vector2(jCurrent.X, jCurrent.Y);
                        int jNext = (j + 1) % other.PolygonPathTransformed.Count;
                        Vector3 jNextVector = other.PolygonPathTransformed[jNext];
                        Vector2 b2 = new Vector2(jNextVector.X, jNextVector.Y);

                        if (LinesCross(a1, a2, b1, b2))
                            return true;
                    }
                    
                }
                
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

        private Boolean CircleIntersectsAxisAlignedBoundingBox(MJRectangle other)
        {
            Vector2 pos = Parent.absoluteCoordinateSystem.Position;
            if (pos.X > other.MaxX + Radius)
                return false;
            if (pos.X < other.MinX - Radius)
                return false;
            if (pos.Y > other.MaxY + Radius)
                return false;
            if (pos.Y < other.MinY - Radius)
                return false;

            return true;
        }

        public Boolean PointInsideBody(Vector2 point)
        {
            if (Radius > -1)
                return PointInsideCircle(point);
            
            if (PointInsideAxisAlignedBoundingBox(point))
                return PointInsidePolygon(point);

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
            Vector2 h1 = point;
            Vector2 h2 = new Vector2(AxisAlignedBoundingBox.MaxX + 1, point.Y);
            int counter = 0;

            for (int i = 0; i < PolygonPathTransformed.Count; i++)
            {
                Vector2 a1 = new Vector2(PolygonPathTransformed[i].X, PolygonPathTransformed[i].Y);
                int next = (i + 1) % PolygonPathTransformed.Count;
                Vector2 a2 = new Vector2(PolygonPathTransformed[next].X, PolygonPathTransformed[next].Y);
                if (LineCrossesHorizontal(h1, h2, a1, a2))
                    counter++;
            }

            return counter % 2 != 0;
        }

        private Boolean LinesCross(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {

            if (LinesDoNotOverlapVertically(a1, a2, b1, b2) || 
                LinesDoNotOverlapHorizontally(a1, a2, b1, b2))
                return false;

            //Ya = Aa*x + Ab
            //Yb = Ab*x + Bb
            float Aa = (a1.Y - a2.Y) / (a1.X - a2.X);
            float Ba = (b1.Y - b2.Y) / (b1.X - b2.X);
            float Ab = a1.Y - Aa * a1.X;
            float Bb = b1.Y - Ba * b1.X;
            
            //Ya = Yb -> x * (Aa - Ba) = (Bb - Ab)
            float x = (Bb - Ab) / (Aa - Ba);
            
            float max = Math.Max(a1.X, a2.X);
            float min = Math.Min(a1.X, a2.X);

            return x >= min && x <= max;
        }

        private static bool LinesDoNotOverlapHorizontally(Vector2 a1, 
            Vector2 a2, Vector2 b1, Vector2 b2)
        {
            return 
                ((b1.X > a1.X && b1.X > a2.X) && (b2.X > a1.X && b2.X > a2.X)) 
                    ||
                ((b1.X < a1.X && b1.X < a2.X) && (b2.X < a1.X && b2.X < a2.X));
        }

        private static bool LinesDoNotOverlapVertically(Vector2 a1, 
            Vector2 a2, Vector2 b1, Vector2 b2)
        {
            return 
                ((b1.Y > a1.Y && b1.Y > a2.Y) && (b2.Y > a1.Y && b2.Y > a2.Y)) 
                    ||
                ((b1.Y < a1.Y && b1.Y < a2.Y) && (b2.Y < a1.Y && b2.Y < a2.Y));
        }

        /*
         * <summary>
         * pointH1 and pointH2 are assumed to be the points of the horizontal 
         * line.
         * </summary>
         */
        private Boolean LineCrossesHorizontal(Vector2 h1, Vector2 h2,
            Vector2 a1, Vector2 a2)
        {
            if (LinesDoNotOverlapVertically(h1, h2, a1, a2) && 
                LinesDoNotOverlapHorizontally(h1, h2, a1, a2))
            {
                return false;
            }

            //Ya = Aa*x + Ab
            float Aa = (a2.Y - a1.Y) / (a2.X - a1.X);
            //float Aa = (a1.Y - a2.Y) / (a1.X - a2.X);
            float Ab = a1.Y - Aa * a1.X;
            Console.WriteLine("Aa: " + Aa);
            
            //x = (Ya - b) / a
            float x = (h1.Y - Ab) / Aa;
            Console.WriteLine("x: " + x);
            
            float max = Math.Max(h1.X, h2.X);
            float min = Math.Min(h1.X, h2.X);
            return x >= min && x <= max;
        }

        public Boolean PointInsidePol(Vector2 point)
        {
            Vector2 endOfRay = new Vector2(AxisAlignedBoundingBox.MaxX + 10, point.Y);
            int counter = 0;

            for (int i = 0; i < PolygonPathTransformed.Count; i++)
            {
                int nextPos = (i + 1) % PolygonPathTransformed.Count;
                Vector3 current = PolygonPathTransformed[i];
                Vector3 next = PolygonPathTransformed[nextPos];

                Vector2 a1 = new Vector2(current.X, current.Y);
                Vector2 a2 = new Vector2(next.X, next.Y);

                if (ArcLineCrossed(point, endOfRay, a1, a2))
                    counter++;
            }


            return counter % 2 != 0;
        }

        private Boolean ArcLineCrossed(Vector2 p, Vector2 e, Vector2 a1, Vector2 a2)
        {
            float dy = a2.Y - a1.Y;
            float dx = a2.X - a1.X;

            float minX = (float)(Math.Min(p.X, e.X));
            float maxX = (float)(Math.Max(p.X, e.X));

            if (dy == 0)
            {
                return p.Y == a2.Y;
            }

            if (dx == 0)
            {
                if (a2.X >= minX && a2.X <= maxX)
                {
                    float minY = (float)(Math.Min(a1.Y, a2.Y));
                    float maxY = (float)(Math.Min(a1.Y, a2.Y));
                    return p.Y >= minY && p.Y <= maxY;
                }

                return false;
            }
            
            //Y = a * x + b
            //Ya = Aa * x + Ab
            float Aa = dy / dx;
            float Ab = a1.Y - Aa * a1.X;
            
            // x = (Y - b) / a
            // x = (p.Y - Ab) / Aa
            float x = (p.Y - Ab) / Aa;

            return x >= minX && x <= maxX;            
        }

        private Boolean LineCrossesCircle(Vector2 a1, Vector2 a2)
        {
            Vector2 circlePos = Parent.absoluteCoordinateSystem.Position;
            Vector2 AB = new Vector2(a2.X - a1.X, a2.Y - a1.Y);
            Vector2 BA = new Vector2(a1.X - a2.X, a1.Y - a2.Y);
            Vector2 AC = new Vector2(circlePos.X - a1.X, circlePos.Y - a1.Y);
            Vector2 BC = new Vector2(circlePos.X - a2.X, circlePos.Y - a2.Y);
            float angleABAC = CalculateAngleBetween(AB, AC);
            float angleBABC = CalculateAngleBetween(BA, BC);

            if (angleABAC > Math.PI / 2)
                return AC.Length() <= Radius;

            if (angleBABC > Math.PI / 2)
                return BC.Length() <= Radius;

            float distanceToLine = (float)(Math.Sin(angleABAC) * AC.Length());
            return distanceToLine <= Radius; 
        }

        private float CalculateAngleBetween(Vector2 line1, Vector2 line2)
        {
            float topEquation = line1.X * line2.X + line1.Y * line2.Y;
            float result = topEquation / (line1.Length() * line2.Length());
            return (float)(Math.Acos(result));
        }
    } 

}