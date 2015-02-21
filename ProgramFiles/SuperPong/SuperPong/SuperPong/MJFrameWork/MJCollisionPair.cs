using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork
{
    public class MJCollisionPair
    {
        private MJPhysicsBody body1;
        public MJPhysicsBody Body1 { get { return this.body1; } }

        private MJPhysicsBody body2;
        public MJPhysicsBody Body2 { get { return this.body2; } }

        public MJCollisionPair(MJPhysicsBody body1, MJPhysicsBody body2)
        {
            this.body1 = body1;
            this.body2 = body2;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() == typeof(MJCollisionPair))
            {
                MJCollisionPair other = (MJCollisionPair)obj;
                if (other.Body1 == Body1 && other.Body2 == Body2)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
