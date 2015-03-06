using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.MJFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.Powerups
{
    public class FloatingPowerup : MJSprite
    {

        public PowerupType PowerupType { get; set; }

        public FloatingPowerup(Texture2D texture, PowerupType powerupType, Random randomGenerator, Vector2 position)
            : base(texture)
        {
            this.Name = "Powerup";
            this.Position = position;
            this.PowerupType = powerupType;
            MJPhysicsBody body = MJPhysicsBody.CircularMJPhysicsBody(texture.Bounds.Width / 2);
            body.Bitmask = Bitmasks.POWERUP;
            body.CollisionMask = Bitmasks.WALL;
            body.IntersectionMask = Bitmasks.PADDLE | Bitmasks.GOAL;
            body.Velocity = GenerateRandomInitialVelocity(randomGenerator);
            AttachPhysicsBody(body);
        }

        private Vector2 GenerateRandomInitialVelocity(Random randomGenerator)
        {
            int xDirection = randomGenerator.Next(201) - 100;
            int yDirection = randomGenerator.Next(201) - 100;
            float length = (float)Math.Sqrt(xDirection * xDirection + yDirection * yDirection);
            Vector2 unitVector = new Vector2(xDirection / length, yDirection / length);
            int speed = randomGenerator.Next(300) + 200;
            return unitVector * speed;
        }
    
    }
}
