using System.Collections.Generic;
using ImGuiNET;


namespace ZigZag.Editor.Ui
{
    class MainMenu
    {
        public MainMenu(Dictionary<string, DockableWindow> windows)
        {
            m_windows = windows;
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
                if (ImGui.MenuItem("Node Hierarchy", "", m_windows.ContainsKey("Node Hierarchy")))
                {
                    if (m_windows.ContainsKey("Node Hierarchy"))
                    {
                        // take focus
                    }
                    else
                    {
                        m_windows.Add("Node Hierarchy", new Windows.HierarchyWindow("Node Hierarchy"));
                    }
                }
                if (ImGui.MenuItem("History", "", m_windows.ContainsKey("History")))
                {
                    if (m_windows.ContainsKey("History"))
                    {
                        // take focus
                    }
                    else
                    {
                        m_windows.Add("History", new Windows.HistoryWindow("History"));
                    }
                }
                ImGui.EndMenu();
            }

            ImGui.EndMainMenuBar();
            style.EndMainMenu(this);
        }

        private Dictionary<string, DockableWindow> m_windows;
    }
}
