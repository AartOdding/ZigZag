using OpenTK.Windowing.GraphicsLibraryFramework;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace ZigZag.Editor
{
    public static class ImGuiPlatformIntegration
    {

        public static void UpdateIO(NativeWindow window)
        {
            var io = ImGui.GetIO();

            io.DeltaTime = 1.0f / 60.0f;
            io.DisplaySize.X = window.Size.X;
            io.DisplaySize.Y = window.Size.Y;

            io.MousePos.X = window.MousePosition.X;
            io.MousePos.Y = window.MousePosition.Y;
            io.MouseDown[0] = window.IsMouseButtonDown(MouseButton.Left);
            io.MouseDown[1] = window.IsMouseButtonDown(MouseButton.Right);
        }


        public static void SetupKeys()
        {
            var io = ImGui.GetIO();
            io.KeyMap[(int)ImGuiKey.Tab] = (int)Keys.Tab;
            io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Keys.Left;
            io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Keys.Right;
            io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Keys.Up;
            io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Keys.Down;
            io.KeyMap[(int)ImGuiKey.PageUp] = (int)Keys.PageUp;
            io.KeyMap[(int)ImGuiKey.PageDown] = (int)Keys.PageDown;
            io.KeyMap[(int)ImGuiKey.Home] = (int)Keys.Home;
            io.KeyMap[(int)ImGuiKey.End] = (int)Keys.End;
            io.KeyMap[(int)ImGuiKey.Delete] = (int)Keys.Delete;
            io.KeyMap[(int)ImGuiKey.Backspace] = (int)Keys.Backspace;
            io.KeyMap[(int)ImGuiKey.Enter] = (int)Keys.Enter;
            io.KeyMap[(int)ImGuiKey.Escape] = (int)Keys.Escape;
            io.KeyMap[(int)ImGuiKey.Space] = (int)Keys.Space;
            io.KeyMap[(int)ImGuiKey.A] = (int)Keys.A;
            io.KeyMap[(int)ImGuiKey.C] = (int)Keys.C;
            io.KeyMap[(int)ImGuiKey.V] = (int)Keys.V;
            io.KeyMap[(int)ImGuiKey.X] = (int)Keys.X;
            io.KeyMap[(int)ImGuiKey.Y] = (int)Keys.Y;
            io.KeyMap[(int)ImGuiKey.Z] = (int)Keys.Z;
        }

    }
}
