using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork
{
    public class MJRectangle
    {

        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MaxX { get; set; }
        public float MaxY { get; set; }

        public MJRectangle(float minX, float minY, float maxX, float maxY) {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public override string ToString()
        {
            return "MinX: " + MinX + ", MinY: " + MinY + ", MaxX: " + MaxX +
                ", MaxY: " + MaxY;
        }

    }
}
