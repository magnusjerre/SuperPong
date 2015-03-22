using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.MJFrameWork;
using Microsoft.Xna.Framework;

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
            if (type == PowerupType.LINE)
                powerup.ColorTint = Color.Yellow;
            else
                powerup.ColorTint = Color.Red;
            AddChild(powerup);
            Name = "PowerupDisplay";
        }

    }
}
