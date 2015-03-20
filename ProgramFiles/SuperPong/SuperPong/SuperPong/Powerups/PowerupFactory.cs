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

        public Powerup CreatePowerup(PowerupType type, Player player, Vector2 rightStickPosition) {
            if (type == PowerupType.LINE) 
            {
                return CreateLinePowerup(player);
            }
            else if (type == PowerupType.SQUARE)
            {
                return CreateSquarePowerup(rightStickPosition);
            }

            return null;
        }

        public Powerup CreateLinePowerup(Player powerupOwner)
        {
            MJSprite sprite = new MJSprite(textures[PowerupType.LINE.ToString()]);
            MJPhysicsBody body = MJPhysicsBody.RectangularMJPhysicsBody(sprite.Size, sprite.GetOrigin());
            body.Bitmask = Bitmasks.WALL;
            body.CollisionMask = Bitmasks.BALL | Bitmasks.POWERUP;
            body.IsStatic = true;
            Powerup powerup = new Powerup(sprite, body);
            powerup.HitCounter = new HitCounter(1, powerup);

            if (powerupOwner.Name.Equals("PaddleLeft"))
                powerup.Position = new Vector2(powerupOwner.absoluteCoordinateSystem.Position.X - 100, GameScene.Height / 2);
            else if (powerupOwner.Name.Equals("PaddleRight"))
                powerup.Position = new Vector2(powerupOwner.absoluteCoordinateSystem.Position.X + 100, GameScene.Height / 2);

            return powerup;
        }

        public Powerup CreateSquarePowerup(Vector2 position)
        {
            MJSprite sprite = new MJSprite(textures[PowerupType.SQUARE.ToString()]);
            MJPhysicsBody body = MJPhysicsBody.RectangularMJPhysicsBody(sprite.Size, sprite.GetOrigin());
            body.Bitmask = Bitmasks.WALL;
            body.CollisionMask = Bitmasks.BALL | Bitmasks.POWERUP;
            body.RotationalSpeed = 1f;
            body.IsStatic = true;
            Powerup powerup = new Powerup(sprite, body);
            powerup.HitCounter = new HitCounter(3, powerup);
            powerup.Position = position;
            return powerup;
        }
    }
}
