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
        public ContentManager content;
        private MJPhysicsManager physicsManager;

        public MJScene(ContentManager content, string name) : base()
        {
            this.content = content;
            Name = name;
        }

        public Texture2D LoadTexture2D(string textureName)
        {
            return content.Load<Texture2D>(textureName);
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
