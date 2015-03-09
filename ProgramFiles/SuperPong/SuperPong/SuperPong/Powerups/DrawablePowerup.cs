using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.Powerups
{
    public interface DrawablePowerup : Powerup
    {
        void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch);
    }
}
