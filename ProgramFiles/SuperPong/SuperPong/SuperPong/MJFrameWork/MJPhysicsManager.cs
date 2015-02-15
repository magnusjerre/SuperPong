using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SuperPong.MJFrameWork
{
    public class MJPhysicsManager
    {

        public static void ApplyForce(Vector2 force, MJPhysicsBody body)
        {
            Vector2 acceleration = force / body.Mass;
            body.AccelerationFromForce += acceleration;
        }

        public static void ApplyRotation(Vector2 force, Vector2 point, MJPhysicsBody body)
        {
            Vector2 pos = body.Parent.absoluteCoordinateSystem.Position;
            float angularAcceleartion = CalculateRotationalSpeed(force, point, pos, body.Mass);
            body.RotationalAcceleration = angularAcceleartion;
        }

        public static Vector2 GetPerpendicularVector(Vector2 vector)
        {
            //v1(x,y) -> v2 = (-v1y, v1x):> v2x = -v1y, v2y = v1.x
            //cos(theta) = (a * b) / (|a| * |b|) -> (a * b) == 0 since it is perpendicular
            //ax * bx + ay * by == 0 -> ax * -ay + ay * ax
            //i.e: bx = -ay and by = ax
            return new Vector2(-vector.Y, vector.X);
        }

        public static float CalculatedAngleBetween(Vector2 a, Vector2 b)
        {
            float ab = a.X * b.X + a.Y * b.Y;
            float ablength = a.Length() * b.Length();
            return (float)(Math.Acos(ab / ablength));
        }

        public static float CalculatedAngleBetweenNotAngle(Vector2 a, Vector2 b)
        {
            float ab = a.X * b.X + a.Y * b.Y;
            float ablength = a.Length() * b.Length();
            return ab / ablength;
        }

        public static float CalculatePerpendicularForce(Vector2 perpendicular, Vector2 force)
        {
            float angleIsh = CalculatedAngleBetweenNotAngle(perpendicular, force);
            return force.Length() * angleIsh;
        }

        public static float CalculateRotationalSpeed(Vector2 force, Vector2 point, Vector2 pos, float mass)
        {
            Vector2 diff = new Vector2(point.X - pos.X, point.Y - pos.Y);
            if (diff.Length() == 0)
                return 0;

            Vector2 perpendicular = GetPerpendicularVector(diff);
            float rotationalForce = CalculatePerpendicularForce(perpendicular, force);
            return rotationalForce / mass;
        }

    }
}
