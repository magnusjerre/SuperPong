using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperPong.MJFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong
{
    public class StartMenuScene : MJScene
    {

        public int Height { get; set; }
        public int Width { get; set; }
        Vector2 center;

        SpriteFont font;
        Font startGameFont;
        const string START_GAME = "START GAME";
        InputHandler inputHandler;

        public StartMenuScene(int height, int width)
            : base("START_MENU")
        {
            Height = height;
            Width = width;
            center = new Vector2(width / 2, height / 2);
        }

        public override void Initialize()
        {
            base.Initialize();
            startGameFont = new Font(font, new Microsoft.Xna.Framework.Vector2(0.5f, 0.5f));
            startGameFont.Text = START_GAME;
        }

        public override void LoadContent()
        {
            font = Content.Load<SpriteFont>("TheSpriteFont");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start))
            {
                MJSceneManager.GetInstance().PushScene(new GameScene(Height, Width));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.M))
                MJSceneManager.GetInstance().PushScene(new GameScene(Height, Width));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(startGameFont.SpriteFont, startGameFont.Text, center, Color.Green, 0f, startGameFont.TextOffset, 1f, SpriteEffects.None, 1f);
        }

    }
}
