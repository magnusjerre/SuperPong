using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork.Interfaces
{
    public interface MJPhysicsEventListener
    {
        void CollisionBegan(MJIntersection pair);
        void CollisionEnded(MJIntersection pair);

        void IntersectionBegan(MJIntersection pair);
        void IntersectionEnded(MJIntersection pair);
    }
}
