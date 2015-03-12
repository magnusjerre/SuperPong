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

        public PowerupType PowerupType { get; set; }

        public PowerupDisplay(Texture2D frameTexture, Texture2D powerupTexture, PowerupType type)
            : base(frameTexture)
        {
            PowerupType = type;
            MJSprite powerup = new MJSprite(powerupTexture);
            AddChild(powerup);
            Name = "PowerupDisplay";
        }

    }
}
