using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigZag.SceneGraph.Math
{
    public readonly struct Rectangle
    {
        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            if (Width < 0 || Height < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public readonly float X;
        public readonly float Y;
        public readonly float Width;
        public readonly float Height;

        public bool Contains(Vector2 point)
        {
            return point.X >= X && point.X <= X + Width
                && point.Y >= Y && point.Y <= Y + Height;
        }

        public Vector2 TopLeft()
        {
            return new Vector2(X, Y);
        }

        public Vector2 TopRight()
        {
            return new Vector2(X + Width, Y);
        }

        public Vector2 BottomLeft()
        {
            return new Vector2(X, Y + Height);
        }

        public Vector2 BottomRight()
        {
            return new Vector2(X + Width, Y + Height);
        }

        public Vector2 Centre()
        {
            return new Vector2(X + 0.5f * Width, Y + 0.5f * Height);
        }
    }
}
