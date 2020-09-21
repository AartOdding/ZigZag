using System;

namespace ZigZag
{
    public struct Color : IEquatable<Color>
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public Color(byte r_, byte g_, byte b_)
        {
            r = r_;
            g = g_;
            b = b_;
            a = 255;
        }

        public Color(byte r_, byte g_, byte b_, byte a_)
        {
            r = r_;
            g = g_;
            b = b_;
            a = a_;
        }

        public bool Equals(Color other)
        {
            return r == other.r && g == other.g && b == other.b && a == other.a;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Color))
            {
                return false;
            }

            Color other = (Color)obj;
            return r == other.r && g == other.g && b == other.b && a == other.a;
        }

        public override int GetHashCode()
        {
            int hashCode = (r << 24);
            hashCode |= (g << 16);
            hashCode |= (b << 8);
            hashCode |= a;
            return hashCode;
        }
    }
}
