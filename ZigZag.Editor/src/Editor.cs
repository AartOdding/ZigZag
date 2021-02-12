using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Windowing.Desktop;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ZigZag.Core;
using ZigZag.Editor.Ui.Windows;
using ZigZag.Editor.Ui;
using ZigZag.Runtime;
using ZigZag.SceneGraph;
using ZigZag.Mathematics;


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
            Vector2 v1 = new Vector2(1, 0);
            Transform2D t1 = Transform2D.CreateTranslation(10, 15);
            Transform2D t2 = Transform2D.CreateScale(1.5f, 1.5f);
            Transform2D t3 = Transform2D.CreateRotation(1.55f);

            Console.WriteLine(t1 * v1);
            Console.WriteLine(t2 * v1);
            Console.WriteLine(t3 * v1);

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

            m_nativeWindow.MouseDown += args =>
            {
                Console.WriteLine($"Pressed: {args.Button}");

                switch (args.Button)
                {
                    case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left:
                        m_nodeGraphWindow.m_scene.SetMouseButton(ZigZag.SceneGraph.MouseButton.Left, true);
                        break;
                    case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Middle:
                        m_nodeGraphWindow.m_scene.SetMouseButton(ZigZag.SceneGraph.MouseButton.Middle, true);
                        break;
                    case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Right:
                        m_nodeGraphWindow.m_scene.SetMouseButton(ZigZag.SceneGraph.MouseButton.Right, true);
                        break;
                }
            };

            m_nativeWindow.MouseUp += args =>
            {
                Console.WriteLine($"Released: {args.Button}");

                switch (args.Button)
                {
                    case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left:
                        m_nodeGraphWindow.m_scene.SetMouseButton(ZigZag.SceneGraph.MouseButton.Left, false);
                        break;
                    case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Middle:
                        m_nodeGraphWindow.m_scene.SetMouseButton(ZigZag.SceneGraph.MouseButton.Middle, false);
                        break;
                    case OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Right:
                        m_nodeGraphWindow.m_scene.SetMouseButton(ZigZag.SceneGraph.MouseButton.Right, false);
                        break;
                }
            };

            m_nativeWindow.MouseMove += args =>
            {
                m_nodeGraphWindow.m_scene.SetMousePosition(args.X - m_nodeGraphWindow.ContentPos.X, args.Y - m_nodeGraphWindow.ContentPos.Y);
            };

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
            m_nodeGraphWindow = new NodeGraphWindow("Node Graph");
            m_mainMenu.HierarchyWindow = m_hierarchyWindow;
            m_mainMenu.HistoryWindow = m_historyWindow;
            m_mainMenu.NodeGraphWindow = m_nodeGraphWindow;

            GeometryBuilder builder = new GeometryBuilder();
            builder.AddRectangle(new Mathematics.Rectangle(50, 50, 200, 200));
            builder.AddEllipse(new Mathematics.Vector2(300, 300), 150, 150);
            m_geometry = builder.Build();
            m_drawer = new SceneGraph.GeometryDrawer();
            m_drawer.AddGeometry(1, ref m_geometry, 500);
        }

        Geometry m_geometry;
        SceneGraph.GeometryDrawer m_drawer;

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
            if (m_nodeGraphWindow.IsOpen) m_nodeGraphWindow.Draw(activeStyle);

            ImGui.ShowDemoWindow();
            
            ImGui.EndFrame();
            activeStyle.EndOverall();

            ImGui.Render();
            ImGuiRendererIntegration.Render(ImGui.GetDrawData(), m_nativeWindow.Size.X, m_nativeWindow.Size.Y);

            //m_nodeGraphWindow.Render();
            m_drawer.Draw(m_nativeWindow.Size.X, m_nativeWindow.Size.Y);

            m_nativeWindow.Context.SwapBuffers();
        }

        public bool WantsToClose()
        {
            return m_nativeWindow.IsExiting;
        }


        private NativeWindow m_nativeWindow;
        private IntPtr m_imguiContext;

        private Project m_project;

        private MainMenu m_mainMenu;
        private HierarchyWindow m_hierarchyWindow;
        private HistoryWindow m_historyWindow;
        private NodeGraphWindow m_nodeGraphWindow;

        // Very hacky: 
        // 1 << 14 is the bit flag ImGuiDockNodeFlags_NoWindowMenuButton
        // 1 << 15 is the bit flag ImGuiDockNodeFlags_NoCloseButton
        // Because both of these come from imgui_internal.h they are not exposed
        // thorugh ImGui.NET, but we can still use them as hardcoded values.
        private const int dockNodeFlags = (1 << 14) | (1 << 15);
    }
}
