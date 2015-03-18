using SuperPong.MJFrameWork;
using SuperPong.MJFrameWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.Powerups
{
    public class HitCounter : MJPhysicsEventListener
    {

        Powerup powerup;
        int hitsLeft;

        public HitCounter()
        {

        }

        public HitCounter(int hits, Powerup powerup)
        {
            hitsLeft = hits;
            this.powerup = powerup;
            MJPhysicsManager.getInstance().AddListenerSafely(this);
        }

        public void Remove()
        {
            MJPhysicsManager.getInstance().RemoveListenerSafely(this);
            powerup = null;
        }

        public void CollisionBegan(MJFrameWork.MJIntersection pair)
        {
            if (pair.Body1 == powerup.PhysicsBody || pair.Body2 == powerup.PhysicsBody)
            {
                hitsLeft--;
                if (hitsLeft == 0)
                {
                    powerup.NotifyAllOfEnd();                    
                }
            }
        }

        public void CollisionEnded(MJFrameWork.MJIntersection pair)
        {
        }

        public void IntersectionBegan(MJFrameWork.MJIntersection pair)
        {
        }

        public void IntersectionEnded(MJFrameWork.MJIntersection pair)
        {
        }
    }
}
