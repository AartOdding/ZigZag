using ImGuiNET;
using System.Numerics;


namespace ZigZag.Editor.Ui
{
    static class Drawing
    {
        public static uint U32Color(Vector3 color)
        {
            return U32Color(color.X, color.Y, color.Z, 1);
        }

        public static uint U32Color(float r, float g, float b)
        {
            return U32Color(r, g, b, 1);
        }

        public static uint U32Color(Vector4 color)
        {
            return U32Color(color.X, color.Y, color.Z, color.W);
        }

        public static uint U32Color(float r, float g, float b, float a)
        {
            uint r_ = (((uint)(r * 255.0f)) << 24) >> 0;
            uint g_ = (((uint)(b * 255.0f)) << 24) >> 8; // No idea why but g and b need to be switched for the correct color
            uint b_ = (((uint)(g * 255.0f)) << 24) >> 16;
            uint a_ = (((uint)(a * 255.0f)) << 24) >> 24;
            return r_ | g_ | b_ | a_;
        }

        public static void DrawThickLineRectangle(ImDrawListPtr drawList, Vector2 min, Vector2 max, Vector2 thickness, Vector4 color)
        {
            DrawThickLineRectangle(drawList, min, max, thickness, ImGui.GetColorU32(color));
        }

        public static void DrawThickLineRectangle(ImDrawListPtr drawList, Vector2 min, Vector2 max, Vector2 thickness, uint color)
        {
            var X1 = min.X + thickness.X;
            var X2 = max.X - thickness.X;
            var Y1 = min.Y + thickness.Y;
            var Y2 = max.Y - thickness.Y;

            drawList.AddRectFilled(min, new Vector2(X1, max.Y), color); // Left column
            drawList.AddRectFilled(new Vector2(X2, min.Y), max, color); // Right column
            drawList.AddRectFilled(new Vector2(X1, min.Y), new Vector2(X2, Y1), color); // Remainder top
            drawList.AddRectFilled(new Vector2(X1, Y2), new Vector2(X2, max.Y), color); // Remainder bottom
        }
    }
}
