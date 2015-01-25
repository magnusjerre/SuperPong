using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SuperPong.MJFrameWork
{
    public class MJUtils
    {
        public static float GetAngleFromXAxis(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }
    }
}
