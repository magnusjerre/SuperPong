using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.MJFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.Powerups
{
    public class PowerupFactory
    {

        ContentManager content;
        Dictionary<String, Texture2D> textures;

        public PowerupFactory(ContentManager content)
        {
            this.content = content;
            textures = new Dictionary<string, Texture2D>();
        }

        public void LoadContent()
        {
            textures.Add(PowerupType.LINE.ToString(), content.Load<Texture2D>("line"));
            textures.Add(PowerupType.SQUARE.ToString(), content.Load<Texture2D>("square"));
        }

        public Powerup CreatePowerup(PowerupType type) {
            if (type == PowerupType.LINE) 
            {
                return CreateLinePowerup();
            }
            else if (type == PowerupType.SQUARE)
            {
                return CreateSquarePowerup();
            }

            return null;
        }

        public Powerup CreateLinePowerup()
        {
            MJSprite sprite = new MJSprite(textures[PowerupType.LINE.ToString()]);
            sprite.ColorTint = Color.Yellow;
            MJPhysicsBody body = MJPhysicsBody.RectangularMJPhysicsBody(sprite.Size, sprite.GetOrigin());
            body.Bitmask = Bitmasks.WALL;
            body.CollisionMask = Bitmasks.BALL | Bitmasks.POWERUP;
            body.IsStatic = true;
            Powerup powerup = new Powerup(sprite, body);
            powerup.HitCounter = new HitCounter(1, powerup);

            return powerup;
        }

        public Powerup CreateSquarePowerup()
        {
            MJSprite sprite = new MJSprite(textures[PowerupType.SQUARE.ToString()]);
            sprite.ColorTint = Color.Red;
            MJPhysicsBody body = MJPhysicsBody.RectangularMJPhysicsBody(sprite.Size, sprite.GetOrigin());
            body.Bitmask = Bitmasks.WALL;
            body.CollisionMask = Bitmasks.BALL | Bitmasks.POWERUP;
            body.RotationalSpeed = 1f;
            body.IsStatic = true;
            Powerup powerup = new Powerup(sprite, body);
            powerup.HitCounter = new HitCounter(3, powerup);
            
            return powerup;
        }
    }
}
