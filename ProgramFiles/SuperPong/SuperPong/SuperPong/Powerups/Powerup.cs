using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.Powerups
{
    public interface Powerup
    {
        void UsePowerup();
        void StopPowerup();
        void Update(Microsoft.Xna.Framework.GameTime gameTime);
    }
}
