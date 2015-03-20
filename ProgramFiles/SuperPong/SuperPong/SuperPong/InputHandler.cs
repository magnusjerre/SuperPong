using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SuperPong.MJFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong
{
    public class InputHandler
    {
        
        InputMethod handler;        

        public InputHandler(InputListener listener, InputType inputType)
        {
            if (inputType == InputType.KEYBOARD)
            {
                handler = new KeyboardHandler(listener);
            }
            else
            {
                handler = new GamePadHandler(listener);
            }
        }

        public void Update(GameTime gameTime)
        {
            handler.Update(gameTime);
        }

    }

    public enum InputType
    {
        KEYBOARD, GAMEPAD
    }

    public abstract class InputMethod : MJUpdate
    {
        protected InputListener listener;
        protected readonly Vector2 down = new Vector2(0, 1);
        protected readonly Vector2 up = new Vector2(0, -1);
        protected const int player1 = 1, player2 = 2;

        public InputMethod(InputListener listener)
        {
            this.listener = listener;
        }

        public abstract void Update(GameTime gameTime);
    }

    public class GamePadHandler : InputMethod
    {
        
        public GamePadHandler(InputListener listener)
            : base(listener)
        {
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }

    public class KeyboardHandler : InputMethod
    {
        public KeyboardHandler(InputListener listener)
            : base(listener)
        {
        }

        public override void Update(GameTime gameTime)
        {
            //Player 1 - Left paddle
            //Paddle movement
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                listener.MovePlayer(player1, down);
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
                listener.MovePlayer(player1, up);
            else
                listener.StopPlayerMovement(player1);

            //Stick movement
            Vector2 rightStickP1 = new Vector2();
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
                rightStickP1 = new Vector2(rightStickP1.X, rightStickP1.Y - 1);
            if (Keyboard.GetState().IsKeyDown(Keys.H))
                rightStickP1 = new Vector2(rightStickP1.X, rightStickP1.Y + 1);
            if (Keyboard.GetState().IsKeyDown(Keys.G))
                rightStickP1 = new Vector2(rightStickP1.X - 1, rightStickP1.Y);
            if (Keyboard.GetState().IsKeyDown(Keys.J))
                rightStickP1 = new Vector2(rightStickP1.X + 1, rightStickP1.Y);
            listener.MovePlayerStick(player1, rightStickP1);

            //Powerup
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                listener.UsePowerup(player1);

            //Player 2 - Right paddle
            //Paddle movement
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                listener.MovePlayer(player2, down);
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                listener.MovePlayer(player2, up);
            else
                listener.StopPlayerMovement(player2);

            //Stick movement
            Vector2 rightStickP2 = new Vector2();
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
                rightStickP2 = new Vector2(rightStickP2.X, rightStickP2.Y - 1);
            if (Keyboard.GetState().IsKeyDown(Keys.H))
                rightStickP2 = new Vector2(rightStickP2.X, rightStickP2.Y + 1);
            if (Keyboard.GetState().IsKeyDown(Keys.G))
                rightStickP2 = new Vector2(rightStickP2.X - 1, rightStickP2.Y);
            if (Keyboard.GetState().IsKeyDown(Keys.J))
                rightStickP2 = new Vector2(rightStickP2.X + 1, rightStickP2.Y);
            listener.MovePlayerStick(player2, rightStickP2);

            //Powerup
            if (Keyboard.GetState().IsKeyDown(Keys.M))
                listener.UsePowerup(player2);

            //Not player related
            if (Keyboard.GetState().IsKeyDown(Keys.R))
                listener.RestartGame();
        }
    }
}
