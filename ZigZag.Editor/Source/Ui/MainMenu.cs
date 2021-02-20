using ImGuiNET;
using ZigZag.Core;
using ZigZag.Editor.WiringEditor;
using ZigZag.Editor.Ui.Windows;


namespace ZigZag.Editor.Ui
{
    class MainMenu
    {
        public MainMenu()
        {
        }

        public Project Project
        {
            get;
            set;
        }

        public WiringEditorWindow WiringWindow
        {
            get;
            set;
        }

        public HierarchyWindow HierarchyWindow
        {
            get;
            set;
        }

        public HistoryWindow HistoryWindow
        {
            get;
            set;
        }


        public void Draw(Style style)
        {
            style.BeginMainMenu(this);
            ImGui.BeginMainMenuBar();

            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Save", "CTRL+S")) { }
                if (ImGui.MenuItem("Open", "CTRL+O")) { }
                if (ImGui.MenuItem("New", "CTRL+N")) { }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Edit"))
            {
                if (ImGui.MenuItem("Undo", "CTRL+Z")) { }
                if (ImGui.MenuItem("Redo", "CTRL+Y", false, false)) { }  // Disabled item
                ImGui.Separator();
                if (ImGui.MenuItem("Cut", "CTRL+X")) { }
                if (ImGui.MenuItem("Copy", "CTRL+C")) { }
                if (ImGui.MenuItem("Paste", "CTRL+V")) { }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("View"))
            {
                WindowMenuItem("Wiring", WiringWindow);
                WindowMenuItem("Node Hierarchy", HierarchyWindow);
                WindowMenuItem("History", HistoryWindow);
                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();
            style.EndMainMenu(this);
        }

        private void WindowMenuItem(string text, DockableWindow window)
        {
            if (ImGui.MenuItem(text, "", window.IsOpen))
            {
                if (window.IsOpen)
                {
                    ImGui.SetWindowFocus(window.Name);
                }
                else
                {
                    window.IsOpen = true;
                }
            }
        }

    }
}
