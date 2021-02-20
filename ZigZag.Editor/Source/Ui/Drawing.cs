using System.Numerics;
using ImGuiNET;


namespace ZigZag.Editor.Ui
{
    static class Drawing
    {
        public static void DrawVariableThicknessRectangle(ImDrawListPtr drawList, Vector2 minCorner, Vector2 maxCorner,
            float thicknessX, float thicknessY, Vector4 color)
        {
            DrawVariableThicknessRectangle(drawList, minCorner, maxCorner, thicknessY, thicknessY,
                thicknessX, thicknessX, Color.U32(color));
        }

        public static void DrawVariableThicknessRectangle(ImDrawListPtr drawList, Vector2 minCorner, Vector2 maxCorner,
            float thicknessX, float thicknessY, uint color)
        {
            DrawVariableThicknessRectangle(drawList, minCorner, maxCorner, thicknessY, thicknessY,
                thicknessX, thicknessX, color);
        }

        public static void DrawVariableThicknessRectangle(ImDrawListPtr drawList, Vector2 minCorner, Vector2 maxCorner,
            float thicknessTop, float thicknessBottom, float thicknessLeft, float thicknessRight, Vector4 color)
        {
            DrawVariableThicknessRectangle(drawList, minCorner, maxCorner, thicknessTop, thicknessBottom, 
                thicknessLeft, thicknessRight, Color.U32(color));
        }

        public static void DrawVariableThicknessRectangle(ImDrawListPtr drawList, Vector2 minCorner, Vector2 maxCorner, 
            float thicknessTop, float thicknessBottom, float thicknessLeft, float thicknessRight, uint color)
        {
            var X1 = minCorner.X + thicknessLeft;
            var X2 = maxCorner.X - thicknessRight;
            var Y1 = minCorner.Y + thicknessTop;
            var Y2 = maxCorner.Y - thicknessBottom;

            if (thicknessLeft > 0) drawList.AddRectFilled(minCorner, new Vector2(X1, maxCorner.Y), color); // Left column
            if (thicknessRight > 0) drawList.AddRectFilled(new Vector2(X2, minCorner.Y), maxCorner, color); // Right column
            if (thicknessTop > 0) drawList.AddRectFilled(new Vector2(X1, minCorner.Y), new Vector2(X2, Y1), color); // Remainder top
            if (thicknessBottom > 0) drawList.AddRectFilled(new Vector2(X1, Y2), new Vector2(X2, maxCorner.Y), color); // Remainder bottom
        }
    }
}
