using System;
using System.Numerics;
using OpenTK;
using OpenTK.Windowing.Desktop;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ZigZag.Editor
{
    class Program
    {
        public static Ui.Style ActiveStyle
        {
            get;
            set;
        } = new Ui.FlatStyle();


        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(800, 600),
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

            OpenGLTest t = new OpenGLTest();
            t.Setup();

            ImGuiPlatformIntegration.SetupKeys();
            ImGuiRendererIntegration.Initialize();
            ImGuiRendererIntegration.CreateFontsTexture();

            Ui.MainMenu menu = new Ui.MainMenu();
            Ui.HistoryWindow history = new Ui.HistoryWindow("History 1");
            Ui.HierarchyWindow hierarchy = new Ui.HierarchyWindow("Hierarchy 1");
            Ui.HistoryWindow history2 = new Ui.HistoryWindow("History 2");
            Ui.HierarchyWindow hierarchy2 = new Ui.HierarchyWindow("Hierarchy 2");

            while (!n1.IsExiting)
            {
                n1.ProcessEvents();

                GL.Viewport(0, 0, n1.Size.X, n1.Size.Y);
                GL.ClearColor(0.3f, 0, 0.1f, 1);
                GL.Clear(ClearBufferMask.ColorBufferBit);

                ImGuiPlatformIntegration.UpdateIO(n1);

                var activeStyle = ActiveStyle;

                activeStyle.BeginOverall();
                ImGui.NewFrame();

                // Store active style in a variable in case it is changed midframe

                menu.Draw(activeStyle);


                // Very hacky: 
                // 1 << 14 is the bit flag ImGuiDockNodeFlags_NoWindowMenuButton
                // 1 << 15 is the bit flag ImGuiDockNodeFlags_NoCloseButton
                // Because both of these come from imgui_internal.h they are not exposed
                // thorugh ImGui.NET, but we can still use them as hardcoded values.
                const int dockNodeFlags = (1 << 14) | (1 << 15);
                //const int dockNodeFlags = 0;

                // 1 << 19 = NoDockingOverMe


                var mainWindowPos = ImGui.GetCursorPos() - ImGui.GetStyle().WindowPadding;
                var mainWindowSize = ImGui.GetIO().DisplaySize - mainWindowPos;

                ImGui.SetNextWindowPos(mainWindowPos);
                ImGui.SetNextWindowSize(mainWindowSize);

                if (ImGui.Begin("mainWindow", ImGuiWindowFlags.NoDecoration 
                    | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoBringToFrontOnFocus))
                {
                    ImGui.SetCursorPos(new Vector2(5, 5));
                    //ImGui.DockSpace(1110, new Vector2(1000, 1000), (ImGuiDockNodeFlags)dockNodeFlags);
                    ImGui.DockSpace(123, mainWindowSize - new Vector2(10, 10), (ImGuiDockNodeFlags)dockNodeFlags);
                    //ImGui.DockSpaceOverViewport(new ImGuiViewportPtr(), (ImGuiDockNodeFlags)dockNodeFlags);

                }

                ImGui.End();



                if (!hierarchy.WantsToClose)
                {
                    hierarchy.Draw(activeStyle);
                }
                if (!history.WantsToClose)
                {
                    history.Draw(activeStyle);
                }
                if (!hierarchy2.WantsToClose)
                {
                    hierarchy2.Draw(activeStyle);
                }
                if (!history2.WantsToClose)
                {
                    history2.Draw(activeStyle);
                }

                //Console.WriteLine($"Hierarchy: {hierarchy.IsDocked}, History: {history.IsDocked}");

                ImGui.ShowDemoWindow();


                ImGui.EndFrame();
                activeStyle.EndOverall();

                ImGui.Render();
                ImGuiRendererIntegration.Render(ImGui.GetDrawData(), n1.Size.X, n1.Size.Y);
                

                //t.Draw(n1.Size.X, n1.Size.Y);

                n1.Context.SwapBuffers();
            }

        }
    }
}
