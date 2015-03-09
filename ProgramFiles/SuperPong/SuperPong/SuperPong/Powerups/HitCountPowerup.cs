using Microsoft.Xna.Framework;
using SuperPong.MJFrameWork;
using SuperPong.MJFrameWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.Powerups
{
    public class HitCountPowerup : DrawablePowerup, MJPhysicsEventListener
    {
        int hitsLeft;
        MJSprite sprite;
        MJPhysicsBody body;
        PowerupManager manager;

        public HitCountPowerup(MJSprite sprite, MJPhysicsBody body, PowerupManager manager)
        {
            this.sprite = sprite;
            this.sprite.Name = "Cake";
            this.body = body;
            hitsLeft = 1;
            this.manager = manager;
        }

        public void UsePowerup()
        {
            Vector2 position = new Vector2(750, 600);
            sprite.Position = position;
            sprite.AttachPhysicsBody(body);
            MJPhysicsManager.getInstance().AddListenerSafely(this);
        }

        public void StopPowerup()
        {
            sprite.DetachPhysicsBodySafely();
            MJPhysicsManager.getInstance().RemoveListenerSafely(this);
            manager.NotifyPowerupStopped(this);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }


        public void CollisionBegan(MJCollisionPair pair)
        {
            if (pair.Body1 == body || pair.Body2 == body)
            {
                hitsLeft--;
                if (hitsLeft == 0)
                {
                    StopPowerup();
                }
            }
        }

        public void CollisionEnded(MJCollisionPair pair)
        {
        }

        public void IntersectionBegan(MJCollisionPair pair)
        {
        }

        public void IntersectionEnded(MJCollisionPair pair)
        {
        }
    }
}
