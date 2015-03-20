using SuperPong.MJFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.Powerups
{
    public class Powerup : MJNode
    {

        protected List<PowerupObserver> observers;
        public Timer Timer { get; set; }
        public HitCounter HitCounter { get; set; }

        public Powerup(MJSprite sprite, MJPhysicsBody body)
            :this()
        {
            AddChild(sprite);
            AttachPhysicsBody(body);
        }

        public Powerup()
            : base()
        {
            observers = new List<PowerupObserver>();
            Timer = new Timer();
            HitCounter = new HitCounter();
        }

        public void StopAndRemovePowerup()
        {
            NotifyAllOfEnd();
            RemoveFromParent();
        }

        public void NotifyAllOfEnd()
        {
            Timer.Remove();
            HitCounter.Remove();
            foreach (PowerupObserver observer in observers)
            {
                observer.NotifyPowerupEnded(this);
            }
            RemoveFromParent();
            DetachPhysicsBodySafely();
        }

        public void AddObserver(PowerupObserver newObserver)
        {
            if (!observers.Contains(newObserver))
            {
                observers.Add(newObserver);
            }
        }

        public void RemoveObserver(PowerupObserver observer)
        {
            if (observers.Contains(observer))
            {
                observers.Remove(observer);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            Timer.Update(gameTime);
        }
    }
}
