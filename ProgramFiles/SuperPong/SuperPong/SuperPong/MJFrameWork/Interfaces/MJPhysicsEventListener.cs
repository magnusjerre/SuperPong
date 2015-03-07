using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork.Interfaces
{
    public interface MJPhysicsEventListener
    {
        void CollisionBegan(MJCollisionPair pair);
        void CollisionEnded(MJCollisionPair pair);

        void IntersectionBegan(MJCollisionPair pair);
        void IntersectionEnded(MJCollisionPair pair);
    }
}
