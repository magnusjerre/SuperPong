using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong
{
    public class Font
    {
        public SpriteFont SpriteFont { get; set; }

        private string text;
        public string Text 
        { 
            get { return text; } 
            set 
            { 
                text = value; 
                MeasuredText = SpriteFont.MeasureString(text); 
            } 
        }
        
        public Vector2 MeasuredText;

        /**
         * The input value should be between 0 and 1
         */
        private Vector2 textOffset;
        public Vector2 TextOffset 
        { 
            get 
            { 
                return textOffset * MeasuredText; 
            }
            set
            {
                textOffset = value;
            }
        }
        
        public Color Color { get; set; }

        public Font(SpriteFont spriteFont, Vector2 offset)
        {
            this.SpriteFont = spriteFont;
            TextOffset = offset;
            Color = Color.White;
        }

    }
}
