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

        public static void BounceObjects(MJPhysicsBody body1, MJPhysicsBody body2, Vector2 unitNormal, Vector2 unitTangent)
        {
            //Project each body's onto normal and tangent
            float v1n = body1.Velocity.X * unitNormal.X + body1.Velocity.Y * unitNormal.Y;
            float v1t = body1.Velocity.X * unitTangent.X + body1.Velocity.Y * unitTangent.Y;
            float v2n = body2.Velocity.X * unitNormal.X + body2.Velocity.Y * unitNormal.Y;
            float v2t = body2.Velocity.X * unitTangent.X + body2.Velocity.Y * unitTangent.Y;

            float v1tFinal = v1t;
            float v2tFinal = v2t;

            float v1nFinal = (v1n * (body1.Mass - body2.Mass) + 2 * v2n * body2.Mass) / (body1.Mass + body2.Mass);
            float v2nFinal = (v2n * (body2.Mass - body1.Mass) + 2 * v1n * body1.Mass) / (body1.Mass + body2.Mass);

            Console.WriteLine("v1n: " + v1n);
            Console.WriteLine("v2n: " + v2n);
            Console.WriteLine("v1nFinal: " + v1nFinal);
            Console.WriteLine("v2nFinal: " + v2nFinal);

            Vector2 v1nFinalVector = unitNormal * v1nFinal;
            Vector2 v1tFinalVector = unitTangent * v1tFinal;
            Vector2 v2nFinalVector = unitNormal * v2nFinal;
            Vector2 v2tFinalVector = unitTangent * v2tFinal;

            Vector2 v1Final = v1nFinalVector + v1tFinalVector;
            Vector2 v2Final = v2nFinalVector + v2tFinalVector;

            Console.WriteLine("v1Final: " + v1Final);
            Console.WriteLine("v2Final: " + v2Final);

            body1.VelocityTemp += v1Final;
            body2.VelocityTemp += v2Final;

            body1.Velocity += v1Final;
            body2.Velocity += v2Final;
        }
    }
}
