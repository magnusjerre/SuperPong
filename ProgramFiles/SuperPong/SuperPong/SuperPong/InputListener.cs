using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong
{
    public interface InputListener
    {

        void MovePlayer(int playerNumber, Vector2 direction);
        void StopPlayerMovement(int playerNumber);
        void UsePowerup(int playerNumber);
        void MovePlayerStick(int playerNumber, Vector2 direction);
        void RestartGame();
        void BackButtonPressed();

    }
}
