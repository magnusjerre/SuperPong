using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork
{
    public class MJPhysicsBody : MJDetachable, MJUpdate
    {
        public MJNode Parent { get; set; }

        public void DetachFromParent()
        {
            throw new NotImplementedException();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
