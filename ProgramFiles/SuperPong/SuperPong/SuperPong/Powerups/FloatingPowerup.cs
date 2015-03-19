using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.MJFrameWork;
using SuperPong.MJFrameWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.Powerups
{
    public class FloatingPowerup : MJSprite, MJPhysicsEventListener
    {

        public PowerupType PowerupType { get; set; }
        public PowerupManager manager;

        public FloatingPowerup(PowerupManager manager, Texture2D texture, PowerupType powerupType, Random randomGenerator, Vector2 position)
            : base(texture)
        {
            this.manager = manager;
            this.Name = "Powerup";
            this.Position = position;
            this.PowerupType = powerupType;
            MJPhysicsBody body = MJPhysicsBody.CircularMJPhysicsBody(texture.Bounds.Width / 2);
            body.Bitmask = Bitmasks.POWERUP;
            body.CollisionMask = Bitmasks.WALL;
            body.IntersectionMask = Bitmasks.PADDLE | Bitmasks.GOAL;
            body.Velocity = GenerateRandomInitialVelocity(randomGenerator);
            AttachPhysicsBody(body);
            MJPhysicsManager.getInstance().AddListenerSafely(this);
        }

        private Vector2 GenerateRandomInitialVelocity(Random randomGenerator)
        {
            int xDirection = randomGenerator.Next(201) - 100;
            if (-1 < xDirection && xDirection < 30)
                xDirection += 30;
            else if (-30 < xDirection && xDirection < 0)
                xDirection -= 30;
            int yDirection = randomGenerator.Next(201) - 100;
            float length = (float)Math.Sqrt(xDirection * xDirection + yDirection * yDirection);
            Vector2 unitVector = new Vector2(xDirection / length, yDirection / length);
            int speed = randomGenerator.Next(300) + 200;
            return unitVector * speed;
        }

        public void CollisionBegan(MJIntersection pair)
        {
        }

        public void IntersectionBegan(MJIntersection pair)
        {
            if (!IntersectionWithThis(pair))
                return;

            MJPhysicsBody otherBody = GetOtherBody(pair);
            MJPhysicsManager.getInstance().RemoveListenerSafely(this);
            DetachPhysicsBodySafely();
            manager.NotifyPowerupCaught(otherBody, this);
        }

        private Boolean IntersectionWithThis(MJIntersection pair) 
        {
            return pair.Body1 == PhysicsBody || pair.Body2 == PhysicsBody;
        }

        private MJPhysicsBody GetOtherBody(MJIntersection pair)
        {
            return pair.Body1 == PhysicsBody ? pair.Body2 : pair.Body1;
        }

        public void IntersectionEnded(MJIntersection pair)
        {   
        }
    }
}
