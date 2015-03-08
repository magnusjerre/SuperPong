using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong.MJFrameWork
{
    public class MJNode : MJUpdate, MJDraw
    {
        public string Name { get; set; }

        private Vector2 position;
        public Vector2 Position {
            get
            { 
                return position; 
            }
            set
            {
                position = value;
                UpdateAbsValues();
            }
        }

        private float rotation;
        public float Rotation {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                UpdateAbsValues();
            }
        }

        public MJAbsoluteCoordinateSystem absoluteCoordinateSystem { get; set; }

        public float LayerDepth { get; set; }

        public MJNode Parent { get; set; }
        private List<MJNode> children;
        protected List<MJNode> Children { get { return children; } set { children = value; } }

        public MJPhysicsBody PhysicsBody { get; set; }

        public MJNode()
        {
            Name = null;
            Parent = null;
            Children = new List<MJNode>();

            absoluteCoordinateSystem = new MJAbsoluteCoordinateSystem();
            Position = new Vector2(0, 0);
            Rotation = 0;
            LayerDepth = 1;

            PhysicsBody = null;            
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (MJNode child in Children)
            {
                child.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (MJNode child in children) {
                child.Draw(spriteBatch);
            }
        }

        public void AddChild(MJNode child)
        {
            if (child.Parent == null && !Children.Contains(child)) {
                children.Add(child);
                child.Parent = this;
                child.UpdateAbsValues();
            } else {
                throw new Exception("Child already has a parent: Parent.Name: " + child.Parent.Name);
            }
        }

        public void RemoveChild(MJNode child)
        {
            if (child != null) {
                children.Remove(child);
                child.Parent = null;
                child.UpdateAbsValues();
            }
        }

        public Boolean HasChildNamed(string name)
        {
            foreach (MJNode child in Children)
            {
                if (child.Name.Equals(name))
                    return true;
            }
            return false;
        }

        public void RemoveFromParent()
        {
            if (Parent != null)
            {
                Parent.RemoveChild(this);
                UpdateAbsValues();
            }
        }

        public void AttachPhysicsBody(MJPhysicsBody physicsBody)
        {
            this.PhysicsBody = physicsBody;
            this.PhysicsBody.Parent = this;
            MJPhysicsManager.getInstance().AddBody(physicsBody);
        }

        public void DetachPhysicsBodySafely()
        {
            if (PhysicsBody != null)
            {
                MJPhysicsManager.getInstance().RemoveBodySafely(PhysicsBody);
            }
        }

        protected void UpdateAbsValuesForThis()
        {
            Vector2 globalPosition = new Vector2(Position.X, Position.Y);
            float absRotation = Rotation;

            if (Parent != null)
            {
                float xAxisAngle = MJUtils.GetAngleFromXAxis(Position);
                float lengthOfPosFromParent = Position.Length();
                float newAngle = xAxisAngle + Parent.absoluteCoordinateSystem.Rotation;
                float newX = (float)(Math.Cos(newAngle) * lengthOfPosFromParent) + Parent.absoluteCoordinateSystem.Position.X;
                float newY = (float)(Math.Sin(newAngle) * lengthOfPosFromParent) + Parent.absoluteCoordinateSystem.Position.Y;

                globalPosition.X = newX;
                globalPosition.Y = newY;
                absRotation = Rotation + Parent.absoluteCoordinateSystem.Rotation;
            }

            absoluteCoordinateSystem.Position = globalPosition;
            absoluteCoordinateSystem.Rotation = absRotation;
        }

        protected void UpdateChildrenAbsValues()
        {
            foreach (MJNode child in Children)
            {
                child.UpdateAbsValuesForThis();
                child.UpdateChildrenAbsValues();
            }
        }

        protected void UpdateAbsValues()
        {
            UpdateAbsValuesForThis();
            UpdateChildrenAbsValues();
        }
    }
}
