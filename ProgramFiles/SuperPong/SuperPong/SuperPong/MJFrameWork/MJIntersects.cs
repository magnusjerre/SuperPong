using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPong.MJFrameWork
{
    public class MJIntersects
    {
        public Boolean Intersects { get; set; }
        public Vector2 Normal { get; set; }

        public MJIntersects(Boolean intersects, Vector2 normal)
        {
            Intersects = intersects;
            Normal = normal;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() == typeof(MJIntersects))
            {
                MJIntersects other = (MJIntersects)obj;
                if (other.Intersects == Intersects && other.Normal == Normal)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
