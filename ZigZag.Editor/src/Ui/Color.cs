using System.Numerics;
using ImGuiNET;


namespace ZigZag.Editor.Ui
{
    static class Color
    {
        public static uint U32(Vector3 color)
        {
            return ImGui.GetColorU32(new Vector4(color.X, color.Y, color.Z, 1));
        }

        public static uint U32(Vector4 color)
        {
            return ImGui.GetColorU32(color);
        }

        public static uint U32(float r, float g, float b)
        {
            return ImGui.GetColorU32(new Vector4(r, g, b, 1));
        }

        public static uint U32(float r, float g, float b, float a)
        {
            return ImGui.GetColorU32(new Vector4(r, g, b, a));
        }

        public static uint U32(uint r, uint g, uint b)
        {
            return U32(r, g, b, 255);
        }

        public static uint U32(uint r, uint g, uint b, uint a)
        {
            const uint mask = 255;
            uint red = (r & mask) << 0;
            uint green = (g & mask) << 8;
            uint blue = (b & mask) << 16;
            uint alpha = (a & mask) << 24;
            return red | green | blue | alpha;
        }

        public static Vector4 Vec4(uint color)
        {
            uint r = (color << 0) >> 24;
            uint g = (color << 8) >> 24;
            uint b = (color << 16) >> 24;
            uint a = (color << 24) >> 24;
            return new Vector4(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
        }
    }
}
