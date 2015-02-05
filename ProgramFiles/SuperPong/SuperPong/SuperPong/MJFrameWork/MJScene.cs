using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.MJFrameWork
{
    public class MJScene : MJNode
    {
        public ContentManager content;
        public Boolean hasLoadedContent = false;

        public MJScene(ContentManager content, string name) : base()
        {
            this.content = content;
            Name = name;
        }

        public virtual void Initialize()
        {

        }

        public virtual void UnloadContent()
        {
            
        }

        public virtual void LoadContent()
        {

        }

    }
}
