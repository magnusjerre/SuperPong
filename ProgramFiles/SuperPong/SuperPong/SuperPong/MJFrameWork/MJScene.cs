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
        private ContentManager content;
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
            this.physicsManager.Listener = this;
        }

        public void DetachPhysicsManager()
        {
            this.physicsManager.Listener = null;
            this.physicsManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            if (physicsManager != null)
                physicsManager.Update(gameTime);
        }

        public void CollisionBegan(MJCollisionPair pair)
        {
        }

        public void CollisionEndded(MJCollisionPair pair)
        {
        }

        public void IntersectionBegan(MJCollisionPair pair)
        {
        }

        public void IntersectionEnded(MJCollisionPair pair)
        {
        }
    }
}
