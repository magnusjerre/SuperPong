﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.MJFrameWork
{
    public class MJSprite : MJNode, MJUpdate, MJDraw
    {

        protected Texture2D Texture { get; set; }
        private int numberOfSubImages;
        public int NumberOfSubImages
        {
            get { return numberOfSubImages; }
            set
            {
                numberOfSubImages = value;
                float width = Texture.Bounds.Width / value;
                float height = Texture.Bounds.Height;
                SetSize(width, height);
            }
        }

        private Vector2 size;
        public Vector2 Size { get { return size; } }
        private void SetSize(float width, float height)
        {
            this.size = new Vector2(width, height);
        }

        private Rectangle sourceRectangle;
        public Rectangle SourceRectangle { get { return sourceRectangle; } set { sourceRectangle = value; } }
        public void UpdateSourceRectangle()
        {
            if (NumberOfSubImages != 1)
            {
                int x = (int)(CurrentImageNumber * Size.X);
                sourceRectangle = new Rectangle(x, 0, (int)Size.X, (int)Size.Y);
            }
        }
        
        public float Scale { get; set; }
        public Color ColorTint { get; set; }
        public Vector2 origin;
        public Vector2 Origin { 
            get 
            {
                return new Vector2(origin.X * Size.X * Scale, origin.Y * Size.Y * Scale);
            } 
            set
            {
                this.origin = value;
            }
        }

        public Vector2 GetOrigin()
        {
            return this.origin;
        }

        public float Alpha { get; set; }
        public SpriteEffects SEffects { get; set; }

        private int currentImageNumber;
        protected int CurrentImageNumber { get { return currentImageNumber; } }
        protected void UpdateImageNumber()
        {
            currentImageNumber = (currentImageNumber + 1) % NumberOfSubImages;
        }

        public int AnimationTime { get; set; }
        public int ElapsedAnimationTime { get; set; }

        public MJSprite(Texture2D texture) : base()
        {
            this.Texture = texture;
            NumberOfSubImages = 1;
            SourceRectangle = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);
            Scale = 1;
            ColorTint = Color.White;
            Origin = new Vector2(0.5f, 0.5f);
            SEffects = SpriteEffects.None;
            AnimationTime = 100;
            ElapsedAnimationTime = 0;
            Alpha = 1.0f;
        }

        public MJSprite(Texture2D texture, int numberOfFrames) : this(texture)
        {
            NumberOfSubImages = numberOfFrames;
            UpdateSourceRectangle();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, absoluteCoordinateSystem.Position, SourceRectangle, ColorTint * Alpha, absoluteCoordinateSystem.Rotation, Origin, Scale, SEffects, LayerDepth);

            foreach (MJNode child in Children)
            {
                child.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (NumberOfSubImages > 1)
            {
                ElapsedAnimationTime += gameTime.ElapsedGameTime.Milliseconds;
                if (ElapsedAnimationTime > AnimationTime)
                {
                    ElapsedAnimationTime = 0;
                    UpdateImageNumber();
                    UpdateSourceRectangle();
                }
            }

            foreach (MJNode child in Children)
            {
                child.Update(gameTime);
            }
        }

    }
}
