using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.Powerups
{
    public interface PowerupObserver
    {
        void NotifyPowerupEnded(Powerup powerup);
    }
}
