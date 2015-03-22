using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.MJFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong
{
    public class ReplayGameNode : MJNode
    {

        Font replay, yes, no;
        Vector2 replayPos = new Vector2(0, -50);
        Vector2 yesPos = new Vector2(-50, 50);
        Vector2 noPos = new Vector2(50, 50);

        public ReplayGameNode(Texture2D aButtonTexture, Texture2D bButtonTexture, Texture2D backgroundTexture, SpriteFont font) : base()
        {
            MJSprite background = new MJSprite(backgroundTexture);
            background.Alpha = 0.5f;
            MJSprite aButton = new MJSprite(aButtonTexture);
            MJSprite bButton = new MJSprite(bButtonTexture);

            aButton.Position = new Vector2(-50, 25);
            bButton.Position = new Vector2(50, 25);

            replay = new Font(font, new Vector2(0.5f, 0.5f));
            replay.Text = "REPLAY?";

            yes = new Font(font, new Vector2(0.5f, 0.5f));
            yes.Text = "yes";

            no = new Font(font, new Vector2(0.5f, 0.5f));
            no.Text = "no";

            AddChild(background);
            AddChild(aButton);
            AddChild(bButton);

            Position = new Vector2(GameScene.Width / 2, GameScene.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(replay.SpriteFont, replay.Text, Position + replayPos, Color.Black, 0f, replay.TextOffset, 0.75f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(yes.SpriteFont, yes.Text, Position + yesPos, Color.Black, 0f, yes.TextOffset, 0.75f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(no.SpriteFont, no.Text, Position + noPos, Color.Black, 0f, no.TextOffset, 0.75f, SpriteEffects.None, 1f);
        }
    }
}
