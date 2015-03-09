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
    public class PowerupManager : MJUpdate, ResetPoint, ResetGame
    {

        ContentManager content;
        Dictionary<string, Texture2D> floatingTextures;
        
        Random randomGenerator;
        int timeLeftToNextPowerup;
        const int MAXTIMEBETWEENPOWERUPS = 5000;   //milliseconds
        Boolean canAddNewPowerup = true;

        Vector2 initialPosition;

        Powerup player1Powerup, player2Powerup;
        PowerupDisplay player1PowerupDisplay, player2PowerupDisplay;
        Vector2 player1PowerupDisplayPosition, player2PowerupDisplayPosition;

        MJScene scene;

        public PowerupManager(MJScene scene, ContentManager content, int width, int height)
        {
            this.scene = scene;
            this.content = content;
            randomGenerator = new Random(2);
            floatingTextures = new Dictionary<string, Texture2D>();
            initialPosition = new Vector2(width / 2, height / 2);
            timeLeftToNextPowerup = GenerateNextTimeToPowerup();
            player1PowerupDisplayPosition = new Vector2(200, 100);
            player2PowerupDisplayPosition = new Vector2(width - 200, 100);
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
            floatingTextures.Add(PowerupType.LINE.ToString(), content.Load<Texture2D>("line_floating_powerup"));
            floatingTextures.Add(PowerupType.SQUARE.ToString(), content.Load<Texture2D>("square_floating_powerup"));
            floatingTextures.Add("frame", content.Load<Texture2D>("frame"));
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            timeLeftToNextPowerup -= gameTime.ElapsedGameTime.Milliseconds;
            if (timeLeftToNextPowerup < 0 && canAddNewPowerup)
            {
                timeLeftToNextPowerup = GenerateNextTimeToPowerup();
                PowerupType nextType = GenerateNextPowerupType();
                scene.AddChild(new FloatingPowerup(this, floatingTextures[nextType.ToString()], nextType, randomGenerator, initialPosition));
                canAddNewPowerup = false;
            }
        }

        public void ResetGame()
        {
            timeLeftToNextPowerup = GenerateNextTimeToPowerup();
        }

        public void ResetPoint()
        {
            
        }
        
        public void NotifyPowerupStopped(Powerup powerup)
        {
            if (powerup == player1Powerup)
            {
                player1Powerup = null;
                player1PowerupDisplay = null;
            }
            
            else if (powerup == player2Powerup)
            {
                player2Powerup = null;
                player2PowerupDisplay = null;
            }
        }

        public void NotifyPowerupCaught(MJPhysicsBody otherBody, FloatingPowerup floatingPowerup) {
            floatingPowerup.RemoveFromParent();
            canAddNewPowerup = true;
            if (Player1CaughtPowerup(otherBody))
            {
                player1PowerupDisplay = new PowerupDisplay(floatingTextures["frame"], floatingTextures[floatingPowerup.PowerupType.ToString()]);
                player1PowerupDisplay.Position = player1PowerupDisplayPosition;
                scene.AddChild(player1PowerupDisplay);
            }
            else if (Player2CaughtPowerup(otherBody))
            {
                player2PowerupDisplay = new PowerupDisplay(floatingTextures["frame"], floatingTextures[floatingPowerup.PowerupType.ToString()]);
                player2PowerupDisplay.Position = player2PowerupDisplayPosition;
                scene.AddChild(player2PowerupDisplay);
            }            
        }

        private Boolean Player1CaughtPowerup(MJPhysicsBody otherBody)
        {
            return otherBody.Parent.Name.Equals("PaddleLeft");
        }

        private Boolean Player2CaughtPowerup(MJPhysicsBody otherBody)
        {
            return otherBody.Parent.Name.Equals("PaddleRight");
        }
    }
}
