using System;


namespace ZigZag.Mathematics
{
    public readonly struct Rectangle : IEquatable<Rectangle>
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Width;
        public readonly float Height;

        public Rectangle(float x, float y, float width, float height)
        {
            if (width < 0 || height < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

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

        public override bool Equals(object other)
        {
            if ((other == null) || !other.GetType().Equals(typeof(Rectangle)))
            {
                return false;
            }
            else
            {
                return Equals((Rectangle)other);
            }
        }

        public bool Equals(Rectangle other)
        {
            return X == other.X
                && Y == other.Y
                && Width == other.Width
                && Height == other.Height;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(X, Y, Width, Height).GetHashCode();
        }

        public static bool operator ==(Rectangle lhs, Rectangle rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Rectangle lhs, Rectangle rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
