using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.MJFrameWork
{
    class MJNode : MJUpdate, MJDraw
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }

        public MJNode Parent { get; set; }
        private List<MJNode> children;
        protected List<MJNode> Children { get { return children; } set { children = value; } }

        public MJPhysicsBody PhysicsBody { get; set; }
        public MJParticleEmitter ParticleEmitter { get; set; }

        public MJNode()
        {
            Name = null;
            Position = new Vector2(0, 0);
            Rotation = 0;

            Parent = null;
            Children = new List<MJNode>();

            PhysicsBody = null;
            ParticleEmitter = null;
        }

        public void Update(GameTime gameTime)
        {
            if (PhysicsBody != null)
                PhysicsBody.Update(gameTime);

            if (ParticleEmitter != null)
                ParticleEmitter.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MJNode child in children) {
                child.Draw(spriteBatch);
            }

            if (ParticleEmitter != null)
                ParticleEmitter.Draw(spriteBatch);
        }

        public void AdChild(MJNode child)
        {
            if (child.Parent == null) {
                children.Add(child);
                child.Parent = this;
            } else {
                throw new Exception("Child already has a parent: Parent.Name: " + child.Parent.Name);
            }
        }

        public void RemoveChild(MJNode child)
        {
            if (child != null) {
                children.Remove(child);
                child.Parent = null;
            }
        }

        public void RemoveFromParent()
        {
            if (Parent != null)
            {
                Parent.RemoveChild(this);
            }
        }

        public void AttachPhysicsBody(MJPhysicsBody physicsBody)
        {
            this.PhysicsBody = physicsBody;
            this.PhysicsBody.Parent = this;
        }

        public void DetachPhysicsBody()
        {
            this.PhysicsBody.DetachFromParent();
            this.PhysicsBody = null;
        }

        public void AttachParticleEmitter(MJParticleEmitter particleEmiiter)
        {
            this.ParticleEmitter = particleEmiiter;
            this.ParticleEmitter.Parent = this;
        }

        public void DetachParticleEmitter()
        {
            this.ParticleEmitter.DetachFromParent();
            this.ParticleEmitter = null;
        }
    }
}
