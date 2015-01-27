using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using SuperPong.MJFrameWork;

namespace SuperPong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MJSprite sprite, sprite2, sprite3;
        MJSceneManager sceneManager;
        int elapsedButtonPressTime = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            sceneManager = new MJSceneManager(new FirstScene(Content));
            sceneManager.AddScene(new SecondScene(Content));
            Console.WriteLine("Constructor");
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            Console.WriteLine("Initialize");
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sceneManager.MainScene.LoadContent();
            Console.WriteLine("LoadContent");
            //Texture2D texture = Content.Load<Texture2D>("ball");
            
            //sprite = new MJSprite(texture);
            //sprite.Name = "sprite";
            //sprite.Position = new Vector2(200, 200);
            ////sprite.Rotation = 3.14f;
            
            //sprite2 = new MJSprite(texture);
            //sprite2.Name = "sprite2";
            //sprite2.ColorTint = Color.Blue;
            //sprite2.Position = new Vector2(100, 100);
            //sprite.AddChild(sprite2);
            
            //sprite3 = new MJSprite(texture);
            //sprite3.Name = "sprite3";
            //sprite3.ColorTint = Color.Green;
            //sprite3.Position = new Vector2(-50, 30);
            //sprite2.AddChild(sprite3);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
            sceneManager.Update(gameTime);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (elapsedButtonPressTime == 0)
                {
                    if (sceneManager.MainScene == sceneManager.GetSceneNamed("FirstScene"))
                    {
                        sceneManager.TransitionTo(sceneManager.GetSceneNamed("SecondScene"));
                    }
                    else
                    {
                        sceneManager.TransitionTo(sceneManager.GetSceneNamed("FirstScene"));
                    }
                }
                elapsedButtonPressTime += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedButtonPressTime > 200)
                {
                    elapsedButtonPressTime = 0;
                }
            }
            else
            {
                elapsedButtonPressTime = 0;
            }
            //sprite.Update(gameTime);
            //sprite.Rotation += 0.01f;
            //sprite2.Rotation -= 0.03f;
            //sprite.Position = new Vector2(sprite.Position.X + 1, sprite.Position.Y);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            spriteBatch.Begin();
            sceneManager.Draw(spriteBatch);
            //sprite.Draw(spriteBatch);
            //sprite2.Draw(spriteBatch);
            //sprite3.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
