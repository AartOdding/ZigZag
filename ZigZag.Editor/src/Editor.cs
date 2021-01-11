using System;
using System.Collections.Generic;
using System.Numerics;
using OpenTK.Windowing.Desktop;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ZigZag.Editor.Ui.Windows;
using ZigZag.Editor.Ui;


namespace ZigZag.Editor
{
    public sealed class Editor : Runtime.IEditor
    {
        internal static Style ActiveStyle
        {
            get;
            set;
        } = new FlatStyle();

        public void OpenEditor()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(800, 600),
                Title = "ZigZag Editor",
                AutoLoadBindings = true
            };

            m_nativeWindow = new NativeWindow(nativeWindowSettings);
            m_nativeWindow.Context.MakeCurrent();
            GLFW.SwapInterval(1);

            m_imguiContext = ImGui.CreateContext();
            ImGui.SetCurrentContext(m_imguiContext);
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            unsafe
            {
                fixed (byte* ptr = Resources.Fonts.WorkSans_Regular)
                {
                    ImGui.GetIO().Fonts.AddFontFromMemoryTTF((IntPtr)ptr, Resources.Fonts.WorkSans_Regular.Length, 28);
                }
            }

            ImGuiPlatformIntegration.SetupKeys();
            ImGuiRendererIntegration.Initialize();
            ImGuiRendererIntegration.CreateFontsTexture();
 
            HistoryWindow history = new HistoryWindow("History 1");
            HierarchyWindow hierarchy = new HierarchyWindow("Hierarchy 1");
            HistoryWindow history2 = new HistoryWindow("History 2");
            HierarchyWindow hierarchy2 = new HierarchyWindow("Hierarchy 2");
        }

        public void CloseEditor()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            m_nativeWindow.ProcessEvents();
            ImGuiPlatformIntegration.UpdateIO(m_nativeWindow);

            GL.Viewport(0, 0, m_nativeWindow.Size.X, m_nativeWindow.Size.Y);
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            var activeStyle = ActiveStyle;
            activeStyle.BeginOverall();

            ImGui.NewFrame();

            m_mainMenu.Draw(activeStyle);

            var mainWindowPos = ImGui.GetCursorPos() - ImGui.GetStyle().WindowPadding;
            var mainWindowSize = ImGui.GetIO().DisplaySize - mainWindowPos;

            ImGui.SetNextWindowPos(mainWindowPos);
            ImGui.SetNextWindowSize(mainWindowSize);

            if (ImGui.Begin("mainWindow", ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoResize
                | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoBringToFrontOnFocus))
            {
                Drawing.DrawVariableThicknessRectangle(ImGui.GetWindowDrawList(), mainWindowPos,
                    mainWindowPos + mainWindowSize, 5, 5, 5, 5, ((FlatStyle)ActiveStyle).ApplicationBackgroundColor);
                ImGui.SetCursorPos(new Vector2(5, 5));
                ImGui.DockSpace(123, mainWindowSize - new Vector2(10, 10), (ImGuiDockNodeFlags)dockNodeFlags);
            }
            ImGui.End();

            foreach (var window in m_openWindows)
            {
                window.Draw(activeStyle);
            }
            m_openWindows.RemoveAll(window => window.WantsToClose);

            ImGui.ShowDemoWindow();
            
            ImGui.EndFrame();
            activeStyle.EndOverall();

            ImGui.Render();
            ImGuiRendererIntegration.Render(ImGui.GetDrawData(), m_nativeWindow.Size.X, m_nativeWindow.Size.Y);

            m_nativeWindow.Context.SwapBuffers();
        }

        public bool WantsToClose()
        {
            return m_nativeWindow.IsExiting;
        }


        private NativeWindow m_nativeWindow;
        private IntPtr m_imguiContext;
        private readonly List<DockableWindow> m_openWindows = new List<DockableWindow>();
        private readonly MainMenu m_mainMenu = new MainMenu();

        // Very hacky: 
        // 1 << 14 is the bit flag ImGuiDockNodeFlags_NoWindowMenuButton
        // 1 << 15 is the bit flag ImGuiDockNodeFlags_NoCloseButton
        // Because both of these come from imgui_internal.h they are not exposed
        // thorugh ImGui.NET, but we can still use them as hardcoded values.
        private const int dockNodeFlags = (1 << 14) | (1 << 15);
    }
}
