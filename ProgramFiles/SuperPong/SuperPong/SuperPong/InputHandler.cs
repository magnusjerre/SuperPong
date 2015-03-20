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

        public InputHandler(InputListener listener, int playerNumber)
        {
            handler = new GamePadHandler(listener, playerNumber);
        }

        public InputHandler(InputListener listener)
        {
            handler = new KeyboardHandler(listener);
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
        protected int playerNumber;
        protected PlayerIndex index;

        public GamePadHandler(InputListener listener, int playerNumber)
            : base(listener)
        {
            this.playerNumber = playerNumber;
            
            if (playerNumber == 1)
                this.index = PlayerIndex.One;
            else
                this.index = PlayerIndex.Two;
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState state = GamePad.GetState(index);
            Vector2 leftStick = state.ThumbSticks.Left;
            Vector2 rightStick = state.ThumbSticks.Right;
           
            if (leftStick.Y != 0)
                listener.MovePlayer(playerNumber, new Vector2(leftStick.X, -leftStick.Y));
            else
                listener.StopPlayerMovement(playerNumber);

            if (rightStick.X != 0 || rightStick.Y != 0)
                listener.MovePlayerStick(playerNumber, new Vector2(rightStick.X, -rightStick.Y));

            if (state.IsButtonDown(Buttons.A))
                listener.UsePowerup(playerNumber);

            if (state.IsButtonDown(Buttons.Y))
                listener.RestartGame();
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
            Vector2 rightStickP1 = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
                rightStickP1 = new Vector2(rightStickP1.X, rightStickP1.Y - 1);
            if (Keyboard.GetState().IsKeyDown(Keys.H))
                rightStickP1 = new Vector2(rightStickP1.X, rightStickP1.Y + 1);
            if (Keyboard.GetState().IsKeyDown(Keys.G))
                rightStickP1 = new Vector2(rightStickP1.X - 1, rightStickP1.Y);
            if (Keyboard.GetState().IsKeyDown(Keys.J))
                rightStickP1 = new Vector2(rightStickP1.X + 1, rightStickP1.Y);

            if (rightStickP1.X != 0 || rightStickP1.Y != 0)
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
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                rightStickP2 = new Vector2(rightStickP2.X, rightStickP2.Y - 1);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                rightStickP2 = new Vector2(rightStickP2.X, rightStickP2.Y + 1);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                rightStickP2 = new Vector2(rightStickP2.X - 1, rightStickP2.Y);
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                rightStickP2 = new Vector2(rightStickP2.X + 1, rightStickP2.Y);
            if (rightStickP2.X != 0 || rightStickP2.Y != 0)
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
