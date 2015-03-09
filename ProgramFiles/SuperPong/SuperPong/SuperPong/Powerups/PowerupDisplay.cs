using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.MJFrameWork;

namespace SuperPong.Powerups
{
    public class PowerupDisplay : MJSprite
    {

        public PowerupDisplay(Texture2D frameTexture, Texture2D powerupTexture)
            : base(frameTexture)
        {
            MJSprite powerup = new MJSprite(powerupTexture);
            AddChild(powerup);
            Name = "PowerupDisplay";
        }

    }
}
