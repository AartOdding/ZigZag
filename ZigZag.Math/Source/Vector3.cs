

namespace ZigZag.Math
{
    public readonly struct Vector3
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector2 vec)
        {
            X = vec.X;
            Y = vec.Y;
            Z = 0;
        }

        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }

        public static Vector3 operator *(Vector3 lhs, float rhs)
        {
            return new Vector3(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
        }

        public static Vector3 operator *(float lhs, Vector3 rhs)
        {
            return new Vector3(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z);
        }

        public static Vector3 operator /(Vector3 lhs, float rhs)
        {
            return new Vector3(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
        }

        public Vector3 Add(float dx, float dy, float dz)
        {
            return new Vector3(X + dx, Y + dy, Z + dz);
        }

        public Vector3 AddX(float dx)
        {
            return new Vector3(X + dx, Y, Z);
        }

        public Vector3 AddY(float dy)
        {
            return new Vector3(X, Y + dy, Z);
        }

        public Vector3 AddZ(float dz)
        {
            return new Vector3(X, Y, Z + dz);
        }
    }
}
