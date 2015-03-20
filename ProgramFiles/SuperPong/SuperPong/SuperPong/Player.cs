using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SuperPong.MJFrameWork;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperPong
{
    public class Player : MJNode, ResetPoint, ResetGame
    {
        private MJSprite sprite;
        public MJSprite Sprite { get { return sprite; } }

        public Vector2 InitialPosition { get; set; }
        public Vector2 OriginalMovementVelocity { get; set; }   //Absolute values of velocity
        public Vector2 CurrentMovementVelcoity { get; set; }    //Absolute values of velocity  

        public Player(MJSprite sprite, MJPhysicsBody body, Vector2 initialPosition)
        {
            this.sprite = sprite;
            AddChild(this.sprite);
            
            AttachPhysicsBody(body);
            
            InitialPosition = initialPosition;
            OriginalMovementVelocity = new Vector2(0, 500);
            CurrentMovementVelcoity = new Vector2(0, 0);

        }

        public override void Update(GameTime gameTime)
        {
            float endY = absoluteCoordinateSystem.Position.Y + CurrentMovementVelcoity.Y * (gameTime.ElapsedGameTime.Milliseconds / 1000f);
            if (endY <= Sprite.Size.Y / 2)
            {
                CurrentMovementVelcoity = Vector2.Zero;
                Position = new Vector2(absoluteCoordinateSystem.Position.X, Sprite.Size.Y / 2);
            }
            else if (endY >= GameScene.Height - Sprite.Size.Y / 2)
            {
                CurrentMovementVelcoity = Vector2.Zero;
                Position = new Vector2(absoluteCoordinateSystem.Position.X, GameScene.Height - Sprite.Size.Y / 2);
            }

           
            PhysicsBody.Velocity = CurrentMovementVelcoity;
            base.Update(gameTime);
        }

        public void Move(Vector2 direction) //positive y = downwards, negative y = upwards
        {
            CurrentMovementVelcoity = OriginalMovementVelocity * direction;
        }

        public void StopMove()
        {
            CurrentMovementVelcoity = new Vector2(0, 0);
        }

        public void ResetPoint()
        {
            Position = InitialPosition;
        }

        public void ResetGame()
        {
            ResetPoint();
        }
    }
}
