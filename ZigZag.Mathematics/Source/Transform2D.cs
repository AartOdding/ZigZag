using System;


namespace ZigZag.Mathematics
{
    public readonly struct Transform2D : IEquatable<Transform2D>
    {
        public static Transform2D Identity = new Transform2D(System.Numerics.Matrix3x2.Identity);

        public bool IsIdentity
        {
            get
            {
                return m_value.IsIdentity;
            }
        }
        public Vector2 Translation
        {
            get
            {
                return new Vector2(m_value.Translation);
            }
        }

        public static Transform2D CreateTranslation(float x, float y)
        {
            return new Transform2D(System.Numerics.Matrix3x2.CreateTranslation(x, y));
        }
        public static Transform2D CreateTranslation(Vector2 position)
        {
            return new Transform2D(System.Numerics.Matrix3x2.CreateTranslation(position.m_value));
        }

        public static Transform2D CreateRotation(float radians)
        {
            return new Transform2D(System.Numerics.Matrix3x2.CreateRotation(radians));
        }
        public static Transform2D CreateRotation(float radians, Vector2 centerPoint)
        {
            return new Transform2D(System.Numerics.Matrix3x2.CreateRotation(radians, centerPoint.m_value));
        }

        public static Transform2D CreateScale(float scale)
        {
            return new Transform2D(System.Numerics.Matrix3x2.CreateScale(scale));
        }
        public static Transform2D CreateScale(float scale, Vector2 centerPoint)
        {
            return new Transform2D(System.Numerics.Matrix3x2.CreateScale(scale, centerPoint.m_value));
        }
        public static Transform2D CreateScale(float scaleX, float scaleY)
        {
            return new Transform2D(System.Numerics.Matrix3x2.CreateScale(scaleX, scaleY));
        }
        public static Transform2D CreateScale(float scaleX, float scaleY, Vector2 centerPoint)
        {
            return new Transform2D(System.Numerics.Matrix3x2.CreateScale(scaleX, scaleY, centerPoint.m_value));
        }
        public static Transform2D CreateScale(Vector2 scale)
        {
            return new Transform2D(System.Numerics.Matrix3x2.CreateScale(scale.m_value));
        }
        public static Transform2D CreateScale(Vector2 scale, Vector2 centerPoint)
        {
            return new Transform2D(System.Numerics.Matrix3x2.CreateScale(scale.m_value, centerPoint.m_value));
        }

        public static Transform2D CreateSkew(float radiansX, float radiansY)
        {
            return new Transform2D(System.Numerics.Matrix3x2.CreateSkew(radiansX, radiansY));
        }
        public static Transform2D CreateSkew(float radiansX, float radiansY, Vector2 centerPoint)
        {
            return new Transform2D(System.Numerics.Matrix3x2.CreateSkew(radiansX, radiansY, centerPoint.m_value));
        }

        // translate
        // rotate
        // scale
        // skew

        public static Vector2 operator *(Transform2D transform, Vector2 vector)
        {
            return new Vector2(System.Numerics.Vector2.Transform(vector.m_value, transform.m_value));
        }

        public bool Equals(Transform2D other)
        {
            return m_value == other.m_value;
        }

        internal Transform2D(System.Numerics.Matrix3x2 value)
        {
            m_value = value;
        }

        internal readonly System.Numerics.Matrix3x2 m_value;
    }
}
