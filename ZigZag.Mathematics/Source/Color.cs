using System;


namespace ZigZag.Mathematics
{
    public readonly struct Color : IEquatable<Color>
    {
        public Color(float r, float g, float b, float a)
        {
            Red = r;
            Green = g;
            Blue = b;
            Alpha = a;
        }

        public readonly float Red;
        public readonly float Green;
        public readonly float Blue;
        public readonly float Alpha;

        public uint U32()
        {
            const uint mask = 255;
            
            uint r = (uint)(Red * 255.0f);
            uint g = (uint)(Green * 255.0f);
            uint b = (uint)(Blue * 255.0f);
            uint a = (uint)(Alpha * 255.0f);
            
            uint red = (r & mask) << 0;
            uint green = (g & mask) << 8;
            uint blue = (b & mask) << 16;
            uint alpha = (a & mask) << 24;

            return red | green | blue | alpha;
        }

        public bool Equals(Color other)
        {
            return Red == other.Red
                && Green == other.Green
                && Blue == other.Blue
                && Alpha == other.Alpha;
        }
    }
}
