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
    public class PowerupManager : MJUpdate, ResetPoint, ResetGame, PowerupObserver
    {
        GameScene scene;
        ContentManager content;
        Dictionary<string, Texture2D> floatingTextures;
        
        Random randomGenerator;
        int timeLeftToNextPowerup;
        const int MAXTIMEBETWEENPOWERUPS = 5000;   //milliseconds
        Boolean canAddNewPowerup = true;

        Vector2 initialPositionFLoatingPowerup;

        PowerupDisplay player1PowerupDisplay, player2PowerupDisplay;
        Vector2 player1PowerupDisplayPosition, player2PowerupDisplayPosition;
        Powerup player1Powerup, player2Powerup;

        PowerupFactory factory;

        public PowerupManager(GameScene scene, ContentManager content, int width, int height)
        {
            this.scene = scene;
            this.content = content;
            factory = new PowerupFactory(content);
            randomGenerator = new Random(2);
            floatingTextures = new Dictionary<string, Texture2D>();
            initialPositionFLoatingPowerup = new Vector2(width / 2, height / 2);
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
            floatingTextures.Add(PowerupType.LINE.ToString(), content.Load<Texture2D>("floating-powerup-line"));
            floatingTextures.Add(PowerupType.SQUARE.ToString(), content.Load<Texture2D>("floating-powerup-square"));
            floatingTextures.Add("frame", content.Load<Texture2D>("frame"));
            factory.LoadContent();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            timeLeftToNextPowerup -= gameTime.ElapsedGameTime.Milliseconds;
            if (timeLeftToNextPowerup < 0 && canAddNewPowerup)
            {
                timeLeftToNextPowerup = GenerateNextTimeToPowerup();
                PowerupType nextType = GenerateNextPowerupType();
                scene.AddToGameLayer(new FloatingPowerup(this, floatingTextures[nextType.ToString()], nextType, randomGenerator, initialPositionFLoatingPowerup));
                canAddNewPowerup = false;
            }
        }

        public void ResetGame()
        {
            timeLeftToNextPowerup = GenerateNextTimeToPowerup();

            if (player1Powerup != null)
            {
                player1Powerup.StopAndRemovePowerup();
                player1Powerup = null;
            }

            if (player1PowerupDisplay != null)
            {
                player1PowerupDisplay.RemoveFromParent();
                player1PowerupDisplay = null;
            }

            if (player2Powerup != null)
            {
                player2Powerup.StopAndRemovePowerup();
                player2Powerup = null;
            }

            if (player2PowerupDisplay != null)
            {
                player2PowerupDisplay.RemoveFromParent();
                player2PowerupDisplay = null;
            }


        }

        public void ResetPoint()
        {
        }

        public void NotifyPowerupCaught(MJPhysicsBody otherBody, FloatingPowerup floatingPowerup) {
            floatingPowerup.RemoveFromParent();
            canAddNewPowerup = true;
            if (Player1CaughtPowerup(otherBody))
            {
                if (player1PowerupDisplay != null)
                    player1PowerupDisplay.RemoveFromParent();

                player1PowerupDisplay = new PowerupDisplay(floatingTextures["frame"], floatingTextures[floatingPowerup.PowerupType.ToString()], floatingPowerup.PowerupType);
                player1PowerupDisplay.Position = player1PowerupDisplayPosition;
                scene.AddToGameLayer(player1PowerupDisplay);
            }
            else if (Player2CaughtPowerup(otherBody))
            {
                if (player2PowerupDisplay != null)
                    player2PowerupDisplay.RemoveFromParent();
                
                player2PowerupDisplay = new PowerupDisplay(floatingTextures["frame"], floatingTextures[floatingPowerup.PowerupType.ToString()], floatingPowerup.PowerupType);
                player2PowerupDisplay.Position = player2PowerupDisplayPosition;
                scene.AddToGameLayer(player2PowerupDisplay);
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

        public void NotifyPowerupEnded(Powerup powerup)
        {
        }

        public void UsePowerup(Player player, Vector2 rightStickPosition)
        {
            if (player.Name.Equals("PaddleLeft") && player1PowerupDisplay != null)
            {
                if (player1Powerup != null)
                    player1Powerup.StopAndRemovePowerup();

                player1Powerup = factory.CreatePowerup(player1PowerupDisplay.PowerupType);
                if (player1PowerupDisplay.PowerupType == PowerupType.LINE)
                    player1Powerup.Position = new Vector2(10, GameScene.Height / 2);
                else
                    player1Powerup.Position = rightStickPosition;
                scene.AddToGameLayer(player1Powerup);

                player1PowerupDisplay.RemoveFromParent();
                player1PowerupDisplay = null;
            }

            else if (player.Name.Equals("PaddleRight") && player2PowerupDisplay != null)
            {
                if (player2Powerup != null)
                    player2Powerup.StopAndRemovePowerup();

                player2Powerup = factory.CreatePowerup(player2PowerupDisplay.PowerupType);
                if (player2PowerupDisplay.PowerupType == PowerupType.LINE)
                    player2Powerup.Position = new Vector2(GameScene.Width - 10, GameScene.Height / 2);
                else
                    player2Powerup.Position = rightStickPosition;
                scene.AddToGameLayer(player2Powerup);

                player2PowerupDisplay.RemoveFromParent();
                player2PowerupDisplay = null;
            }


        }

    }
}
