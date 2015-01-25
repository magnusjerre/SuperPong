using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;

namespace SuperPong.MJFrameWork
{
    public class MJScene : MJNode
    {
        public ContentManager content;

        public MJScene(ContentManager content, string name) : base()
        {
            this.content = content;
            Name = name;
        }

        protected void Initialize()
        {

        }

        protected void UnloadContent()
        {

        }

        protected void LoadContent()
        {

        }
    }
}
