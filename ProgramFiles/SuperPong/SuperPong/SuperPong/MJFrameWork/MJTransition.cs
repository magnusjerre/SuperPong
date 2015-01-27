using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SuperPong.MJFrameWork.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.MJFrameWork
{
    public class MJTransition
    {
        private MJScene newScene;
        public MJScene NewScene { get { return newScene; } }
        
        private MJScene oldScene;
        private MJTransisitionType transitionType;
        private MJTransitionListener transitionListener;

        public MJTransition(MJTransitionListener transitionListener, MJScene oldScene, MJScene newScene, MJTransisitionType transitionType)
        {
            this.newScene = newScene;
            this.oldScene = oldScene;
            this.transitionType = transitionType;
            this.transitionListener = transitionListener;
        }

        public void StartTransition()
        {
            NewScene.LoadContent();
            NewScene.Initialize();
            if (transitionType == MJTransisitionType.None)
            {
                transitionListener.NotifyTransitionEnded(this);
            }
        }

        public void Update(GameTime gameTime)
        {
            newScene.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            newScene.Draw(spriteBatch);
        }

    }

    public enum MJTransisitionType
    {
        None = 0
    }
}
