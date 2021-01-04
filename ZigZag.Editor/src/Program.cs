using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ZigZag.Editor
{
    class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Test window",
                AutoLoadBindings = true
            };

            NativeWindow n1 = new NativeWindow(nativeWindowSettings);
            n1.Context.MakeCurrent();
            GLFW.SwapInterval(1);

            n1.Refresh += () => {  };

            IntPtr m_imguiContext = ImGui.CreateContext();
            ImGui.SetCurrentContext(m_imguiContext);
            var io = ImGui.GetIO();
            io.Fonts.AddFontDefault();

            OpenGLTest t = new OpenGLTest();
            t.Setup();

            ImGuiPlatformIntegration.SetupKeys();
            ImGuiRendererIntegration.Initialize();
            ImGuiRendererIntegration.CreateFontsTexture();

            while (!n1.IsExiting)
            {
                n1.ProcessEvents();

                GL.Viewport(0, 0, n1.Size.X, n1.Size.Y);
                GL.ClearColor(0.3f, 0, 0.1f, 1);
                GL.Clear(ClearBufferMask.ColorBufferBit);

                
                ImGuiPlatformIntegration.UpdateIO(n1);
                ImGui.NewFrame();
                ImGui.ShowDemoWindow();
                ImGui.EndFrame();
                ImGui.Render();

                ImGuiRendererIntegration.Render(ImGui.GetDrawData(), n1.Size.X, n1.Size.Y);
                

                t.Draw(n1.Size.X, n1.Size.Y);

                n1.Context.SwapBuffers();
            }

        }
    }
}
