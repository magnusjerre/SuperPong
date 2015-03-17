﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using SuperPong.MJFrameWork.Interfaces;

namespace SuperPong.MJFrameWork
{
    public class MJPhysicsManager : MJUpdate
    {
        private static MJPhysicsManager singleton;

        public static MJPhysicsManager getInstance()
        {
            if (singleton == null)           
                singleton = new MJPhysicsManager();
            return singleton;
        }

        public List<MJPhysicsEventListener> listeners;
        public List<MJPhysicsEventListener> listenersToRemove;
        public List<MJPhysicsEventListener> listenersToAdd;

        public List<MJCollisionPair> collisionPairs;
        public List<MJCollisionPair> intersectionPairs;
        
        public List<MJPhysicsBody> allBodies;
        public List<MJPhysicsBody> bodiesToRemove;
        public List<MJPhysicsBody> bodiesToAdd;

        private MJPhysicsManager()
        {
            collisionPairs = new List<MJCollisionPair>();
            intersectionPairs = new List<MJCollisionPair>();
            allBodies = new List<MJPhysicsBody>();
            listeners = new List<MJPhysicsEventListener>();
            listenersToRemove = new List<MJPhysicsEventListener>();
            listenersToAdd = new List<MJPhysicsEventListener>();
            bodiesToRemove = new List<MJPhysicsBody>();
            bodiesToAdd = new List<MJPhysicsBody>();
        }

        private void AddListenersSafely()
        {
            foreach (MJPhysicsEventListener listener in listenersToAdd)
            {
                listeners.Add(listener);
            }
            listenersToAdd.Clear();
        }

        private void RemoveListenersSafely()
        {
            foreach (MJPhysicsEventListener listener in listenersToRemove)
            {
                listeners.Remove(listener);
            }
            listenersToRemove.Clear();
        }

        public void AddListenerSafely(MJPhysicsEventListener newListener)
        {
            if (!listeners.Contains(newListener))
            {
                listenersToAdd.Add(newListener);
            }
        }

        public void RemoveListenerSafely(MJPhysicsEventListener listener)
        {
            if (listeners.Contains(listener))
            {
                listenersToRemove.Add(listener);
            }
        }

        public void AddBodySafely(MJPhysicsBody body)
        {
            if (!allBodies.Contains(body))
            {
                bodiesToAdd.Add(body);
            }
        }

        private void AddBodiesSafely()
        {
            foreach (MJPhysicsBody body in bodiesToAdd)
            {
                allBodies.Add(body);
            }
            bodiesToAdd.Clear();
        }

        private void RemoveBodiesSafely()
        {
            foreach (MJPhysicsBody body in bodiesToRemove)
            {
                MJNode parent = body.Parent;
                parent.PhysicsBody = null;
                body.Parent = null;
                allBodies.Remove(body);
            }

            bodiesToRemove.Clear();
        }

        public void RemoveBodySafely(MJPhysicsBody body)
        {
            if (!bodiesToRemove.Contains(body))
            {
                bodiesToRemove.Add(body);
            }
        }

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

        /*
         * <summar>
         * Works for collision between two objects. I don't think it works correctly for a three-way collision.
         * </summary>
         */
        public static void BounceObjects(MJPhysicsBody body1, MJPhysicsBody body2, Vector2 unitNormal, Vector2 unitTangent)
        {
            //Project each body's onto normal and tangent
            float v1n = body1.Velocity.X * unitNormal.X + body1.Velocity.Y * unitNormal.Y;
            float v1t = body1.Velocity.X * unitTangent.X + body1.Velocity.Y * unitTangent.Y;
            float v2n = body2.Velocity.X * unitNormal.X + body2.Velocity.Y * unitNormal.Y;
            float v2t = body2.Velocity.X * unitTangent.X + body2.Velocity.Y * unitTangent.Y;

            float v1tFinal = v1t;
            float v2tFinal = v2t;

            float v1nFinal = 0f;
            float v2nFinal = 0f;
            if (body1.IsStatic)
            {
                v1nFinal = v1n;
                v2nFinal = 2 * v1n - v2n;
            }
            else if (body2.IsStatic)
            {
                v1nFinal = 2 * v2n - v1n;
                v2nFinal = v2n;
            }
            else
            {
                v1nFinal = (v1n * (body1.Mass - body2.Mass) + 2 * v2n * body2.Mass) / (body1.Mass + body2.Mass);
                v2nFinal = (v2n * (body2.Mass - body1.Mass) + 2 * v1n * body1.Mass) / (body1.Mass + body2.Mass);
            }

            Vector2 v1nFinalVector = unitNormal * v1nFinal;
            Vector2 v1tFinalVector = unitTangent * v1tFinal;
            Vector2 v2nFinalVector = unitNormal * v2nFinal;
            Vector2 v2tFinalVector = unitTangent * v2tFinal;

            Vector2 v1Final = v1nFinalVector + v1tFinalVector;
            Vector2 v2Final = v2nFinalVector + v2tFinalVector;

            body1.Velocity = v1Final;
            body2.Velocity = v2Final;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < allBodies.Count ; i++)
            {
                for (int j = i + 1; j < allBodies.Count; j++)
                {
                    //Verify that a check for collision is needed
                    //The collision/intersection bitmask must include both objects in both objects
                    MJPhysicsBody body1 = allBodies[i];
                    MJPhysicsBody body2 = allBodies[j];

                    if (body1.ToBeRemoved || body2.ToBeRemoved)
                        continue;
                        
                    if (body1.ShouldCheckForCollision(body2.Bitmask) || body1.ShouldCheckForIntersection(body2.Bitmask))
                    {
                        MJCollisionPair collisionPair = new MJCollisionPair(body1, body2);
                        MJIntersects intersects = MJInteresection.Collides(body1, body2);
                        //MJIntersects intersects = MJIntersection.Intersects(body1, body2);
                        if (intersects.Intersects)
                        {
                            if (body1.ShouldCheckForCollision(body2.Bitmask) && !collisionPairs.Contains(collisionPair))
                            {
                                collisionPairs.Add(collisionPair);
                                foreach (MJPhysicsEventListener listener in listeners)
                                {
                                    listener.CollisionBegan(collisionPair);
                                }
                                BounceObjects(body1, body2, intersects.Normal, new Vector2(-intersects.Normal.Y, intersects.Normal.X));
                            }

                            if (body1.ShouldCheckForIntersection(body2.Bitmask) && !intersectionPairs.Contains(collisionPair))
                            {
                                intersectionPairs.Add(collisionPair);
                                foreach (MJPhysicsEventListener listener in listeners)
                                {
                                    listener.IntersectionBegan(collisionPair);
                                }
                            }
                        }
                        else
                        {
                            if (body1.ShouldCheckForCollision(body2.Bitmask) && collisionPairs.Contains(collisionPair))
                            {
                                collisionPairs.Remove(collisionPair);
                                foreach (MJPhysicsEventListener listener in listeners)
                                {
                                    listener.CollisionEnded(collisionPair);
                                }
                            }

                            if (body1.ShouldCheckForIntersection(body2.Bitmask) && intersectionPairs.Contains(collisionPair))
                            {
                                intersectionPairs.Remove(collisionPair);
                                foreach (MJPhysicsEventListener listener in listeners)
                                {
                                    listener.IntersectionEnded(collisionPair);
                                }
                            }
                        }
                    }

                }
            }

            RemoveBodiesSafely();
            AddBodiesSafely();

            RemoveListenersSafely();
            AddListenersSafely();

            foreach (MJPhysicsBody body in allBodies)
            {
                body.Update(gameTime);
            }
        }
    }
}
