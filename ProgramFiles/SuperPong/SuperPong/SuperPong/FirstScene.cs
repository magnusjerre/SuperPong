using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SuperPong.MJFrameWork;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperPong
{
    public class FirstScene : MJScene
    {

        Texture2D ballTexture;

        public FirstScene(ContentManager content)
            : base(content, "FirstScene")
        {
            
        }

        public override void LoadContent()
        {
            hasLoadedContent = true;
            ballTexture = content.Load<Texture2D>("ball");
            if (!HasChildNamed("ball"))
            {
                MJSprite ball = new MJSprite(ballTexture);
                ball.Name = "ball";
                ball.Position = new Vector2(100, 100);
                AddChild(ball);
                Console.WriteLine("NumberOfChildren: " + Children.Count);
            }
            
        }

    }
}
