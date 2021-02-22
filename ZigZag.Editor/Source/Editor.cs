using System;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using ZigZag.Core;
using ZigZag.Editor.Ui.Windows;
using ZigZag.Editor.Ui;
using ZigZag.Editor.WiringEditor;
using ZigZag.Editor.ImGuiIntegration;
using ZigZag.Mathematics;
using ZigZag.Runtime;
using GLFW = OpenTK.Windowing.GraphicsLibraryFramework.GLFW;


namespace ZigZag.Editor
{
    public sealed class Editor : IEditor
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
                AutoLoadBindings = true,
                NumberOfSamples = 8
            };

            m_nativeWindow = new NativeWindow(nativeWindowSettings);
            m_nativeWindow.Context.MakeCurrent();
            GLFW.SwapInterval(1);

            m_nativeWindow.MouseDown += handleMousePress;
            m_nativeWindow.MouseUp += HandleMouseRelease;
            m_nativeWindow.MouseMove += HandleMouseMove;
            m_nativeWindow.MouseWheel += HandleMouseWheel;


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

            m_mainMenu = new MainMenu();
            m_hierarchyWindow = new HierarchyWindow("Hierarchy");
            m_historyWindow = new HistoryWindow("History");
            m_wiringWindow = new WiringEditorWindow("Wiring");
            m_mainMenu.HierarchyWindow = m_hierarchyWindow;
            m_mainMenu.HistoryWindow = m_historyWindow;
            m_mainMenu.WiringWindow = m_wiringWindow;
        }

        public void CloseEditor()
        {
        }

        public void ProjectChanged(Project project)
        {
            m_project = project;
            m_mainMenu.Project = project;
            m_hierarchyWindow.Project = project;
            m_historyWindow.Project = project;
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
                ImGui.SetCursorPos(new System.Numerics.Vector2(5, 5));
                ImGui.DockSpace(123, mainWindowSize - new System.Numerics.Vector2(10, 10), (ImGuiDockNodeFlags)dockNodeFlags);
            }
            ImGui.End();

            if (m_hierarchyWindow.IsOpen) m_hierarchyWindow.Draw(activeStyle);
            if (m_historyWindow.IsOpen) m_historyWindow.Draw(activeStyle);
            if (m_wiringWindow.IsOpen) m_wiringWindow.Draw(activeStyle);

            ImGui.ShowDemoWindow();
            
            ImGui.EndFrame();
            activeStyle.EndOverall();

            ImGui.Render();
            ImGuiRendererIntegration.Render(ImGui.GetDrawData(), m_nativeWindow.Size.X, m_nativeWindow.Size.Y);

            if (m_wiringWindow.IsVisible)
            {
                m_wiringWindow.Render(m_nativeWindow.Size.X, m_nativeWindow.Size.Y);
            }

            m_nativeWindow.Context.SwapBuffers();
        }

        public bool WantsToClose()
        {
            return m_nativeWindow.IsExiting;
        }

        public void handleMousePress(MouseButtonEventArgs mouseDownEvent)
        {
            if (m_wiringWindow.ContentArea.Contains(m_mousePosition))
            {
                switch (mouseDownEvent.Button)
                {
                    case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left:
                        m_wiringWindow.m_scene.MouseButtonPress(SceneGraph.MouseButton.Left);
                        break;
                    case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Middle:
                        m_wiringWindow.m_scene.MouseButtonPress(SceneGraph.MouseButton.Middle);
                        break;
                    case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Right:
                        m_wiringWindow.m_scene.MouseButtonPress(SceneGraph.MouseButton.Right);
                        break;
                }
            }
        }

        public void HandleMouseRelease(MouseButtonEventArgs mouseUpEvent)
        {
            switch (mouseUpEvent.Button)
            {
                case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left:
                    m_wiringWindow.m_scene.MouseButtonRelease(SceneGraph.MouseButton.Left);
                    break;
                case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Middle:
                    m_wiringWindow.m_scene.MouseButtonRelease(SceneGraph.MouseButton.Middle);
                    break;
                case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Right:
                    m_wiringWindow.m_scene.MouseButtonRelease(SceneGraph.MouseButton.Right);
                    break;
            }
        }

        public void HandleMouseMove(MouseMoveEventArgs mouseMoveEvent)
        {
            m_mousePosition = new Vector2(mouseMoveEvent.X, mouseMoveEvent.Y);
            m_wiringWindow.m_scene.MouseMovement(
                mouseMoveEvent.X - m_wiringWindow.ContentPos.X,
                mouseMoveEvent.Y - m_wiringWindow.ContentPos.Y);
        }

        public void HandleMouseWheel(MouseWheelEventArgs mouseWheelEvent)
        {
            if (m_wiringWindow.ContentArea.Contains(m_mousePosition))
            {
                m_wiringWindow.m_scene.MouseWheel(mouseWheelEvent.OffsetY - m_mouseWheel);
                m_mouseWheel = mouseWheelEvent.OffsetY;
            }
        }

        private NativeWindow m_nativeWindow;
        private IntPtr m_imguiContext;

        private Project m_project;

        private MainMenu m_mainMenu;
        private HierarchyWindow m_hierarchyWindow;
        private HistoryWindow m_historyWindow;
        private WiringEditorWindow m_wiringWindow;

        private float m_mouseWheel;
        private Vector2 m_mousePosition;

        // Very hacky: 
        // 1 << 14 is the bit flag ImGuiDockNodeFlags_NoWindowMenuButton
        // 1 << 15 is the bit flag ImGuiDockNodeFlags_NoCloseButton
        // Because both of these come from imgui_internal.h they are not exposed
        // thorugh ImGui.NET, but we can still use them as hardcoded values.
        private const int dockNodeFlags = (1 << 14) | (1 << 15);
    }
}
