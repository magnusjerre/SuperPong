using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SuperPong.MJFrameWork;

namespace SuperPong
{
    class SecondScene : MJScene
    {

        Texture2D fireBallTexture;
        
        public SecondScene(ContentManager content)
            : base(content, "SecondScene")
        {
            
        }

        public override void LoadContent()
        {
            fireBallTexture = content.Load<Texture2D>("fireballs");
        }

        public override void Initialize()
        {
            MJSprite fireball = new MJSprite(fireBallTexture, 3);
            fireball.Name = "fireball";
            fireball.Position = new Vector2(200, 300);
            AddChild(fireball);
        }
    }
}
