using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SuperPong.MJFrameWork.Interfaces;
using Microsoft.Xna.Framework;

namespace SuperPong.MJFrameWork
{
    public class MJScene : MJNode, MJPhysicsEventListener
    {
        public ContentManager Content;
        private MJPhysicsManager physicsManager;

        public MJScene(string name) : base()
        {
            Name = name;
        }

        public Texture2D LoadTexture2D(string textureName)
        {
            return Content.Load<Texture2D>(textureName);
        }

        public virtual void Initialize()
        {
            LoadContent();
        }

        public void UnloadContent()
        {
            if (Content != null)
                Content.Unload();
        }

        public virtual void LoadContent()
        {

        }

        public void AttachPhysicsManager(MJPhysicsManager manager)
        {
            this.physicsManager = manager;
            this.physicsManager.AddListenerSafely(this);
        }

        public void DetachPhysicsManager()
        {
            this.physicsManager.RemoveListenerSafely(this);
            this.physicsManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (physicsManager != null)
                physicsManager.Update(gameTime);
        }

        public virtual void CollisionBegan(MJIntersection pair)
        {
        }

        public virtual void CollisionEnded(MJIntersection pair)
        {
        }

        public virtual void IntersectionBegan(MJIntersection pair)
        {
        }

        public virtual void IntersectionEnded(MJIntersection pair)
        {
        }
    }
}
