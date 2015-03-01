using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace SuperPong
{
    public class ScoreFont : ScoreObserver, ResetGame
    {
        private const int divisorForYLocation = 30;
        private Vector2 centerForTextPlacement;

        Font LeftPlayerScore { get; set; }
        Font RightPlayerScore { get; set; }

        Vector2 LeftPlayerPosition { get; set; }
        Vector2 RightPlayerPosition { get; set; }

        public ScoreFont(Vector2 screenBounds, SpriteFont spriteFont)
        {
            centerForTextPlacement = new Vector2(screenBounds.X / 2, screenBounds.Y / divisorForYLocation);
            LeftPlayerPosition = new Vector2(centerForTextPlacement.X - 20, centerForTextPlacement.Y);
            RightPlayerPosition = new Vector2(centerForTextPlacement.X + 20, centerForTextPlacement.Y);
            
            LeftPlayerScore = new Font(spriteFont, new Vector2(1, 0));
            RightPlayerScore = new Font(spriteFont, new Vector2(0, 0));

            LeftPlayerScore.Text = "" + 0;
            RightPlayerScore.Text = "" + 0;
        }

        public void NotifyMaxScoreReached(string playerReachingMaxScore)
        {
            if (playerReachingMaxScore.Equals(ScoreKeeper.LEFT_PLAYER))
            {
                LeftPlayerScore.Text = "Winner!";
                RightPlayerScore.Text = "Loser...";
            }
            else if (playerReachingMaxScore.Equals(ScoreKeeper.RIGHT_PLAYER))
            {
                LeftPlayerScore.Text = "Loser...";
                RightPlayerScore.Text = "Winner!";
            }
        }

        public void NotifyLeftPlayerScored(int newScore)
        {
            LeftPlayerScore.Text = "" + newScore;
        }

        public void NotifyRightPlayerScored(int newScore)
        {
            RightPlayerScore.Text = "" + newScore;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(LeftPlayerScore.SpriteFont, LeftPlayerScore.Text, LeftPlayerPosition, LeftPlayerScore.Color, 0f, LeftPlayerScore.TextOffset, 1f, SpriteEffects.None, 1);
            spriteBatch.DrawString(RightPlayerScore.SpriteFont, RightPlayerScore.Text, RightPlayerPosition, RightPlayerScore.Color, 0f, RightPlayerScore.TextOffset, 1f, SpriteEffects.None, 1);
        }

        public void ResetGame()
        {
            LeftPlayerScore.Text = "" + 0;
            RightPlayerScore.Text = "" + 0;
        }
    }
}
