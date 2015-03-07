using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.MJFrameWork;
using SuperPong.MJFrameWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.Powerups
{
    public class PowerupManager : MJUpdate, MJDraw, ResetPoint, ResetGame, MJPhysicsEventListener
    {

        ContentManager content;
        Dictionary<PowerupType, Texture2D> floatingTextures;
        
        Random randomGenerator;
        int timeLeftToNextPowerup;
        const int MAXTIMEBETWEENPOWERUPS = 5000;   //milliseconds

        FloatingPowerup floatingPowerup;
        Boolean shouldRemoveFloatingPowerup = false;
        Vector2 initialPosition;

        public PowerupManager(ContentManager content, int width, int height)
        {
            this.content = content;
            randomGenerator = new Random(2);
            floatingTextures = new Dictionary<PowerupType, Texture2D>();
            initialPosition = new Vector2(width / 2, height / 2);
            timeLeftToNextPowerup = GenerateNextTimeToPowerup();
            MJPhysicsManager.getInstance().AddListener(this);
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
                if (floatingPowerup == null)
                {
                    PowerupType nextType = GenerateNextPowerupType();
                    floatingPowerup = new FloatingPowerup(floatingTextures[nextType], nextType, randomGenerator, initialPosition);
                }
            }

            if (shouldRemoveFloatingPowerup)
            {
                shouldRemoveFloatingPowerup = false;
                floatingPowerup.DetachPhysicsBody();
                floatingPowerup = null;
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
            timeLeftToNextPowerup = GenerateNextTimeToPowerup();
            if (floatingPowerup != null)
            {
                floatingPowerup.DetachPhysicsBody();
                floatingPowerup = null;
            }
        }

        public void ResetPoint()
        {
            
        }

        public void CollisionBegan(MJCollisionPair pair)
        {
            
        }

        public void CollisionEnded(MJCollisionPair pair)
        {
            
        }

        public void IntersectionBegan(MJCollisionPair pair)
        {
            if (CaughtFloatingPowerup(pair))
            {
                shouldRemoveFloatingPowerup = true;
            }
        }

        public void IntersectionEnded(MJCollisionPair pair)
        {
            
        }

        private Boolean CaughtFloatingPowerup(MJCollisionPair pair)
        {
            if (pair.Body1 != null && pair.Body2 != null && floatingPowerup != null)
                return pair.Body1 == floatingPowerup.PhysicsBody || pair.Body2 == floatingPowerup.PhysicsBody;
            return false;
        }

    }
}
