using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork
{
    public class MJCollision
    {

        public static Boolean MJLinesCross(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            return MJLinesCross(a1, a2, b1, b2, 0.1f);
        }

        public static Boolean MJLinesCross(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float delta)
        {
            if (bothLinesHorizontal(a1, a2, b1, b2, delta))
                return MJHorizontalOverlap(a1, a2, b1, b2, delta);
            if (lineIsVertical(a1, a2, delta))
                return MJLinesCrossVertical(a1, a2, b1, b2, delta);
            else if (lineIsVertical(b1, b2, delta))
                return MJLinesCrossVertical(b1, b2, a1, a2, delta);
            else
                return MJLinesCrossDef(a1, a2, b1, b2, delta);
        }

        public static Boolean MJLinesCrossVertical(Vector2 v1, Vector2 v2, Vector2 a1, Vector2 a2, float delta)
        {
            if (LinesDoNotOverlapVertically(v1, v2, a1, a2) ||
                LinesDoNotOverlapHorizontally(v1, v2, a1, a2))
                return false;

            if (lineIsVertical(a1, a2, delta))  //Handles dx==0
            {
                if (v1.X - delta < a1.X && a1.X < v1.X + delta)
                    return MJVerticalOverlap(v1, v2, a1, a2, delta) || MJVerticalOverlap(a1, a2, v1, v2, delta);
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

        public static Boolean MJLinesCrossDef(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float delta)
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
                    return slantedLinesOverlaps(a1, a2, b1, b2, delta);
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

        public static Boolean MJVerticalOverlap(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float delta)
        {
            if (lineIsVertical(a1, a2, delta) && lineIsVertical(b1, b2, delta))
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
            }
            return false;
        }

        public static Boolean MJHorizontalOverlap(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float delta)
        {
            if (lineIsHorizontal(a1, a2, delta) && lineIsHorizontal(b1, b2, delta))
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
            }
            return false;
        }

        public static Boolean slantedLinesOverlaps(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float delta)
        {
            if (lineIsHorizontal(a1, a2, delta) || 
                lineIsHorizontal(b1, b2, delta) || 
                lineIsVertical(a1, a2, delta) || 
                lineIsVertical(b1, b2, delta))
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

        public static Boolean bothLinesHorizontal(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float delta)
        {
            return lineIsHorizontal(a1, a2, delta) && lineIsHorizontal(b1, b2, delta);
        }

        public static Boolean lineIsHorizontal(Vector2 a1, Vector2 a2, float delta)
        {
            float dy = a2.Y - a1.Y;
            return -delta < dy && dy < delta;
        }

        public static Boolean lineIsVertical(Vector2 a1, Vector2 a2, float delta)
        {
            float dx = a2.X - a1.X;
            return -delta < dx && dx < delta;
        }

        public static float Min(float a, float b)
        {
            return a < b ? a : b;
        }

        public static float Max(float a, float b)
        {
            return a > b ? a : b;
        }
    }
}
