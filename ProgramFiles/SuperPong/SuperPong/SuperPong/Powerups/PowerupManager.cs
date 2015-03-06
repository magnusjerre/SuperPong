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
    public class PowerupManager : MJUpdate, MJDraw, ResetPoint, ResetGame
    {

        ContentManager content;
        Dictionary<PowerupType, Texture2D> floatingTextures;
        
        Random randomGenerator;
        int timeLeftToNextPowerup;
        const int MAXTIMEBETWEENPOWERUPS = 5000;   //milliseconds

        FloatingPowerup floatingPowerup;
        Vector2 initialPosition;

        public PowerupManager(ContentManager content, int width, int height)
        {
            this.content = content;
            randomGenerator = new Random(2);
            floatingTextures = new Dictionary<PowerupType, Texture2D>();
            initialPosition = new Vector2(width / 2, height / 2);
            timeLeftToNextPowerup = GenerateNextTimeToPowerup();
        }

        private PowerupType GenerateNextPowerupType()
        {
            int numberOfEnums = Enum.GetNames(typeof(PowerupType)).Length;
            int nextEnum = randomGenerator.Next(numberOfEnums);
            return (PowerupType)nextEnum;
        }

        private int GenerateNextTimeToPowerup()
        {
            return randomGenerator.Next(MAXTIMEBETWEENPOWERUPS);
        }

        public void LoadContent()
        {
            floatingTextures.Add(PowerupType.LINE, content.Load<Texture2D>("line_floating_powerup"));
            floatingTextures.Add(PowerupType.SQUARE, content.Load<Texture2D>("square_floating_powerup"));
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            timeLeftToNextPowerup -= gameTime.ElapsedGameTime.Milliseconds;
            if (timeLeftToNextPowerup < 0)
            {
                timeLeftToNextPowerup = GenerateNextTimeToPowerup();
                PowerupType nextType = GenerateNextPowerupType();
                if (floatingPowerup != null)
                {
                    floatingPowerup.DetachPhysicsBody();
                    floatingPowerup = null;
                }
                else
                {
                    floatingPowerup = new FloatingPowerup(floatingTextures[nextType], nextType, randomGenerator, initialPosition);
                }
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (floatingPowerup != null)
            {
                floatingPowerup.Draw(spriteBatch);
            }
        }

        public void ResetGame()
        {
            ResetPoint();
        }

        public void ResetPoint()
        {
            timeLeftToNextPowerup = GenerateNextTimeToPowerup();
            if (floatingPowerup != null)
            {
                floatingPowerup.DetachPhysicsBody();
                floatingPowerup = null;
            }
        }
    }
}
