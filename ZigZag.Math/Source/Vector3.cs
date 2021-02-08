

namespace ZigZag.Math
{
    public readonly struct Vector3
    {
        public float X
        {
            get
            {
                return m_value.X;
            }
        }
        public float Y
        {
            get
            {
                return m_value.Y;
            }
        }
        public float Z
        {
            get
            {
                return m_value.Z;
            }
        }

        public Vector3(float x, float y, float z)
        {
            m_value = new System.Numerics.Vector3(x, y, z);
        }

        public Vector3(Vector2 v)
        {
            m_value = new System.Numerics.Vector3(v.X, v.Y, 0);
        }

        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.m_value + rhs.m_value);
        }

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.m_value - rhs.m_value);
        }

        public static Vector3 operator *(Vector3 lhs, float rhs)
        {
            return new Vector3(lhs.m_value * rhs);
        }

        public static Vector3 operator *(float lhs, Vector3 rhs)
        {
            return new Vector3(lhs * rhs.m_value);
        }

        public static Vector3 operator /(Vector3 lhs, float rhs)
        {
            return new Vector3(lhs.m_value / rhs);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        internal readonly System.Numerics.Vector3 m_value;

        internal Vector3(System.Numerics.Vector3 value)
        {
            m_value = value;
        }
    }
}
