using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SuperPong.MJFrameWork
{
    public class MJAbsoluteCoordinateSystem
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public MJAbsoluteCoordinateSystem()
        {
            Position = new Vector2(0, 0);
            Rotation = 0;
        }
    }
}
