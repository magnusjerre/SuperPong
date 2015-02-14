﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork
{
    public class MJCollision
    {

        //---------------- Point inside/on ------------------\\

        public static Boolean PointInsideCircle(float radius, Vector2 pos, Vector2 point)
        {
            float dx = point.X - pos.X;
            float dy = point.Y - pos.Y;
            return dx * dx + dy * dy <= radius * radius;
        }

        public static Boolean PointInsideRectangle(MJRectangle rectangle, Vector2 point)
        {
            if (point.X < rectangle.MinX)
                return false;
            if (point.X > rectangle.MaxX)
                return false;
            if (point.Y < rectangle.MinY)
                return false;
            if (point.Y > rectangle.MaxY)
                return false;
            return true;
        }

        public static Boolean PointInsidePolygon(MJRectangle boundingBox, List<Vector2> path, Vector2 point)
        {
            Vector2 endOfRay = new Vector2(boundingBox.MaxX + 10, point.Y);
            int counter = 0;

            for (int i = 0; i < path.Count; i++)
            {
                int nextPos = (i + 1) % path.Count;
                Vector2 a1 = path[i];
                Vector2 a2 = path[nextPos];                
                if (LinesIntersect(point, endOfRay, a1, a2))
                {
                    if (!PointOnLine(point, endOfRay, a1, 0.1f))
                        counter++;
                }
            }
            
            return counter % 2 != 0;
        }

        public static Boolean PointOnLine(Vector2 a1, Vector2 a2, Vector2 point, float delta)
        {
            if (PointCannotBeOnLine(a1, a2, point, delta))
                return false;

            float dy = a2.Y - a1.Y;
            float dx = a2.X - a1.X;

            if (-delta < dx && dx < delta)  //Vertical line
            {
                if (a2.X - delta < point.X && point.X < a2.X + delta)
                {
                    float minY = Min(a1.Y, a2.Y) - delta;
                    float maxY = Max(a1.Y, a2.Y) + delta;
                    return minY < point.Y && point.Y < maxY;
                }
                return false;
            }

            float a = dy / dx;
            float b = a1.Y - a * a1.X;
            float y = a * point.X + b;
            
            return y - delta < point.Y && point.Y < y + delta;
        }

        private static bool PointCannotBeOnLine(Vector2 a1, Vector2 a2, Vector2 point, float delta)
        {
            return (point.X < a1.X - delta && point.X < a2.X - delta) ||
                   (point.X > a1.X + delta && point.X > a2.X + delta) ||
                   (point.Y < a1.Y - delta && point.Y < a2.Y - delta) ||
                   (point.Y > a1.Y + delta && point.Y > a2.Y + delta);
        }

        //--------------- Shapes intersect -------------------\\

        public static Boolean CirclesIntersect(float radius1, Vector2 pos1,
            float radius2, Vector2 pos2)
        {
            float dx = pos2.X - pos1.X;
            float dy = pos2.Y - pos1.Y;
            float distance = dx * dx + dy * dy;
            return distance < (radius1 + radius2) * (radius1 + radius2);
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

        private static Boolean RectangleIntersectsCirclesBoundingRectangle(
            float radius, Vector2 pos, MJRectangle rectangle)
        {
            MJRectangle rect = new MJRectangle(pos.X - radius, pos.Y - radius,
                pos.X + radius, pos.Y + radius);
            return RectanglesIntersect(rect, rectangle);
        }

        //----------------- Line Intersections -----------------\\
        /*
         * <summary> Checks whether two lines intersect or not with a delta of
         * 0.1f
         * </summary>
         */
        public static Boolean LinesIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            return LinesIntersect(a1, a2, b1, b2, 0.1f);
        }

        /*
         * <summary> Check whether two liones intersect or not with the input
         * delta value.
         * </summary>
         */
        public static Boolean LinesIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float delta)
        {
            if (LinesCannotOverlap(a1, a2, b1, b2))
                return false;
            else if (BothLinesHorizontal(a1, a2, b1, b2, delta))
                return OverlapsHorizontally(a1, a2, b1, b2, delta);
            if (LineIsVertical(a1, a2, delta))
                return LinesIntersectVertical(a1, a2, b1, b2, delta);
            else if (LineIsVertical(b1, b2, delta))
                return LinesIntersectVertical(b1, b2, a1, a2, delta);
            else
                return LinesIntersectNonVertically(a1, a2, b1, b2, delta);
        }

        /*
         * <summary> Handles intersection where at least one of the lines is
         * vertical.
         * </summary>
         */
        private static Boolean LinesIntersectVertical(Vector2 v1, Vector2 v2, Vector2 a1, Vector2 a2, float delta)
        {
            if (LineIsVertical(a1, a2, delta))  //Handles dx==0
            {
                if (v1.X - delta < a1.X && a1.X < v1.X + delta)
                    return OverlapsVertically(v1, v2, a1, a2, delta) || OverlapsVertically(a1, a2, v1, v2, delta);
                return false;
            }

            //Confusingly, minYV is at the top of the screen, maxYV is at the bottom
            float min = (float)(Math.Min(v1.Y, v2.Y)) - delta;
            float max = (float)(Math.Max(v1.Y, v2.Y)) + delta;
            float xOfVerticalLine = v1.X;

            float dy = a2.Y - a1.Y;
            float dx = a2.X - a1.X;

            //Y = a * x + b
            float a = dy / dx;
            float b = a1.Y - a * a1.X;

            float yCross = a * xOfVerticalLine + b;

            return min < yCross && yCross < max;
        }

        private static Boolean LinesCannotOverlap(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            return LinesDoNotOverlapHorizontally(a1, a2, b1, b2) ||
                LinesDoNotOverlapVertically(a1, a2, b1, b2);
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
         * <summary> Only valid for lines that are not vertical, and where at 
         * least one line is not horizontal
         * </summary>
         */
        private static Boolean LinesIntersectNonVertically(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float delta)
        {
            float dyA = a2.Y - a1.Y;
            float dyB = b2.Y - b1.Y;
            float dxA = a2.X - a1.X;
            float dxB = b2.X - b1.X;

            //YA = aA * x + bA
            //YB = aB * x + bB
            float aA = dyA / dxA;
            float bA = a2.Y - aA * a2.X;
            float aB = dyB / dxB;
            float bB = b2.Y - aB * b2.X;

            if (aA - delta < aB && aB < aA + delta)
                if (bA - delta < bB && bB < bA + delta)
                    return OverlapsSlanted(a1, a2, b1, b2, delta);
                else
                    return false;

            //YA = YB
            //aA * x + bA = aB * x + bB
            //(aA - aB) * x = bB - bA
            //x = (bB - bA) / (aA - aB)
            float xIntersection = (bB - bA) / (aA - aB);
            float yIntersection = aA * xIntersection + bA;

            float minXA = Min(a1.X, a2.X) - delta;
            float maxXA = Max(a1.X, a2.X) + delta;
            float minYA = Min(a1.Y, a2.Y) - delta;
            float maxYA = Max(a1.Y, a2.Y) + delta;

            float minXB = Min(b1.X, b2.X) - delta;
            float maxXB = Max(b1.X, b2.X) + delta;
            float minYB = Min(b1.Y, b2.Y) - delta;
            float maxYB = Max(b1.Y, b2.Y) + delta;

            return ((minXA < xIntersection && xIntersection < maxXA) &&
                (minYA < yIntersection && yIntersection < maxYA)) &&
                ((minXB < xIntersection && xIntersection < maxXB) &&
                (minYB < yIntersection && yIntersection < maxYB));
        }

        private static Boolean OverlapsVertically(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float delta)
        {
            if (a1.X - delta < b1.X && b1.X < a1.X + delta)
            {
                float minYA = Min(a1.Y, a2.Y) - delta;
                float maxYA = Max(a1.Y, a2.Y) + delta;
                if ((minYA < b1.Y && b1.Y < maxYA) || (minYA < b2.Y && b2.Y < maxYA))
                    return true;
                float minYB = Min(b1.Y, b2.Y) - delta;
                float maxYB = Max(b1.Y, b2.Y) + delta;
                if ((minYB < a1.Y && a1.Y < maxYB) || (minYB < a2.Y && a2.Y < maxYB))
                    return true;
            }
            return false;
        }

        private static Boolean OverlapsHorizontally(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float delta)
        {
            if (a1.Y - delta < b1.Y && b1.Y < a1.Y + delta)
            {
                float minXA = Min(a1.X, a2.X) - delta;
                float maxXA = Max(a1.X, a2.X) + delta;
                if ((minXA < b1.X && b1.X < maxXA) || (minXA < b2.X && b2.X < maxXA))
                    return true;
                float minXB = Min(b1.X, b2.X) - delta;
                float maxXB = Max(b1.X, b2.X) + delta;
                if ((minXB < a1.X && a1.X < maxXB) || (minXB < a2.X && a2.X < maxXB))
                    return true;
            }
            return false;
        }

        private static Boolean OverlapsSlanted(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float delta)
        {
            if (LineIsHorizontal(a1, a2, delta) || 
                LineIsHorizontal(b1, b2, delta) || 
                LineIsVertical(a1, a2, delta) || 
                LineIsVertical(b1, b2, delta))
                return false;

            float minXA = Min(a1.X, a2.X) - delta;
            float maxXA = Max(a1.X, a2.X) + delta;
            float minYA = Min(a1.Y, a2.Y) - delta;
            float maxYA = Max(a1.Y, a2.Y) + delta;

            if (((minXA < b1.X && b1.X < maxXA) && (minYA < b1.Y && b1.Y < maxYA)) ||
                ((minXA < b2.X && b2.X < maxXA) && (minYA < b2.Y && b2.Y < maxYA)))
                return true;

            float minXB = Min(b1.X, b2.X) - delta;
            float maxXB = Max(b1.X, b2.X) + delta;
            float minYB = Min(b1.Y, b2.Y) - delta;
            float maxYB = Max(b1.Y, b2.Y) + delta;

            if (((minXB < a1.X && a1.X < maxXB) && (minYA < a1.Y && a1.Y < maxXB)) ||
                ((minXB < a2.X && a2.X < maxXB) && (minYB < a2.Y && a2.Y < maxYB)))
                return true;

            return false;
        }

        private static Boolean BothLinesHorizontal(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float delta)
        {
            return LineIsHorizontal(a1, a2, delta) && LineIsHorizontal(b1, b2, delta);
        }

        public static Boolean LineIsHorizontal(Vector2 a1, Vector2 a2, float delta)
        {
            float dy = a2.Y - a1.Y;
            return -delta < dy && dy < delta;
        }

        public static Boolean LineIsVertical(Vector2 a1, Vector2 a2, float delta)
        {
            float dx = a2.X - a1.X;
            return -delta < dx && dx < delta;
        }

        //----------------- Math Methods -----------------\\
        private static float Min(float a, float b)
        {
            return a < b ? a : b;
        }

        private static float Max(float a, float b)
        {
            return a > b ? a : b;
        }
    }
}
