

namespace ZigZag.SceneGraph.Math
{
    public readonly struct Vector2
    {
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        public static Vector2 operator *(Vector2 lhs, float rhs)
        {
            return new Vector2(lhs.X * rhs, lhs.Y * rhs);
        }

        public static Vector2 operator *(float lhs, Vector2 rhs)
        {
            return new Vector2(lhs * rhs.X, lhs * rhs.Y);
        }

        public static Vector2 operator /(Vector2 lhs, float rhs)
        {
            return new Vector2(lhs.X / rhs, lhs.Y / rhs);
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
