using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.MJFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong
{
    public class Cursor : MJSprite
    {

        private Vector2 CursorDirection { get; set; }
        public float CursorSpeed = 600f;
        
        private int halfWidth;
        public int HalfWidth { get { return halfWidth; } }
        
        private int halfHeight;
        public int HalfHeight { get { return halfHeight; } }

        public Cursor(Texture2D texture)
            : base(texture)
        {
            halfWidth = (int)(Size.X / 2);
            halfHeight = (int) (Size.Y / 2);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Vector2 endPosition = absoluteCoordinateSystem.Position + CursorDirection * CursorSpeed * (gameTime.ElapsedGameTime.Milliseconds / 1000f);

            if (endPosition.X <= HalfWidth)
                endPosition = new Vector2(HalfWidth, endPosition.Y);
            else if (endPosition.X >= GameScene.Width - HalfWidth)
                endPosition = new Vector2(GameScene.Width - HalfWidth, endPosition.Y);

            if (endPosition.Y <= halfHeight)
                endPosition = new Vector2(endPosition.X, HalfHeight);
            else if (endPosition.Y >= GameScene.Height - HalfHeight)
                endPosition = new Vector2(endPosition.X, GameScene.Height - HalfWidth);

            Position = endPosition;
            CursorDirection = Vector2.Zero;

            base.Update(gameTime);
        }

        public void Move(Vector2 direction)
        {
            CursorDirection = direction;
        }

    }
}
