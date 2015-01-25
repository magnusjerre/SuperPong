using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.MJFrameWork
{
    public class MJParticleEmitter : MJDetachable, MJUpdate, MJDraw
    {

        public MJNode Parent { get; set; }

        public void DetachFromParent()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
