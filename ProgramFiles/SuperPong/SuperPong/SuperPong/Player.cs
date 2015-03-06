using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SuperPong.MJFrameWork;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong
{
    public class Player : MJNode
    {
        private MJSprite sprite;
        public MJSprite Sprite { get { return sprite; } }

        public Vector2 OriginalMovementVelocity { get; set; }   //Absolute values of velocity
        public Vector2 CurrentMovementVelcoity { get; set; }    //Absolute values of velocity  

        public Player(MJSprite sprite)
        {
            this.sprite = sprite;
            AddChild(this.sprite);
            OriginalMovementVelocity = new Vector2(0, 500);
            CurrentMovementVelcoity = new Vector2(0, 0);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            sprite.PhysicsBody.Velocity = CurrentMovementVelcoity;
        }

        public void Move(Vector2 direction) //positive y = downwards, negative y = upwards
        {
            CurrentMovementVelcoity = OriginalMovementVelocity * direction;
        }

        public void StopMove()
        {
            CurrentMovementVelcoity = new Vector2(0, 0);
        }
    }
}
