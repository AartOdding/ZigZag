﻿

namespace ZigZag.Math
{
    public readonly struct Vector2
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

        public Vector2(float x, float y)
        {
            m_value = new System.Numerics.Vector2(x, y);
        }

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.m_value + rhs.m_value);
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.m_value - rhs.m_value);
        }

        public static Vector2 operator *(Vector2 lhs, float rhs)
        {
            return new Vector2(lhs.m_value * rhs);
        }

        public static Vector2 operator *(float lhs, Vector2 rhs)
        {
            return new Vector2(lhs * rhs.m_value);
        }

        public static Vector2 operator /(Vector2 lhs, float rhs)
        {
            return new Vector2(lhs.m_value / rhs);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        internal readonly System.Numerics.Vector2 m_value;

        internal Vector2(System.Numerics.Vector2 value)
        {
            m_value = value;
        }
    }
}
