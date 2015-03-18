using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork
{
    public class MJIntersection
    {

        private MJPhysicsBody body1;
        public MJPhysicsBody Body1 { get { return this.body1; } }

        private MJPhysicsBody body2;
        public MJPhysicsBody Body2 { get { return this.body2; } }

        public Boolean Intersects { get; set; }
        public Vector2 Normal { get; set; }
        public float Mtv { get; set; }

        public MJIntersection(MJPhysicsBody body1, MJPhysicsBody body2)
            : this(true, Vector2.Zero, 0f, body1, body2)
        {
        }

        public MJIntersection(Boolean intersects, Vector2 normal, float mtv, MJPhysicsBody body1, MJPhysicsBody body2)
        {
            Intersects = intersects;
            Normal = normal;
            Mtv = mtv;

            this.body1 = body1;
            this.body2 = body2;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() == typeof(MJIntersection))
            {
                MJIntersection other = (MJIntersection)obj;
                if (other.Body1 == Body1 && other.Body2 == Body2 && other.Intersects == Intersects && other.Normal == Normal && other.Mtv == Mtv)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
