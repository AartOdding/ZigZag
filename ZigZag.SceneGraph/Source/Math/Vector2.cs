using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigZag.SceneGraph.Math
{
    readonly struct Vector2
    {
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2 Add(float dx, float dy)
        {
            return new Vector2(X + dx, Y + dy);
        }

        public Vector2 AddX(float dx)
        {
            return new Vector2(X + dx, Y);
        }

        public Vector2 AddY(float dx)
        {
            return new Vector2(X + dx, Y);
        }

        public readonly float X;
        public readonly float Y;
    }
}
