using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.Powerups
{
    public class Timer
    {
        Powerup powerup;
        int millisecondsLeft;

        public Timer()
        {

        }

        public Timer(int milliseconds, Powerup powerup)
        {
            this.millisecondsLeft = milliseconds;
            this.powerup = powerup;
        }

        public void Update(GameTime gameTime)
        {
            if (powerup == null)
                return;
            
            millisecondsLeft -= gameTime.ElapsedGameTime.Milliseconds;
            if (millisecondsLeft < 1)
            {
                powerup.NotifyAllOfEnd();
            }
        }

        public void Remove()
        {
            powerup = null;
        }
    }
}
