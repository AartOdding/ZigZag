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
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            
            // Used to convert a byte[] to IntPtr.
            unsafe
            {
                fixed (byte* ptr = Resources.Fonts.WorkSans_Regular)
                {
                    io.Fonts.AddFontFromMemoryTTF((IntPtr)ptr, Resources.Fonts.WorkSans_Regular.Length, 32);
                }
            }

            //io.Fonts.AddFontDefault();

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

                // Very hacky: 
                // 1 << 14 is the bit flag ImGuiDockNodeFlags_NoWindowMenuButton
                // 1 << 15 is the bit flag ImGuiDockNodeFlags_NoCloseButton
                // Because both of these come from imgui_internal.h they are not exposed
                // thorugh ImGui.NET, but we can still use them as hardcoded values.
                const int dockNodeFlags = (1 << 14) | (1 << 15);

                ImGui.DockSpaceOverViewport(new ImGuiViewportPtr(), (ImGuiDockNodeFlags)dockNodeFlags);

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
